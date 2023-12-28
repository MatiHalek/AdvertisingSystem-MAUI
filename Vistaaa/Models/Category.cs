using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";

        public Category(uint id, string name)
        {
            Id = id;
            Name = name;
        }
        public Category()
        {

        }
    }
}
