

using Shop.Models;


namespace Shop.Services.Db_Cart
{
    internal interface ICart
    {
        Task<List<Cart>> GetListAsync();
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateAsync(Cart item);
        Task<bool> InsertAsync(Cart item);
        Task<List<Cart>> FoundProductsAsync(string[] query);
    }
}
