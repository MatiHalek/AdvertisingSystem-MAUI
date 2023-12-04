using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistaaa.Classes;

namespace Vistaaa
{
    public class Database
    {
        SQLiteAsyncConnection? DatabaseHandler;

        public Database()
        {
        }
        async Task Init()
        {
            if (DatabaseHandler is not null)
                return;
            DatabaseHandler = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await DatabaseHandler.CreateTableAsync<Advertisement>();
            await DatabaseHandler.CreateTableAsync<Company>();
        }
        public async Task<int> CreateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler.InsertAsync(advertisement);
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(string? search = null)
        {
            await Init();
            if(search != null)
                return await DatabaseHandler.Table<Advertisement>().Where(advertisement => advertisement.Title.ToLower().Contains(search.ToLower())).ToListAsync();
            return await DatabaseHandler.Table<Advertisement>().ToListAsync();
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(uint pageNumber, uint advertisementsOnPage, string? search = null)
        {
            await Init();
            var advertisements = GetAdvertisementsAsync(search);
            if (search != null)
                return await DatabaseHandler.Table<Advertisement>().Where(advertisement => advertisement.Title.ToLower().Contains(search.ToLower())).Skip((int)((pageNumber - 1) * advertisementsOnPage)).Take((int)advertisementsOnPage).ToListAsync();
            return await DatabaseHandler.Table<Advertisement>().Skip((int)((pageNumber - 1) * advertisementsOnPage)).Take((int)advertisementsOnPage).ToListAsync();
        }
        public async Task<int> DeleteAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler.DeleteAsync(advertisement);
        }
        public async Task<int> UpdateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler.UpdateAsync(advertisement);
        }
        public async Task<int> CreateCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler.InsertAsync(company);
        }
        public async Task<List<Company>> GetCompaniesAsync()
        {
            await Init();
            return await DatabaseHandler.Table<Company>().ToListAsync();
        }
        public async Task<int> DeleteCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler.DeleteAsync(company);
        }
        public async Task<int> UpdateCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler.UpdateAsync(company);
        }
    }
}
