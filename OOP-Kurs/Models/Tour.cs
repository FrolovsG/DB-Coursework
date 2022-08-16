using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OOP_Kurs.Models
{
    public enum TourStatus { Planned, InProgress, Completed };

    public class Tour
    {
        private readonly TourType tourType;
        public TourType TourType { get => tourType; }
        public string TourTypeName { get => tourType.Name; }

        public List<Client> ClientList { get; set; }
        public List<Guide> GuideList { get; set; }

        public DateTime StartDate { get; set; }
        public Int16 Id { get; set; }

        public string Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return TourStatus.Planned.ToString();
                else
                    return StartDate + tourType.Duration > DateTime.Now ? TourStatus.InProgress.ToString() : TourStatus.Completed.ToString();
            }
        }

        public Tour(short id, TourType tourType, List<Client> clientList, List<Guide> guideList, DateTime startDate)
        {
            Id = id;
            this.tourType = tourType;
            this.ClientList = clientList;
            this.GuideList = guideList;
            this.StartDate = startDate;
        }

        public override string ToString()
        {
            return $"{Id}: {TourTypeName} {StartDate}";
        }

        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using (var bw = new BinaryWriter(m, Encoding.UTF8))
                {
                    bw.Write(Id);
                    bw.Write(TourType.Id);

                    bw.Write(ClientList.Count);
                    foreach (var id in ClientList.Select(client => client.Id))
                        bw.Write(id);

                    bw.Write(GuideList.Count);
                    foreach (var id in GuideList.Select(guide => guide.Id))
                        bw.Write(id);

                    bw.Write(StartDate.Ticks);
                }

                return m.ToArray();
            }
        }

        public static Tour Deserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var br = new BinaryReader(m, Encoding.UTF8))
                {
                    List<T> GetList<T> (List<T> srclist) where T : Person
                    {
                        var list = new List<T>();
                        int count = br.ReadInt32();
                        for (int i = 0; i < count; ++i)
                        {
                            var id = br.ReadInt16();
                            list.Add(srclist.Find(obj => obj.Id == id));
                        }

                        return list;
                    }

                    var tourID = br.ReadInt16();
                    var typeID = br.ReadInt16();

                    return new Tour(tourID, ObjectPool.TourTypeList.Find(ttype => ttype.Id == typeID),
                                    GetList(ObjectPool.ClientList), GetList(ObjectPool.GuideList),
                                    new DateTime(br.ReadInt64()));
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
                var t = (Tour)obj;
                return (Id == t.Id &&
                        TourType.Equals(t.TourType) &&
                       // ClientList.SequenceEqual(t.ClientList) &&
                       // GuideList.SequenceEqual(t.GuideList) &&
                        StartDate == t.StartDate
                        );
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 845638177;
            hashCode = hashCode * -1521134295 + EqualityComparer<TourType>.Default.GetHashCode(TourType);
            hashCode = hashCode * -1521134295 + StartDate.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }
    }
}
