

using Shop.Models;
using Shop.Services.Interfaces;
using Shop.Services.Db_Products;
using Shop.Services.SettingsManager;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Shop.Services.Auth;


namespace Shop.ViewModels
{
    internal class ProductListViewModel : BaseViewModel, INavigatedAware
    {

        public ProductListViewModel(INavigationService navigationService,
                                    ISettingsManager settingsManager,
                                    IUnfocusedEntry unfocusedEntry,
                                    IPrintMessage printMessage,
                                    IProducts products,
                                    IAuth auth)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _unfocusedEntry = unfocusedEntry;
            _printMessage = printMessage;
            _products = products;
            _auth = auth;

            IsVisibleIndicator = true;
        }


        #region public property


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }


        private bool _isCircleCart;
        public bool IsCircleCart
        {
            get => _isCircleCart;
            set => SetProperty(ref _isCircleCart, value);
        }


        private bool _isCircleFavorit;
        public bool IsCircleFavorit
        {
            get => _isCircleFavorit;
            set => SetProperty(ref _isCircleFavorit, value);
        }


        private ObservableCollection<ProductWithList> _productList;
        public ObservableCollection<ProductWithList> ProductList
        {
            get => _productList;
            set => SetProperty(ref _productList, value);
        }


        public DelegateCommand CartClick => new DelegateCommand(Cart_ClickAsync);
        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);
        public DelegateCommand<object> ChoiseItem => new DelegateCommand<object>(ChoiseItem_ClickAsync);
        public DelegateCommand<object> AddFavoritBtn => new DelegateCommand<object>(AddFavorit_Click);
        public DelegateCommand FilterClick => new DelegateCommand(Filter_ClickAsync);
        public DelegateCommand ButtonSearch => new DelegateCommand(ButtonSearch_Click);
        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);


        #endregion


        private void ButtonSearch_Click()
        {
            if (SearchText != null && SearchText.Length > 0)
            {
                Unfocused_Entry();
                GetProductsAsync(SearchText);
            }
        }

        private void Search()
        {
            _ = Task.Run(() =>
            {
                GetProductsAsync(SearchText);
            });

            if (SearchText.Length == 0)
            {
                Unfocused_Entry();
                _ = Task.Run(() =>
                {
                    GetProductsAsync();
                });
            }
        }

        private async void Filter_ClickAsync()
        {
            Unfocused_Entry();
            await _navigationService.NavigateAsync("FilterModal?useModalNavigation=true");
        }

        private void Unfocused_Entry()
        {
            _unfocusedEntry.HideKeyboard();
        }

        private async void ChoiseItem_ClickAsync(object obj)
        {
            if (!_isPressed)
            {
                _isPressed = true;

                var item = obj as ProductWithList;

                NavigationParameters param = new NavigationParameters
                {
                   {"item", item},
                };
                await _navigationService.NavigateAsync("Detail_Item", parameters: (param));
            }
        }

        private async void AddFavorit_Click(object obj)
        {
            var a = obj as ProductWithList;

            if (!_favoriteList.Any(s => s == a.Id))
            {
                _favoriteList.Add(a.Id);

                _settingsManager.IsCircleFavorit = true;
                IsCircleFavorit = true;

                var s = _login.FavoriteList.Trim();
                _login.FavoriteList = s + " " + a.Id.ToString();

                if (!await _auth.UpdateAsync(_login))
                {
                    _printMessage.ViewMessage(Resources.Strings.Resource.SaveError);
                }
                else
                {
                    _printMessage.ViewMessage(Resources.Strings.Resource.SaveToFavorite, "#228B22");
                }
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.SaveToFavorite, "#228B22");
            }
        }

        private async void Cart_ClickAsync()
        {
            if (!_isPressed)
            {
                _isPressed = true;
                await _navigationService.NavigateAsync("CartPage");
            }
        }

        private async void Go_BackAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private void GetProductsAsync(string search = null)
        {

            ProductList = _staticList;

            if (search != null)
            {
                ProductList = new ObservableCollection<ProductWithList>
                            (ProductList.AsParallel().Where(s => s.Name.ToLower().Contains(search.ToLower())));
            }

            IsVisibleIndicator = false;
        }

        private void GetProductsAsync(List<string> sizes, List<string> genders, List<string> brands, int min, int max)
        {

            ProductList = _staticList;

            if (brands != null && brands.Count > 0)
            {
                ProductList = new ObservableCollection<ProductWithList>
                                  (ProductList.AsParallel().Where(s => brands.Any(ss => s.Brand == ss)));
            }
            if (genders != null && genders.Count > 0)
            {
                ProductList = new ObservableCollection<ProductWithList>
                                  (ProductList.AsParallel().Where(s => genders.Any(ss => s.Gender == ss)));
            }
            if (sizes != null && sizes.Count > 0)
            {
                ProductList = new ObservableCollection<ProductWithList>
                                  (ProductList.AsParallel().Where(s => sizes.Any(ss => s.Sizes.Any(a => a == ss))));
            }
            if (max > 50)
            {
                ProductList = new ObservableCollection<ProductWithList>
                                  (ProductList.AsParallel().Where(s => s.Price >= min && s.Price <= max));
            }

            IsVisibleIndicator = false;
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "SearchText":
                    Search();
                    break;
                default:
                    break;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _isPressed = false;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            IsCircleCart = _settingsManager.IsCircleCart;

            if (parameters.ContainsKey("sizes"))
            {
                var sizes = parameters.GetValue<List<string>>("sizes");
                var genders = parameters.GetValue<List<string>>("gender");
                var brands = parameters.GetValue<List<string>>("brands");
                int minPrice = parameters.GetValue<int>("minPrice");
                int MaxPrice = parameters.GetValue<int>("maxPrice");

                GetProductsAsync(sizes, genders, brands, minPrice, MaxPrice);

            }
            else if (parameters.ContainsKey("seeall") || parameters.ContainsKey("search"))
            {
                string search = null;

                if (parameters.ContainsKey("search"))
                {
                    search = parameters.GetValue<string>("search");
                }
                GetProductsAsync(search);

            }
        }

    }
}
