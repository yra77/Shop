

using Shop.Models;
using Shop.Services.HttpService;


namespace Shop.Services.Db_Products
{
    internal class Products : IProducts
    {


        private readonly IRestService _restService;


        public Products(IRestService restService)
        {
            _restService = restService;
        }


        //public Task<List<Product>> FoundProductsAsync(string[] query)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<Product>> GetProductsAsync()
        {
           return await _restService.GetDataAsync<Product>();
        }

        //public Product GetProducts_OnId_Async(int id)
        //{
        //    return _productList.Find(s=>s.Id == id);
        //}

    }
}
