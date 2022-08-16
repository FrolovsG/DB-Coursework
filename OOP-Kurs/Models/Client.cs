using System;
using System.IO;
using System.Text;

namespace OOP_Kurs.Models
{
    public enum ClientStatus { Common, VIP, Pensioner };

    public class Client : Person
    {
        private ClientStatus status;
        public ClientStatus Status { get => status; set => status = value; }
        public string StatusName { get => Enum.GetName(typeof(ClientStatus), Status); }
    
        public Client(short id, string name, string surname, string idCode, ClientStatus status) : base(id, name, surname, idCode)
        {
            this.status = status;
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
                    bw.Write(StatusName);
                }

                return m.ToArray();
            }
        }

        public static Client Deserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var br = new BinaryReader(m, Encoding.UTF8))
                {
                    return new Client(br.ReadInt16(), br.ReadString(), br.ReadString(), br.ReadString(), (ClientStatus)Enum.Parse(typeof(ClientStatus), br.ReadString()));
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
                var c = (Client)obj;
                return (base.Equals(c) &&
                        Status == c.Status
                        );
            }
        }


    }
}
