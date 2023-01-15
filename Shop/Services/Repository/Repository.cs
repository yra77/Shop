

using Shop.Models;
using SQLite;


namespace Shop.Services.Repository
{

    class Repository : IRepository
    {

        private readonly SQLiteAsyncConnection _connection;


        public Repository()
        {
            var path = FileSystem.AppDataDirectory;
            path = Path.Combine(path, "database.db3");
            SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadWrite |
                                           SQLite.SQLiteOpenFlags.Create |
                                           SQLite.SQLiteOpenFlags.SharedCache;

            _connection = new SQLiteAsyncConnection(path, flags);

            CreateTable<Login>();
            CreateTable<Product>();
           // CreateTable<Cart>();
        }

        public async Task<int> DeleteAsync<T>(int id) where T : class, new()
        {
            return await _connection.DeleteAsync<T>(id);
        }

        public async Task<List<T>> GetDataAsync<T>(string table, string from, string val) where T : class, new()
        {
            return await _connection.QueryAsync<T>($"SELECT * FROM '" + table + "' WHERE " + from +" ='" + val + "'");
        }

        public async Task<List<T>> GetDataAsync<T>(string table, int id) where T : class, new()
        {
            return await _connection.QueryAsync<T>($"SELECT * FROM '" + table + "' WHERE Id ='" + id + "'");
        }

        public async Task<List<T>> GetDataAsync<T>(string table) where T : class, new()
        {
            return await _connection.QueryAsync<T>("SELECT * FROM '" + table + "'");
        }

        public async Task<bool> InsertAsync<T>(T profile) where T : class, new()
        {
            int u = await _connection.InsertAsync(profile);

            return (u > 0) ? true : false;
        }

        public async Task<bool> UpdateAsync<T>(T profile) where T : class, new()
        {
            int u = await _connection.UpdateAsync(profile);

            return (u > 0) ? true : false;
        }

        public async Task<int> InsertAllAsync<T>(List<T> list) where T : class, new()
        {
            return await _connection.InsertAllAsync(list);
        }

        public async Task<int> ClearTableAsync<T>() where T : class, new()
        {
            return await _connection.DeleteAllAsync<T>();
        }

        public void CreateTable<T>() where T : class, new()
        {
            try
            {
                _connection.CreateTableAsync<T>().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error create table " + e.Message);
            }
        }

    }
}
