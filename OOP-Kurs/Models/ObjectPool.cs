using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace OOP_Kurs.Models
{
    [Obsolete]
    public static class ObjectPool
    {
        private static List<Tour> tourList;
        private static List<Site> siteList;
        private static List<Client> clientList;
        private static List<Guide> guideList;
        private static List<TourType> tourTypeList;

        private static DateTime LastSaved { get; set; } = DateTime.Now;
        private static DateTime LastBackup { get; set; } = DateTime.Now;

        public static List<Site> SiteList { get => siteList; set => siteList = value; }
        public static List<Tour> TourList { get => tourList; set => tourList = value; }
        public static List<Client> ClientList { get => clientList; set => clientList = value; }
        public static List<Guide> GuideList { get => guideList; set => guideList = value; }
        public static List<TourType> TourTypeList { get => tourTypeList; set => tourTypeList = value; }

        public static void LoadData()
        {
            SiteList = new List<Site>();
            using (var br = new BinaryReader(new FileStream("dbsite.dat", FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int length = br.ReadInt32();
                    byte[] data = new byte[length];
                    br.Read(data, 0, length);

                    SiteList.Add(Site.Deserialize(data));
                }
            }

            TourTypeList = new List<TourType>();
            using (var br = new BinaryReader(new FileStream("dbttype.dat", FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int length = br.ReadInt32();
                    byte[] data = new byte[length];
                    br.Read(data, 0, length);

                    TourTypeList.Add(TourType.Deserialize(data));
                }
            }

            ClientList = new List<Client>();
            using (var br = new BinaryReader(new FileStream("dbclient.dat", FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int length = br.ReadInt32();
                    byte[] data = new byte[length];
                    br.Read(data, 0, length);

                    ClientList.Add(Client.Deserialize(data));
                }
            }

            GuideList = new List<Guide>();
            using (var br = new BinaryReader(new FileStream("dbguide.dat", FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int length = br.ReadInt32();
                    byte[] data = new byte[length];
                    br.Read(data, 0, length);

                    GuideList.Add(Guide.Deserialize(data));
                }
            }

            TourList = new List<Tour>();
            using (var br = new BinaryReader(new FileStream("dbtour.dat", FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int length = br.ReadInt32();
                    byte[] data = new byte[length];
                    br.Read(data, 0, length);

                    TourList.Add(Tour.Deserialize(data));
                }
            }
        }
        public static void SaveData()
        {
            using (var bw = new BinaryWriter(new BufferedStream(new FileStream("dbsite.dat", FileMode.Create, FileAccess.Write)), Encoding.UTF8))
            {
                foreach (var site in SiteList)
                {
                    byte[] data = site.Serialize();
                    bw.Write(data.Length);
                    bw.Write(data);
                }
            }

            using (var bw = new BinaryWriter(new BufferedStream(new FileStream("dbttype.dat", FileMode.Create, FileAccess.Write)), Encoding.UTF8))
            {
                foreach (var ttype in TourTypeList)
                {
                    byte[] data = ttype.Serialize();
                    bw.Write(data.Length);
                    bw.Write(data);
                }
            }

            using (var bw = new BinaryWriter(new BufferedStream(new FileStream("dbclient.dat", FileMode.Create, FileAccess.Write)), Encoding.UTF8))
            {
                foreach (var client in ClientList)
                {
                    byte[] data = client.Serialize();
                    bw.Write(data.Length);
                    bw.Write(data);
                }
            }

            using (var bw = new BinaryWriter(new BufferedStream(new FileStream("dbguide.dat", FileMode.Create, FileAccess.Write)), Encoding.UTF8))
            {
                foreach (var guide in GuideList)
                {
                    byte[] data = guide.Serialize();
                    bw.Write(data.Length);
                    bw.Write(data);
                }
            }

            using (var bw = new BinaryWriter(new BufferedStream(new FileStream("dbtour.dat", FileMode.Create, FileAccess.Write)), Encoding.UTF8))
            {
                foreach (var tour in TourList)
                {
                    byte[] data = tour.Serialize();
                    bw.Write(data.Length);
                    bw.Write(data);
                }
            }

            LastSaved = DateTime.Now;

            if (DateTime.Now >= LastBackup.AddHours(2))
                BackupData();
        }

        public static void TimedSaveData()
        {
            if (DateTime.Now > LastSaved.AddMinutes(10))
                SaveData();
        }

        public static void BackupData()
        {
            var workdir = Directory.GetCurrentDirectory();
            foreach (string fname in Directory.GetFiles(workdir, @"*.dat"))
            {
                Console.WriteLine(fname);
                File.Copy(fname, fname + @".bak", true);
            }

            LastBackup = DateTime.Now;
        }
    }
}
