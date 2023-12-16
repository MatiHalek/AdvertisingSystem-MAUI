using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(254)]
        public string Email { get; set; } = "";
        [MaxLength(255)]
        public string Password { get; set; } = "";
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        [MaxLength(6)]
        public string? PostalCode { get; set; }
        [MaxLength(50)]
        public string? PostalName { get; set; }
        [MaxLength(100)]
        public string? StreetName { get; set; }
        [MaxLength(10)]
        public string? StreetNumber { get; set; }
        public bool IsAdmin { get; set; }
        [ManyToMany(typeof(UserAdvertisement))]
        public List<Advertisement>? Advertisements { get; set; }    

        public User(string? firstName, string? lastName, DateTime birthDate, string email, string password, string? phone, string? city, string? country, string? postalCode, string? postalName, string? streetName, string? streetNumber, bool isAdmin)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            Password = password;
            Phone = phone;
            City = city;
            Country = country;
            PostalCode = postalCode;
            PostalName = postalName;
            StreetName = streetName;
            StreetNumber = streetNumber;
            IsAdmin = isAdmin;
        }
        public User()
        {

        }
    }
}
