

using Shop.Models;
using Shop.Services.HttpService;
using Shop.Services.Repository;
using Shop.Services.SettingsManager;


namespace Shop.Services.Db_Products
{
    internal class Products : IProducts
    {

        private readonly IRepository _repository;
        private readonly IRestService _restService;
        private readonly ISettingsManager _settingsManager;


        public Products(IRepository repository,
                        IRestService restService,
                        ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _restService = restService;
            _repository = repository;
        }


        //public Task<List<Product>> FoundProductsAsync(string[] query)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<Product>> GetProductsAsync()
        {

              var _productList = await _restService.GetDataAsync<Product>();
                //_productList.Sort(delegate (Product x, Product y) { return x.Name.CompareTo(y.Name); });

                //if (_productList == null || _productList.Count == 0)
                //{
                //    _productList = await _repository.GetDataAsync<Product>("Product");
                //}
                
                //    _ = Task.Run(async () =>
                //    {
                //        await UpdateLocal_Db_Async();
                //    });

            return _productList;
        }

        //public Product GetProducts_OnId_Async(int id)
        //{
        //    return _productList.Find(s=>s.Id == id);
        //}


        //private async Task UpdateLocal_Db_Async()
        //{

        //    DateTime dt1 = DateTime.Now.AddMinutes(-Constants.Constant.UpdateLocal_Db_Hourse);
        //    var dt2 = _settingsManager.LastUpdate_DB;

        //    var list = await _repository.GetDataAsync<Product>("Product");

        //    if (dt1 >= dt2 || list == null || list.Count == 0)
        //    {

        //        _ = await _repository.ClearTableAsync<Product>();

        //        var res = await _repository.InsertAllAsync(_productList);

        //        if (res != _productList.Count)
        //        {
        //            Console.WriteLine("Error DB update");
        //        }
        //        else
        //        {
        //            _settingsManager.LastUpdate_DB = DateTime.Now;
        //        }
        //    }
        //}

    }
}
