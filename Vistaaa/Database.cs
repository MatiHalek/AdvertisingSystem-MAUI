using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistaaa.Classes;
using Vistaaa.Models;

namespace Vistaaa
{
    public class Database : IAsyncDisposable
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
            await DatabaseHandler.CreateTableAsync<Category>();
            await DatabaseHandler.CreateTableAsync<User>();
        }
        public async Task<int> CreateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(advertisement);
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(string? search = null)
        {
            await Init();
            if(search != null)
                return await DatabaseHandler!.Table<Advertisement>().Where(advertisement => advertisement.Title.ToLower().Contains(search.ToLower())).OrderByDescending(advertisement => advertisement.CreationDate).ToListAsync();
            return await DatabaseHandler!.Table<Advertisement>().ToListAsync();
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(uint pageNumber, uint advertisementsOnPage, string? search = null)
        {
            await Init();
            var advertisements = GetAdvertisementsAsync(search);
            if (search != null)
                return await DatabaseHandler!.Table<Advertisement>().Where(advertisement => advertisement.Title.ToLower().Contains(search.ToLower())).Skip((int)((pageNumber - 1) * advertisementsOnPage)).Take((int)advertisementsOnPage).ToListAsync();
            return await DatabaseHandler!.Table<Advertisement>().Skip((int)((pageNumber - 1) * advertisementsOnPage)).Take((int)advertisementsOnPage).ToListAsync();
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            await Init();
            return await DatabaseHandler!.Table<Category>().ToListAsync();
        }
        public async Task<int> DeleteAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.DeleteAsync(advertisement);
        }
        public async Task<int> UpdateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.UpdateAsync(advertisement);
        }
        public async Task<int> CreateCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(company);
        }
        public async Task<List<Company>> GetCompaniesAsync()
        {
            await Init();
            return await DatabaseHandler!.Table<Company>().ToListAsync();
        }
        public async Task<int> DeleteCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler!.DeleteAsync(company);
        }
        public async Task<int> UpdateCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler!.UpdateAsync(company);
        }
        public async Task<User> GetUserAsync(uint id)
        {
            await Init();
            return await DatabaseHandler!.Table<User>().Where(user => user.Id == id).FirstOrDefaultAsync();
        }
        public async Task<int> CreateUserAsync(User user)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(user);
        }   
        public async Task<User?> VerifyUserAsync(string email, string password)
        {
            await Init();
            User? user = await DatabaseHandler!.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
            if(user is not null)
            {
                if (PasswordHasher.Verify(password, user.Password))
                    return user;
            }
            return null;
        }
        public async ValueTask DisposeAsync()
        {
            if (DatabaseHandler is not null)
                await DatabaseHandler.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
