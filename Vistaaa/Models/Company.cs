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
        [MaxLength(254)]
        public string Email { get; set; } = "";
        [MaxLength(255)]
        public string Password { get; set; } = "";
        [MaxLength(50)]
        public string Name { get; set; } = "";
        [MaxLength(1000)]
        public string? Description { get; set; } = "";
        [MaxLength(100)]
        public string StreetName { get; set; } = "";
        [MaxLength(10)]
        public string StreetNumber { get; set; } = "";
        [MaxLength(50)]
        public string City { get; set; } = "";
        [MaxLength(6)]
        public string PostalCode { get; set; } = "";
        [Ignore]
        public List<Advertisement>? Advertisements { get; set; }


        public Company(string email, string password, string name, string? description, string streetName, string streetNumber, string city, string postalCode)
        {
            Email = email;
            Password = password;
            Name = name;
            Description = description;
            StreetName = streetName;
            StreetNumber = streetNumber;
            City = city;
            PostalCode = postalCode;
        }
        public Company()
        {

        }
        public override string ToString()
        {
            return Name;
        }
    }
}
