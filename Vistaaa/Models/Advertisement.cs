using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Models
{
    public class Advertisement
    {
        [PrimaryKey, AutoIncrement]
        public uint Id { get; set; }
        public string Title { get; set; } = "";
        [ForeignKey(typeof(Company))]
        public uint CompanyId { get; set; }
        [ForeignKey(typeof(Category))]
        public uint CategoryId { get; set; }
        public string PositionName { get; set; } = "";
        public string PositionLevel { get; set; } = "";
        public uint ContractType { get; set; }
        public uint EmploymentType { get; set; }
        public uint WorkType { get; set; }
        public decimal? LowestSalary { get; set; }
        public decimal HighestSalary { get; set; }
        public string WorkDays { get; set; } = "";
        [Ignore]
        public bool IsUpToDate
        {
            get
            {
                return ExpirationDate >= DateTime.Now;
            }
        }
        [Ignore]
        public string CompanyName
        {
            get
            {
                Database database = new();
                var companies = Task.Run(database.GetCompanies).Result;
                foreach (var company in companies)
                {
                    if (company.Id == CompanyId)
                    {
                        return company.Name;
                    }
                }
                return "Unknown company";
            }
        }
        [Ignore]
        public string CategoryName
        {
            get
            {
                Database database = new();
                var categories = Task.Run(database.GetCategories).Result;
                foreach (var category in categories)
                {
                    if (category.Id == CategoryId)
                    {
                        return category.Name;
                    }
                }
                return "Unknown category";
            }
        }
        [Ignore]
        public string ContractTypeName
        {
            get
            {
                Database database = new();
                var contractTypes = Task.Run(database.GetContractTypes).Result;
                foreach (var contractType in contractTypes)
                {
                    if (contractType.ContractTypeId == ContractType)
                    {
                        return contractType.Name;
                    }
                }
                return "Unknown contract type";
            }
        }
        [Ignore]
        public string EmploymentTypeName
        {
            get
            {
                Database database = new();
                var employmentTypes = Task.Run(database.GetEmploymentTypes).Result;
                foreach (var employmentType in employmentTypes)
                {
                    if (employmentType.EmploymentTypeId == EmploymentType)
                    {
                        return employmentType.Name;
                    }
                }
                return "Unknown employment type";
            }
        }
        [Ignore]
        public string WorkTypeName
        {
            get
            {
                Database database = new();
                var workTypes = Task.Run(database.GetWorkTypes).Result;
                foreach (var workType in workTypes)
                {
                    if (workType.WorkTypeId == WorkType)
                    {
                        return workType.Name;
                    }
                }
                return "Unknown work type";
            }
        }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Responsibilities { get; set; } = "";
        public string Requirements { get; set; } = "";
        public string Offer { get; set; } = "";
        [ManyToMany(typeof(UserAdvertisement))]
        public List<User>? Users { get; set; }

        public Advertisement(string title, uint companyId, uint categoryId, string positionName, string positionLevel, uint contractType, uint employmentType, uint workType, decimal? lowestSalary, decimal highestSalary, string workDays, DateTime creationDate, DateTime expirationDate, string responsibilities, string requirements, string offer)
        {
            Title = title;
            CompanyId = companyId;
            CategoryId = categoryId;
            PositionName = positionName;
            PositionLevel = positionLevel;
            ContractType = contractType;
            EmploymentType = employmentType;
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
