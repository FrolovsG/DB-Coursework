using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OOP_Kurs.Models
{
	public class Person
	{
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        protected string idCode;
        public string IDCode
        {
            get => idCode;
            set
            {
                if (!Regex.Match(value, "^\\d{6}.\\d{5}$").Success)
                {
                    throw new ArgumentException("Submitted string did not match the ID Code pattern");
                }

                idCode = value;
            }
        }

        public Person(short id, string name, string surname, string idCode)
        {
            Id = id;
            Name = name;
            Surname = surname;

            try
            {
                IDCode = idCode;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public override string ToString()
        {
            return $"{Id}: {Name} {Surname} {IDCode}";
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var p = (Person)obj;
                return (Id == p.Id &&
                        Name == p.Name &&
                        Surname == p.Surname &&
                        IDCode == p.IDCode
                        );
            }
        }

        public override int GetHashCode()
        {
            int hashCode = -910096495;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IDCode);
            return hashCode;
        }
    }
}
