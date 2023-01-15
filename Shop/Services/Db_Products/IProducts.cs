

using Shop.Models;


namespace Shop.Services.Db_Products
{
    public interface IProducts
    {
        Task<List<Product>> GetProductsAsync();
    }
}
