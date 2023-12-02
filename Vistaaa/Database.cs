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
        }
        public async Task<int> CreateAdvertisementAsync(Advertisement advertisement)
        {
            await Init();
            return await DatabaseHandler.InsertAsync(advertisement);
        }
        public async Task<List<Advertisement>> GetAdvertisementsAsync()
        {
            await Init();
            return await DatabaseHandler.Table<Advertisement>().ToListAsync();
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
    }
}
