using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OOP_Kurs.Models
{
    public class TourType
    {
        public List<Site> SiteList { get; set; }

        public Int16    Id { get; set; }

        public string   Name { get; set; }
        public long   Price { get; set; }

        public string PriceString
        {
            get => $"EUR{Price / 100}.{Price % 100}";
        }

        public Int16    MaxParticipants { get; set; }

        public TimeSpan Duration { get; set; }
        public double DurationHours { get => Duration.TotalHours; }
       
        public TourType(short id, string name, long price, short maxParticipants, TimeSpan duration, List<Site> siteList)
        {
            Id = id;
            Name = name;
            Price = Math.Abs(price);
            MaxParticipants = Math.Abs(maxParticipants);
            Duration = duration;
            SiteList = siteList;
        }

        public override string ToString()
        {
            return $"{Id}: {Name} {Duration.Hours}h EUR{Price}, Max-{MaxParticipants}";
        }

        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using (var bw = new BinaryWriter(m, Encoding.UTF8))
                {
                    bw.Write(Id);
                    bw.Write(Name);
                    bw.Write(Price);
                    bw.Write(MaxParticipants);
                    bw.Write(Duration.Ticks);

                    foreach (var name in SiteList.Select(site => site.SiteName))
                        bw.Write(name);
                }

                return m.ToArray();
            }
        }

        public static TourType Deserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var br = new BinaryReader(m, Encoding.UTF8))
                {
                    Func<List<Site>> GetSiteList = () =>
                    {
                        var list = new List<Site>();
                        while (br.BaseStream.Position != br.BaseStream.Length)
                        {
                            var name = br.ReadString();
                            list.Add(ObjectPool.SiteList.Find(site => site.SiteName == name));
                        }

                        return list;
                    };

                    return new TourType(br.ReadInt16(), br.ReadString(), br.ReadInt64(), br.ReadInt16(), new TimeSpan(br.ReadInt64()), GetSiteList());
                }
            }
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var tt = (TourType)obj;
                return (Id == tt.Id &&
                        Name == tt.Name &&
                        Price == tt.Price &&
                        MaxParticipants == tt.MaxParticipants &&
                        Duration == tt.Duration &&
                        SiteList.SequenceEqual(tt.SiteList)
                        );
            }
        }

        public override int GetHashCode()
        {
            int hashCode = -1681014952;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            hashCode = hashCode * -1521134295 + MaxParticipants.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            return hashCode;
        }
    }
}
