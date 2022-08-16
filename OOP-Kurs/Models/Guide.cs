using System;
using System.IO;
using System.Text;

namespace OOP_Kurs.Models
{
    public class Guide : Person
    {
        public bool     IsAvailable { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }

        public Guide(short id, string name, string surname, string idCode, DateTime birthDate, DateTime? employmentDate = null, bool isAvailable = true) : base(id, name, surname, idCode)
        {
            IsAvailable = isAvailable;

            BirthDate = birthDate;
            EmploymentDate = (employmentDate ?? DateTime.Now).Date;
        }

        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using (var bw = new BinaryWriter(m, Encoding.UTF8))
                {
                    bw.Write(Id);
                    bw.Write(Name);
                    bw.Write(Surname);
                    bw.Write(IDCode);
                    bw.Write(BirthDate.Ticks);
                    bw.Write(EmploymentDate.Ticks);
                    bw.Write(IsAvailable);
                }

                return m.ToArray();
            }
        }

        public static Guide Deserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var br = new BinaryReader(m, Encoding.UTF8))
                {
                    return new Guide(br.ReadInt16(), br.ReadString(), br.ReadString(), br.ReadString(),
                                     new DateTime(br.ReadInt64()), new DateTime(br.ReadInt64()), br.ReadBoolean());
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
                var g = (Guide)obj;
                return (base.Equals(g) &&
                        BirthDate == g.BirthDate &&
                        EmploymentDate == g.EmploymentDate &&
                        IsAvailable == g.IsAvailable
                        );
            }
        }

    }
}
