

namespace Shop.Services.Repository
{
    public interface IRepository
    {
        Task<List<T>> GetDataAsync<T>(string table, string from, string val) where T : class, new();
        Task<List<T>> GetDataAsync<T>(string table, int id) where T : class, new();
        Task<List<T>> GetDataAsync<T>(string table) where T : class, new();
        Task<bool> UpdateAsync<T>(T profile) where T : class, new();
        Task<bool> InsertAsync<T>(T profile) where T : class, new();
        Task<int> InsertAllAsync<T>(List<T> list) where T : class, new();
        Task<int> DeleteAsync<T>(int id) where T : class, new();
        Task<int> ClearTableAsync<T>() where T : class, new();
    }

}
