using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class UserAdvertisement
    {
        [PrimaryKey, AutoIncrement]
        public uint UserAdvertisementId { get; set; }
        [ForeignKey(typeof(User))]
        public uint UserId { get; set; }
        [ForeignKey(typeof(Advertisement))] 
        public uint AdvertisementId { get; set; }
        public UserAdvertisement(uint userId, uint advertisementId)
        {
            UserId = userId;
            AdvertisementId = advertisementId;
        }
        public UserAdvertisement()
        {

        }
    }
}
