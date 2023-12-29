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
            await DatabaseHandler.CreateTableAsync<UserAdvertisement>();
            await DatabaseHandler.CreateTableAsync<WorkType>();
            await DatabaseHandler.CreateTableAsync<EmploymentType>();
            await DatabaseHandler.CreateTableAsync<ContractType>();
            await DatabaseHandler!.Table<WorkType>().CountAsync().ContinueWith(task =>
            {
                if (task.Result == 0)
                {
                    DatabaseHandler.InsertAllAsync(
                        new List<WorkType>
                        {
                            new("Stacjonarna"),
                            new("Hybrydowa"),
                            new("Zdalna"),
                        }
                    );
                }
            });
            await DatabaseHandler!.Table<EmploymentType>().CountAsync().ContinueWith(task =>
            {
                if (task.Result == 0)
                {
                    DatabaseHandler.InsertAllAsync(
                        new List<EmploymentType>
                        {
                                new("Pełny etat"),
                                new("Pół etatu"),
                                new("1/4 etatu"),
                                new("3/4 etatu"),
                                new("Staż"),
                                new("Praktyki"),
                                new("Wolontariat"),
                        }
                    );
                }
            });
            await DatabaseHandler!.Table<ContractType>().CountAsync().ContinueWith(task =>
            {
                if (task.Result == 0)
                {
                    DatabaseHandler.InsertAllAsync(
                        new List<ContractType>
                        {
                            new("Umowa o pracę"),
                            new("Umowa zlecenie"),
                            new("Umowa o dzieło"),
                        }
                    );
                }
            });
            await DatabaseHandler!.Table<User>().CountAsync().ContinueWith(task =>
            {
                if (task.Result == 0)
                {
                    Preferences.Set("userId", null);
                }
            });           
        }
        public async Task<int> CreateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(advertisement);
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(string? search = null, SortBy sortBy = SortBy.Date, bool savedOnly = false)
        {
            await Init();
            DateTime currentTime = DateTime.Now;
            List<Advertisement> allAdvertisements = await DatabaseHandler!.Table<Advertisement>().ToListAsync();
            if (savedOnly)
            {
                uint userId = uint.Parse(Preferences.Get("userId", null) ?? "");
                List <UserAdvertisement> userAdvertisements = await DatabaseHandler!.Table<UserAdvertisement>().Where(advertisement => advertisement.UserId == userId).ToListAsync();
                allAdvertisements =
                [
                    .. allAdvertisements.Where(advertisement => userAdvertisements.Any(userAdvertisement => userAdvertisement.AdvertisementId == advertisement.Id)),
                ];
            }
            if (search != null)
            {
                allAdvertisements =
                [
                    .. allAdvertisements.Where(advertisement => advertisement.Title.ToLower().Contains(search.ToLower())),
                ];
            }
            List<Advertisement> advertisements = [];
            if(sortBy == SortBy.Salary)
            {
                advertisements =
                [
                    .. allAdvertisements.Where(advertisement => advertisement.ExpirationDate >= currentTime).OrderByDescending(advertisement => advertisement.HighestSalary),
                ];
                advertisements.AddRange(allAdvertisements.Where(advertisement => advertisement.ExpirationDate < currentTime).OrderByDescending(advertisement => advertisement.HighestSalary));
                return advertisements;
            }
            advertisements =
            [
                .. allAdvertisements.Where(advertisement => advertisement.ExpirationDate >= currentTime).OrderByDescending(advertisement => advertisement.CreationDate),
            ];
            advertisements.AddRange(allAdvertisements.Where(advertisement => advertisement.ExpirationDate < currentTime).OrderByDescending(advertisement => advertisement.CreationDate));   
            return advertisements;
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync(uint pageNumber, uint advertisementsOnPage, string? search = null, SortBy sortBy = SortBy.Date, bool savedOnly = false)
        {
            var advertisements = await GetAdvertisementsAsync(search, sortBy, savedOnly);
            return advertisements.Skip((int)((pageNumber - 1) * advertisementsOnPage)).Take((int)advertisementsOnPage).ToList();
        }
        public async Task<List<Category>> GetCategories()
        {
            await Init();
            return await DatabaseHandler!.Table<Category>().ToListAsync();
        }
        public async Task<int> CreateCategory(Category category)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(category);
        }
        public async Task<int> DeleteAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.DeleteAsync(advertisement);
        }
        public async Task<int> UpdateAdvertisement(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler!.UpdateAsync(advertisement);
        }
        public async Task<int> CreateCompanyAsync(Company company)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(company);
        }
        public async Task<List<Company>> GetCompanies()
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
        public async Task<uint> CreateUser(User user)
        {
            await Init();
            await DatabaseHandler!.InsertAsync(user);
            return user.Id;
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
        public async Task<int> CreateUserAdvertisementAsync(UserAdvertisement userAdvertisement)
        {
            await Init();
            return await DatabaseHandler!.InsertAsync(userAdvertisement);
        }
        public async Task<UserAdvertisement?> CheckIfUserAdvertisementExists(uint userId, uint advertisementId)
        {
            await Init();
            UserAdvertisement userAdvertisement = await DatabaseHandler!.Table<UserAdvertisement>().Where(item => item.UserId == userId && item.AdvertisementId == advertisementId).FirstOrDefaultAsync();
           return userAdvertisement;
        }
        public async Task<int> DeleteUserAdvertisementAsync(UserAdvertisement userAdvertisement)
        {
            await Init();
            return await DatabaseHandler!.DeleteAsync(userAdvertisement);
        }
        public async Task<List<ContractType>> GetContractTypes()
        {
            await Init();
            return await DatabaseHandler!.Table<ContractType>().ToListAsync();
        }
        public async Task<List<EmploymentType>> GetEmploymentTypes()
        {
            await Init();
            return await DatabaseHandler!.Table<EmploymentType>().ToListAsync();
        }
        public async Task<List<WorkType>> GetWorkTypes()
        {
            await Init();
            return await DatabaseHandler!.Table<WorkType>().ToListAsync();
        }
        public async ValueTask DisposeAsync()
        {
            if (DatabaseHandler is not null)
                await DatabaseHandler.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
