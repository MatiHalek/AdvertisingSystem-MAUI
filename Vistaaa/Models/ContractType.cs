using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class ContractType
    {
        [PrimaryKey, AutoIncrement]
        public uint ContractTypeId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public ContractType()
        {
        }
        public ContractType(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
