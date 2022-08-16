using Npgsql;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Kurs.Models
{
	static class TourDB
	{
		static private string pass;
		static private string connectionArgs;

		static private short FIELDS_SITE = 3;
		static private short FIELDS_TTYPE = 5;
		static private short FIELDS_CLIENT = 5;
		static private short FIELDS_GUIDE = 7;
		static private short FIELDS_TOUR = 3;
		public static void Init(string _pass)
		{
			pass = (_pass.Trim().Length != 0 ? _pass : "1");
			connectionArgs = $"Host=localhost; Username=postgres; Password={pass}; Database=tourdb";
		}
		public static bool Check()
		{
			using (NpgsqlConnection con = new NpgsqlConnection(connectionArgs))
			{
				var canlogin = true;
				try
				{
					con.Open();
				}
				catch (PostgresException e)
				{
					if (e.Message.Contains("\"tourdb\" does not exist"))
					{
						var new_con = new NpgsqlConnection($"Host=localhost; Username=postgres; Password={pass}");
						new_con.Open();
						var cmd = new NpgsqlCommand(@"
						CREATE DATABASE tourdb
						WITH OWNER = postgres
						ENCODING = 'UTF8'
						CONNECTION LIMIT = -1;", new_con);
						cmd.ExecuteNonQuery();

						new_con.Close();
						new_con = new NpgsqlConnection(connectionArgs);
						new_con.Open();
						cmd.Connection = new_con;

								cmd.CommandText = @"DROP SCHEMA public CASCADE;
									CREATE SCHEMA public;GRANT ALL ON SCHEMA public TO postgres;
									GRANT ALL ON SCHEMA public TO public; SET DATESTYLE TO 'ISO, DMY'";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE clients(id INTEGER, name VARCHAR(64) NOT NULL,
								surname VARCHAR(64) NOT NULL, idcode VARCHAR(16) NOT NULL,
								status VARCHAR(48) NOT NULL DEFAULT 'Common',
								CONSTRAINT pk_id PRIMARY KEY(id));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE guides(id INTEGER PRIMARY KEY, name VARCHAR(64) NOT NULL,
								surname VARCHAR(64) NOT NULL, idcode VARCHAR(16) NOT NULL,
								available BOOLEAN DEFAULT true, employ_date DATE DEFAULT CURRENT_DATE, birth_date DATE);";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE sites(name VARCHAR(64) PRIMARY KEY, address VARCHAR(96) NOT NULL, quality INTEGER CHECK (quality >= 0 AND quality <= 10));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE ttypes(id INTEGER PRIMARY KEY, name VARCHAR(64) NOT NULL,
								price INTEGER NOT NULL CHECK (price >= 0), max_participants INTEGER NOT NULL CHECK (max_participants > 0), duration BIGINT NOT NULL CHECK (duration > 0));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE tours(id INTEGER PRIMARY KEY, ttype_id INTEGER, start_datetime TIMESTAMP,
								CONSTRAINT fk_ttype FOREIGN KEY(ttype_id) REFERENCES ttypes(id));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE tour_client(tour_id INTEGER NOT NULL, client_id INTEGER NOT NULL,
								PRIMARY KEY(tour_id, client_id),
								CONSTRAINT fk_tour_id FOREIGN KEY(tour_id) REFERENCES tours(id),
								CONSTRAINT fk_client_id FOREIGN KEY(client_id) REFERENCES clients(id));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE tour_guide(tour_id INTEGER NOT NULL, guide_id INTEGER NOT NULL,
								PRIMARY KEY(tour_id, guide_id),
								CONSTRAINT fk_tour_id FOREIGN KEY(tour_id) REFERENCES tours(id),
								CONSTRAINT fk_guide_id FOREIGN KEY(guide_id) REFERENCES guides(id));";
								cmd.ExecuteNonQuery();

								cmd.CommandText = @"CREATE TABLE ttype_site(ttype_id INTEGER NOT NULL, site_name VARCHAR(64) NOT NULL,
								PRIMARY KEY(ttype_id, site_name),
								CONSTRAINT fk_ttype_id FOREIGN KEY(ttype_id) REFERENCES ttypes(id),
								CONSTRAINT fk_site_name FOREIGN KEY(site_name) REFERENCES sites(name));";
								cmd.ExecuteNonQuery();

						new_con.Close();
					}

					if (e.Message.Contains("password"))
						canlogin = false;
				}
				finally
				{
					con.Close();
				}
				return canlogin;
			}
		}

		public static List<Tour> GetTourList()
		{
			var tList = new List<Tour>();

			using(var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, ttype_id, start_datetime FROM tours;";

				var dr = cmd.ExecuteReader();

				while(dr.Read())
				{
					object[] row = new object[FIELDS_TOUR];
					dr.GetValues(row);

					var tourID = dr.GetInt16(0);

					tList.Add(new Tour(tourID, GetTourType(dr.GetInt16(1)),
									GetClientList(tourID), GetGuideList(tourID),
									dr.GetDateTime(2)));
				}
			}

			return tList;
		}

		public static List<Tour> GetTourList(Guide g)
		{
			var tList = new List<Tour>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, ttype_id, start_datetime FROM tours, tour_guide " +
									@"WHERE tour_id=id AND guide_id=@id;";

				cmd.Parameters.AddWithValue("id", g.Id);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_TOUR];
					dr.GetValues(row);

					var tourID = dr.GetInt16(0);
					tList.Add(new Tour(tourID, GetTourType(dr.GetInt16(1)),
									GetClientList(tourID), GetGuideList(tourID),
									dr.GetDateTime(2)));
				}
			}

			return tList;
		}

		public static List<Tour> GetTourList(DateTime from, DateTime to)
		{
			var tList = new List<Tour>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, ttype_id, start_datetime FROM tours " +
									@"WHERE start_datetime>=@from AND start_datetime<=@to;";
				cmd.Parameters.AddWithValue("from", from);
				cmd.Parameters.AddWithValue("to", to);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_TOUR];
					dr.GetValues(row);

					var tourID = dr.GetInt16(0);
					tList.Add(new Tour(tourID, GetTourType(dr.GetInt16(1)),
									GetClientList(tourID), GetGuideList(tourID),
									dr.GetDateTime(2)));
				}
			}

			return tList;
		}

		public static List<TourType> GetTourTypeList()
		{
			var ttList = new List<TourType>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, name, price, max_participants, duration FROM ttypes;";

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_TTYPE];
					dr.GetValues(row);

					var id = dr.GetInt16(0);

					ttList.Add(new TourType(id, dr.GetString(1), dr.GetInt64(2), dr.GetInt16(3), new TimeSpan(dr.GetInt64(4)), GetSiteList(id)));
				}
			}

			return ttList;
		}

		public static List<Site> GetSiteList()
		{
			var sList = new List<Site>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT name, address, quality FROM sites;";

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_SITE];
					dr.GetValues(row);

					sList.Add(new Site(dr.GetString(0), dr.GetString(1), dr.GetInt16(2)));
				}
			}

			return sList;
		}

		public static List<Site> GetSiteList(Int16 id)
		{
			var sList = new List<Site>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT name, address, quality FROM sites, ttype_site " +
									@"WHERE name=site_name AND ttype_id=@ttid;";

				cmd.Parameters.AddWithValue("ttid", id);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_SITE];
					dr.GetValues(row);

					sList.Add(new Site(dr.GetString(0), dr.GetString(1), dr.GetInt16(2)));
				}
			}

			return sList;
		}

		public static List<Guide> GetGuideList()
		{
			var gList = new List<Guide>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, name, surname, idcode, available, employ_date, birth_date FROM guides;";
				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_GUIDE];
					dr.GetValues(row);

					gList.Add(new Guide(dr.GetInt16(0), dr.GetString(1), dr.GetString(2),
								dr.GetString(3), dr.GetDateTime(5), dr.GetDateTime(6), dr.GetBoolean(4)));
				}
			}

			return gList;
		}

		public static List<Guide> GetGuideList(Int16 id)
		{
			var gList = new List<Guide>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, name, surname, idcode, available, employ_date, birth_date FROM guides, tour_guide " +
									@"WHERE guide_id=id AND tour_id=@tid;";

				cmd.Parameters.AddWithValue("tid", id);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_GUIDE];
					dr.GetValues(row);

					gList.Add(new Guide(dr.GetInt16(0), dr.GetString(1), dr.GetString(2),
								dr.GetString(3), dr.GetDateTime(5), dr.GetDateTime(6), dr.GetBoolean(4)));
				}
			}

			return gList;
		}

		public static List<Client> GetClientList()
		{
			var cList = new List<Client>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, name, surname, idcode, status FROM clients;";
				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_CLIENT];
					dr.GetValues(row);

					cList.Add(new Client(dr.GetInt16(0), dr.GetString(1), dr.GetString(2),
								dr.GetString(3), (ClientStatus)Enum.Parse(typeof(ClientStatus), dr.GetString(4))));
				}
			}

			return cList;
		}

		public static List<Client> GetClientList(Int16 id)
		{
			var cList = new List<Client>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, name, surname, idcode, status FROM clients, tour_client " +
									@"WHERE client_id=id AND tour_id=@tid;";

				cmd.Parameters.AddWithValue("tid", id);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_CLIENT];
					dr.GetValues(row);

					cList.Add(new Client(dr.GetInt16(0), dr.GetString(1), dr.GetString(2),
								dr.GetString(3), (ClientStatus)Enum.Parse(typeof(ClientStatus), dr.GetString(4))));
				}
			}

			return cList;
		}

		public static TourType GetTourType(Int16 id)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = "SELECT id, name, price, max_participants, duration FROM ttypes " +
									"WHERE id = @ttid;";
				cmd.Parameters.AddWithValue("ttid", id);
				cmd.Prepare();

				var dr = cmd.ExecuteReader();

				dr.Read();
				var ttid = dr.GetInt16(0);
				return new TourType(ttid, dr.GetString(1), dr.GetInt64(2), dr.GetInt16(3), new TimeSpan(dr.GetInt64(4)), GetSiteList(ttid));
			}
		}

		public static void AddTour(Tour t)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand(@"SET DateStyle TO 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				cmd.CommandText = @"INSERT INTO tours(id, ttype_id, start_datetime) " +
							$"VALUES (@id, @ttype_id, @start_datetime);";
				cmd.Parameters.AddWithValue("id", t.Id);
				cmd.Parameters.AddWithValue("ttype_id", t.TourType.Id);
				cmd.Parameters.AddWithValue("start_datetime", t.StartDate);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AddClient(Client c)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);

				cmd.CommandText = @"INSERT INTO clients(id, name, surname, idcode, status) " +
							$"VALUES (@id, @name, @surname, @idcode, @status);";
				cmd.Parameters.AddWithValue("id", c.Id);
				cmd.Parameters.AddWithValue("name", c.Name);
				cmd.Parameters.AddWithValue("surname", c.Surname);
				cmd.Parameters.AddWithValue("idcode", c.IDCode);
				cmd.Parameters.AddWithValue("status", c.StatusName);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AddGuide(Guide g)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("SET DateStyle TO 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				cmd.CommandText = @"INSERT INTO guides(id, name, surname, idcode, available, employ_date, birth_date) " +
							$"VALUES (@id, @name, @surname, @idcode, @status, @employ_date, @birth_date);";
				cmd.Parameters.AddWithValue("id", g.Id);
				cmd.Parameters.AddWithValue("name", g.Name);
				cmd.Parameters.AddWithValue("surname", g.Surname);
				cmd.Parameters.AddWithValue("idcode", g.IDCode);
				cmd.Parameters.AddWithValue("status", g.IsAvailable);
				cmd.Parameters.AddWithValue("employ_date", g.EmploymentDate);
				cmd.Parameters.AddWithValue("birth_date", g.BirthDate);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AddTourType(TourType tt)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand(@"SET DateStyle TO 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				cmd.CommandText = @"INSERT INTO ttypes(id, name, price, max_participants, duration) " +
							$"VALUES (@id, @name, @price, @max_participants, @duration);";
				cmd.Parameters.AddWithValue("id", tt.Id);
				cmd.Parameters.AddWithValue("name", tt.Name);
				cmd.Parameters.AddWithValue("price", tt.Price);
				cmd.Parameters.AddWithValue("max_participants", tt.MaxParticipants);
				cmd.Parameters.AddWithValue("duration", tt.Duration.Ticks);
				cmd.Prepare();

				cmd.ExecuteNonQuery();

				foreach(var s in tt.SiteList)
				{
					AddSiteToTourType(tt.Id, s.SiteName);
				}
			}
		}

		public static void AddSite(Site s)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);

				cmd.CommandText = @"INSERT INTO sites(name, address, quality) " +
							$"VALUES (@name, @address, @quality);";
				cmd.Parameters.AddWithValue("name", s.SiteName);
				cmd.Parameters.AddWithValue("address", s.Address);
				cmd.Parameters.AddWithValue("quality", s.Quality);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void RemoveTour(Int16 id)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"DELETE FROM tour_client WHERE tour_id=@id; " +
								  @"DELETE FROM tour_guide WHERE tour_id=@id; " +
								  @"DELETE FROM tours WHERE id=@id;";
				cmd.Parameters.AddWithValue("id", id);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void RemoveClient(Int16 id)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"DELETE FROM tour_client WHERE client_id=@id; " +
									@"DELETE FROM clients WHERE id=@id;";
				cmd.Parameters.AddWithValue("id", id);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void RemoveGuide(Int16 id)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"DELETE FROM tour_guide WHERE guide_id=@id; " +
									@"DELETE FROM guides WHERE id=@id;";
				cmd.Parameters.AddWithValue("id", id);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static int NextTourId()
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT Max(id) AS next FROM tours;";

				short id;
				return short.TryParse(cmd.ExecuteScalar().ToString(), out id) ? id + 1 : 0;
			}
		}

		public static int NextClientId()
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT Max(id) AS next FROM clients;";

				short id;
				return short.TryParse(cmd.ExecuteScalar().ToString(), out id) ? id + 1 : 0;
			}
		}

		public static int NextGuideId()
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT Max(id) AS next FROM guides;";

				short id;
				return short.TryParse(cmd.ExecuteScalar().ToString(), out id) ? id + 1 : 0;
			}
		}

		public static int NextTourTypeId()
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT Max(id) AS next FROM ttypes;";

				short id;
				return short.TryParse(cmd.ExecuteScalar().ToString(), out id) ? id + 1 : 0;
			}
		}

		public static long GetRevenue(DateTime from, DateTime to)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("SET DATESTYLE TO 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				var perTour = @"SELECT Sum(ttypes.price) AS rev_i " +
								@"FROM ((tours INNER JOIN ttypes ON tours.ttype_id = ttypes.id) INNER JOIN tour_client ON tours.id = tour_client.tour_id) " +
								@"GROUP BY tours.id " +
								@"HAVING start_datetime>=@from AND start_datetime<=@to ";

				cmd.CommandText = $"SELECT Sum(rev_i) AS Revenue FROM ({perTour}) AS pertour;";
				cmd.Parameters.AddWithValue("from", from);
				cmd.Parameters.AddWithValue("to", to);
				cmd.Prepare();

				long rev;
				return long.TryParse(cmd.ExecuteScalar().ToString(), out rev) ? rev : 0;
			}
		}

		public static Dictionary<string, long> GetRevenueByTourType(DateTime from, DateTime to)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("SET DATESTYLE TO 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				var perTour = @"SELECT ttypes.name, Sum(ttypes.price) AS rev_i " +
								@"FROM ((tours INNER JOIN ttypes ON tours.ttype_id = ttypes.id) INNER JOIN tour_client ON tours.id = tour_client.tour_id) " +
								@"GROUP BY tours.id, ttypes.name " +
								@"HAVING start_datetime>=@from AND start_datetime<=@to ";

				cmd.CommandText = $"SELECT name, Sum(rev_i) AS rev_i FROM ({perTour}) AS pertour GROUP BY name;";
				cmd.Parameters.AddWithValue("from", from);
				cmd.Parameters.AddWithValue("to", to);
				cmd.Prepare();

				var map = new Dictionary<string, long>();
				var dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					map.Add(dr.GetString(0), dr.GetInt64(1));
				}

				return map;
			}
		}

		public static void RemoveClientFromTour(Int16 tid, Int16 cid)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"DELETE FROM tour_client WHERE tour_id=@tid AND client_id=@cid;";
				cmd.Parameters.AddWithValue("tid", tid);
				cmd.Parameters.AddWithValue("cid", cid);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AddClientToTour(Int16 tid, Int16 cid)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"INSERT INTO tour_client(tour_id, client_id) VALUES (@tid, @cid);";
				cmd.Parameters.AddWithValue("tid", tid);
				cmd.Parameters.AddWithValue("cid", cid);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static List<Tour> GetTourListWithoutGuide()
		{
			

			var tList = new List<Tour>();

			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"SELECT id, ttype_id, start_datetime FROM tours LEFT JOIN tour_guide ON id=tour_id WHERE guide_id IS NULL;";

				var dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					object[] row = new object[FIELDS_TOUR];
					dr.GetValues(row);

					var tourID = dr.GetInt16(0);

					//Client and Guide lists are empty here for performance; they are not needed within the call's context
					tList.Add(new Tour(tourID, GetTourType(dr.GetInt16(1)),
									new List<Client>(), new List<Guide>(),
									dr.GetDateTime(2)));
				}
			}

			return tList;
		}

		public static void UnassignGuide(Int16 tid, Int16 gid)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"DELETE FROM tour_guide WHERE tour_id=@tid AND guide_id=@gid;";
				cmd.Parameters.AddWithValue("tid", tid);
				cmd.Parameters.AddWithValue("gid", gid);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AssignGuide(Int16 tid, Int16 gid)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"INSERT INTO tour_guide(tour_id, guide_id) VALUES (@tid, @gid);";
				cmd.Parameters.AddWithValue("tid", tid);
				cmd.Parameters.AddWithValue("gid", gid);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static void AddSiteToTourType(Int16 ttid, string sname)
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();

				var cmd = new NpgsqlCommand("", con);
				cmd.CommandText = @"INSERT INTO ttype_site(ttype_id, site_name) VALUES (@ttid, @sname);";
				cmd.Parameters.AddWithValue("ttid", ttid);
				cmd.Parameters.AddWithValue("sname", sname);
				cmd.Prepare();

				cmd.ExecuteNonQuery();
			}
		}

		public static string ReportRevenue(DateTime from, DateTime to)
		{
			var doc = new Document();
			var sec = doc.AddSection();

			var revenue = GetRevenue(from, to);
			var footer = sec.HeadersFooters.Footer.AddParagraph();
			var EF = footer.AppendText($"Total Revenue: EUR{revenue / 100}.{revenue % 100}");
			EF.CharacterFormat.FontSize = 16;
			EF.CharacterFormat.Bold = true;
			footer.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;

			AddHeaders(ref sec);

			var table = sec.AddTable();
			var row = table.AddRow(4);
			row.IsHeader = true;
			var ttype_header = new string[] { "Tour Type", "Per Person", "Max Participants", "Total Earned"};
			for (int i = 0; i < row.Cells.Count; ++i)
			{
				var p = row.Cells[i].AddParagraph();
				p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;

				var tr = p.AppendText(ttype_header[i]);
				tr.CharacterFormat.FontSize = 16;
				tr.CharacterFormat.Bold = true;
			}
			row.Cells[3].FirstParagraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;

			var tlist = GetTourList(from, to);
			var map = GetRevenueByTourType(from, to);
			foreach (var ttype in new HashSet<TourType>(tlist.Select(t => t.TourType)))
			{
				long rev_i;
				if (map.TryGetValue(ttype.Name, out rev_i))
				{
					table.Rows.Add(table.FirstRow.Clone());
					row = table.LastRow;
					foreach(TableCell cell in row.Cells)
						cell.FirstParagraph.Text = "";

					row.Cells[0].FirstParagraph.AppendText($"{ttype.Name}").CharacterFormat.Bold = true;
					row.Cells[1].FirstParagraph.AppendText($"{ttype.PriceString}").CharacterFormat.Bold = true;
					row.Cells[2].FirstParagraph.AppendText($"{ttype.MaxParticipants}").CharacterFormat.Bold = true;
					row.Cells[3].FirstParagraph.AppendText($"EUR{rev_i / 100}.{rev_i % 100}").CharacterFormat.Bold = true;

					table.Rows.Add(table.FirstRow.Clone());
					row = table.LastRow;
					foreach(TableCell cell in row.Cells)
						cell.FirstParagraph.Text = "";

					var tour_header = new string[] { "Tour ID", "Start Date and Time", "Participated", "Earned" };
					for (int i = 0; i < row.Cells.Count; ++i)
					{
						var tr = row.Cells[i].FirstParagraph.AppendText(tour_header[i]);
						tr.CharacterFormat.FontSize = 12;
						tr.CharacterFormat.Bold = true;
					}

					foreach(var tour in tlist.Where(tour => tour.TourTypeName.Contains(ttype.Name)))
					{
						Console.WriteLine(tour.Id);

						table.Rows.Add(table.LastRow.Clone());
						row = table.LastRow;
						foreach (TableCell cell in row.Cells)
							cell.FirstParagraph.Text = "";

						row.Cells[0].FirstParagraph.AppendText($"{tour.Id}").CharacterFormat.Bold = false;
						row.Cells[1].FirstParagraph.AppendText($"{tour.StartDate}").CharacterFormat.Bold = false;
						row.Cells[2].FirstParagraph.AppendText($"{tour.ClientList.Count}").CharacterFormat.Bold = false;
						row.Cells[3].FirstParagraph.AppendText($"EUR{ttype.Price* tour.ClientList.Count / 100}.{ttype.Price * tour.ClientList.Count % 100}").CharacterFormat.Bold = false;
					}
				}
			}

            var fname = AppDomain.CurrentDomain.BaseDirectory + $"revenue-report{DateTime.Now:ddMMyyyy_HH-mm-ss}.docx";
            doc.SaveToFile(fname, FileFormat.Docx);
			return $"Successfuly created file:\n{fname}";
		}

		private static void AddHeaders(ref Section sec)
		{
			var header = sec.HeadersFooters.Header.AddParagraph();
			header.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
			header.Text = "Tour Agency Database System Report\nRevenue";

			var footer = sec.HeadersFooters.Footer.AddParagraph();
			footer.AppendField("page number", FieldType.FieldPage);
			footer.AppendText(" of ");
			footer.AppendField("number of pages", FieldType.FieldNumPages);
			footer.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
		}

		//used for reallocation of records from filesystem to postgreSQL tables
		public static void Save()
		{
			using (var con = new NpgsqlConnection(connectionArgs))
			{
				con.Open();
				var cmd = new NpgsqlCommand("SET DateStyle to 'ISO, DMY';", con);
				cmd.ExecuteNonQuery();

				foreach(var c in ObjectPool.ClientList)
				{
					cmd.CommandText = @"INSERT INTO clients(id, name, surname, idcode, status) " +
						$"VALUES ({c.Id}, '{c.Name}', '{c.Surname}', '{c.IDCode}', '{c.StatusName}');";
					cmd.ExecuteNonQuery();
				}

				foreach (var g in ObjectPool.GuideList)
				{
					cmd.CommandText = @"INSERT INTO guides(id, name, surname, idcode, available, employ_date, birth_date) " +
						$"VALUES ({g.Id}, '{g.Name}', '{g.Surname}', '{g.IDCode}', '{g.IsAvailable}', '{g.EmploymentDate:dd-MM-yyyy}', '{g.BirthDate:dd-MM-yyyy}');";
					cmd.ExecuteNonQuery();
				}

				foreach (var s in ObjectPool.SiteList)
				{
					cmd.CommandText = @"INSERT INTO sites(name, address, quality) " +
						$"VALUES ('{s.SiteName}', '{s.Address}', {s.Quality});";
					cmd.ExecuteNonQuery();
				}

				foreach (var tt in ObjectPool.TourTypeList)
				{
					cmd.CommandText = @"INSERT INTO ttypes(id, name, price, max_participants, duration) " +
						$"VALUES ({tt.Id}, '{tt.Name}', {tt.Price}, {tt.MaxParticipants}, {tt.Duration.Ticks});";
					cmd.ExecuteNonQuery();

					foreach(var s in tt.SiteList)
					{
						cmd.CommandText = @"INSERT INTO ttype_site(ttype_id, site_name)" +
							$"VALUES ({tt.Id}, '{s.SiteName}');";
						cmd.ExecuteNonQuery();
					}
				}

				foreach (var t in ObjectPool.TourList)
				{
					cmd.CommandText = @"INSERT INTO tours(id, ttype_id, start_datetime) " +
						$"VALUES ({t.Id}, {t.TourType.Id}, '{t.StartDate:yyyy-MM-dd HH:mm:ss}');";
					cmd.ExecuteNonQuery();

					foreach (var c in t.ClientList)
					{
						cmd.CommandText = @"INSERT INTO tour_client(tour_id, client_id)" +
							$"VALUES ({t.Id}, {c.Id});";
						cmd.ExecuteNonQuery();
					}

					foreach (var g in t.GuideList)
					{
						cmd.CommandText = @"INSERT INTO tour_guide(tour_id, guide_id)" +
							$"VALUES ({t.Id}, {g.Id});";
						cmd.ExecuteNonQuery();
					}
				}
			}
		}
	}
}
