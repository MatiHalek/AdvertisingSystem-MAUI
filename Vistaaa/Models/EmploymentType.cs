using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class EmploymentType
    {
        [PrimaryKey, AutoIncrement]
        public uint EmploymentTypeId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public EmploymentType()
        {
        }
        public EmploymentType(string name)
        {
            Name = name;
        }
    }
}
