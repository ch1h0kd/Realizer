using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Data
{
    public class DatabaseContext : IAsyncDisposable
    {
        private const string DbName = "MyDatabase.db";
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);
        ///data/user/0/com.companyname.realizer/files/MyDatabase.db

        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection Database => //Try to get Database
            (_connection ??= new SQLiteAsyncConnection(DbPath, //have connection? If not, build it
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite));//create, read, write database

        private async Task CreateTableIfNotExist<TTable>() where TTable : class, new()
            //<TTable> = type parameter, genetic type. 
        {
            await Database.CreateTableAsync<TTable>();//create a table if it does not exist
        }

        private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
        {
            await CreateTableIfNotExist<TTable>();
            return await action();
        }

        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()//get a talbe
        {
            await CreateTableIfNotExist<TTable>();
            return Database.Table<TTable>();
        }

        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            Console.WriteLine("path = ", DbPath);
            var table = await GetTableAsync<TTable>();//get a table here
            return await table.ToListAsync();
        }

        //returns items that are true to the given condition
        //ex. _context.GetFilteredAsync<Client>(x => x.client_id == id)
        public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();//get a table here
            return await table.Where(predicate).ToListAsync();//get filtered items
        }

        public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
            //<bool> = return type
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
            //var path2 = Android.OS.Environment.ExternalStorageDirectory; //Public external files
            return await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);
        }

        public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.UpdateAsync(item) > 0);
        }

        //deletes an item by ID
        public async Task<bool> DeteleItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.DeleteAsync<TTable>(primaryKey) > 0);
        }

        //returns boolean, whether the table contains an object(row) with the given primary key
        public async Task<bool> ContainsItemByIdAsync<TTable>(TTable primaryKey) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.FindAsync<TTable>(primaryKey) != null);
        }

        //return an object(row) with the given primary key, if not, returns null
        public async Task<TTable> GetByIdAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            return await Execute<TTable, TTable>(async () => await Database.FindAsync<TTable>(primaryKey));
        }

        public async ValueTask DisposeAsync() => await _connection?.CloseAsync();//when out of memory

    }
}