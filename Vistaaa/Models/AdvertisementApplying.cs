using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class AdvertisementApplying
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }
        [ForeignKey(typeof(User))]
        public uint UserId { get; set; }
        [ForeignKey(typeof(Advertisement))]
        public uint AdvertisementId { get; set; }
        public AdvertisementApplying(uint userId, uint advertisementId)
        {
            UserId = userId;
            AdvertisementId = advertisementId;
        }
        public AdvertisementApplying()
        {

        }
    }
}
