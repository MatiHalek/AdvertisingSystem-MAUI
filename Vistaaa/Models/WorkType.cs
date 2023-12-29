using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class WorkType
    {
        [PrimaryKey, AutoIncrement]
        public uint WorkTypeId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public WorkType()
        {
        }
        public WorkType(string name)
        {
            Name = name;
        }
    }
}
