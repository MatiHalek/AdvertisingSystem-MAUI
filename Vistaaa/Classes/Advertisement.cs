using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Classes
{
    public class Advertisement
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }
        public string Title { get; set; }
        public string PositionName { get; set; }
        public string PositionLevel { get; set; }
        public string ContractType { get; set; }
        public string Employment { get; set; }
        public string WorkType { get; set; }
        public decimal? LowestSalary { get; set; }
        public decimal HighestSalary { get; set; }
        public string WorkDays { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public string Offer { get; set; }

        public Advertisement(string title, string positionName, string positionLevel, string contractType, string employment, string workType, decimal? lowestSalary, decimal highestSalary, string workDays, DateTime creationDate, DateTime expirationDate, string responsibilities, string requirements, string offer)
        {
            Title = title;
            PositionName = positionName;
            PositionLevel = positionLevel;
            ContractType = contractType;
            Employment = employment;
            WorkType = workType;
            LowestSalary = lowestSalary;
            HighestSalary = highestSalary;
            WorkDays = workDays;
            CreationDate = creationDate;
            ExpirationDate = expirationDate;
            Responsibilities = responsibilities;
            Requirements = requirements;
            Offer = offer;
        }

        public Advertisement()
        {

        }
    }
}
