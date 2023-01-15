

using Shop.Models;
using Shop.Services.HttpService;
//using Shop.Services.Repository;


namespace Shop.Services.Db_Cart
{
    internal class Carts : ICart
    {

        //private readonly IRepository _repository;
        private readonly IRestService _restService;


        public Carts(//IRepository repository,
                     IRestService restService)
        {
           // _repository = repository;
            _restService = restService;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {         
            return await _restService.DeleteAsync<Cart>(id);//await _repository.DeleteAsync<Cart>(id);
        }

        public Task<List<Cart>> FoundProductsAsync(string[] query)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cart>> GetListAsync()
        {
            var res = await _restService.GetDataAsync<Cart>();
          //  var resLocal = await _repository.GetDataAsync<Cart>("Cart");

            //if (res != null && res.Count > 0)
            //{
            //    if (resLocal == null || res.Count > 0 || res.Count > resLocal.Count)
            //    {
            //        await _repository.ClearTableAsync<Cart>();
            //        await _repository.InsertAllAsync<Cart>(res);
            //    }

            //    if (res.Count < resLocal.Count)
            //    {
            //        await _restService.DeleteAsync<Cart>(0);

            //        foreach (var item in resLocal)
            //        {
            //            await _restService.InsertAsync<Cart>(item);
            //        }
            //    }
            //    return res;
            //}

            return res;
        }

        public async Task<bool> InsertAsync(Cart item)
        {
               // return (await _repository.InsertAsync(item) &&
                        return await _restService.InsertAsync<Cart>(item);
        }

        public async Task<bool> UpdateAsync(Cart item)
        {
           // return (await _repository.UpdateAsync(item) &&
              return await _restService.UpdateDataAsync<Cart>(item.Id, item);
        }
    }
}
