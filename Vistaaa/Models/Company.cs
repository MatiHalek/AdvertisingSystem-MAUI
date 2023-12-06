using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class Company
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string City { get; set; } = "";
        public string Postcode { get; set; } = "";
        [Ignore]
        public List<Advertisement>? Advertisements { get; set; }


        public Company(string name, string description, string street, string number, string city, string postcode)
        {
            Name = name;
            Description = description;
            Street = street;
            Number = number;
            City = city;
            Postcode = postcode;
        }
        public Company()
        {

        }
    }
}
