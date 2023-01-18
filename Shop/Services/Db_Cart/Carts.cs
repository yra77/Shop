

using Shop.Models;
using Shop.Services.HttpService;


namespace Shop.Services.Db_Cart
{
    internal class Carts : ICart
    {


        private readonly IRestService _restService;


        public Carts(IRestService restService)
        {
            _restService = restService;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {         
            return await _restService.DeleteAsync<Cart>(id);
        }

        public Task<List<Cart>> FoundProductsAsync(string[] query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cart>> GetListAsync()
        {
            return await _restService.GetDataAsync<Cart>();
        }

        public async Task<bool> InsertAsync(Cart item)
        {
            return await _restService.InsertAsync<Cart>(item);
        }

        public async Task<bool> UpdateAsync(Cart item)
        {
            return await _restService.UpdateDataAsync<Cart>(item.Id, item);
        }
    }
}
