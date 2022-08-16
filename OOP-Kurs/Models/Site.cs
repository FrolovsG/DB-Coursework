using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OOP_Kurs.Models
{
    public class Site
    {
        public string SiteName { get; set; }
        public string Address { get; set; }

        private short quality;
        public short Quality { get { return quality; }  set { quality = Math.Max((short)0, Math.Min((short)10, value)); } }
       
        public Site(string siteName, string address, short quality)
        {
            SiteName = siteName;
            Address = address;
            Quality = quality;
        }

        public override string ToString()
        {
            return $"{SiteName}: {Address} rating:{Quality}/10";
        }

        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using (var bw = new BinaryWriter(m, Encoding.UTF8))
                {
                    bw.Write(SiteName);
                    bw.Write(Address);
                    bw.Write(quality);
                }

                return m.ToArray();
            }
        }

        public static Site Deserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var br = new BinaryReader(m, Encoding.UTF8))
                {
                    var site = new Site(br.ReadString(), br.ReadString(), br.ReadInt16());
                    return site;
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
                var s = (Site)obj;
                return (SiteName == s.SiteName &&
                        Address == s.Address &&
                        Quality == s.Quality
                        );
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 1347023278;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SiteName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + Quality.GetHashCode();
            return hashCode;
        }
    }
}
