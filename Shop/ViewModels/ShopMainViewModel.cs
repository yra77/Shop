

using Shop.Helpers;
using Shop.Models;
using Shop.Services.Auth;
using Shop.Services.Db_Products;
using Shop.Services.Interfaces;
using Shop.Services.SettingsManager;
using System.Collections.ObjectModel;


namespace Shop.ViewModels
{
    internal class ShopMainViewModel : BaseViewModel, INavigatedAware
    {

        private bool _isMenuOpen;
        private bool _isOpen;


        public ShopMainViewModel(INavigationService navigationService,
                                 ICheck_AndroidServives checkAndroid,
                                 ISettingsManager settingsManager,
                                 IUnfocusedEntry unfocusedEntry,
                                 IChangeThemeService changeTheme,
                                 IPrintMessage printMessage,
                                 IProducts products,
                                 IAuth auth)
        {

            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _unfocusedEntry = unfocusedEntry;
            _printMessage = printMessage;
            _checkAndroid = checkAndroid;
            _changeTheme = changeTheme;
            _products = products;
            _auth = auth;

            TraslateX = 0;
            ScaleBaseContent = 1;
            RotateBaseContent = 0;
            _isMenuOpen = false;
            _isOpen = false;

            Name = _settingsManager.Name;
        }


        #region Public property


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


        private double _traslateX;
        public double TraslateX
        {
            get => _traslateX;
            set => SetProperty(ref _traslateX, value);
        }


        private double _scaleBaseContent;
        public double ScaleBaseContent
        {
            get => _scaleBaseContent;
            set => SetProperty(ref _scaleBaseContent, value);
        }


        private double _rotateBaseContent;
        public double RotateBaseContent
        {
            get => _rotateBaseContent;
            set => SetProperty(ref _rotateBaseContent, value);
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }


        private byte[] _photoPath;
        public byte[] PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value);
        }


        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        private ObservableCollection<ProductWithList> _productList;
        public ObservableCollection<ProductWithList> ProductList
        {
            get => _productList;
            set => SetProperty(ref _productList, value);
        }


        private ObservableCollection<ProductWithList> _productListNew;
        public ObservableCollection<ProductWithList> ProductListNew
        {
            get => _productListNew;
            set => SetProperty(ref _productListNew, value);
        }


        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);
        public DelegateCommand SeeAllCommand => new DelegateCommand(Go_SeeAllAsync);
        public DelegateCommand HamburgerClick => new DelegateCommand(Hamburger_Click);
        public DelegateCommand CartClick => new DelegateCommand(Cart_ClickAsync);
        public DelegateCommand FilterClick => new DelegateCommand(Filter_ClickAsync);
        public DelegateCommand<object> ChoiseItem => new DelegateCommand<object>(ChoiseItem_ClickAsync);
        public DelegateCommand<object> AddFavoritBtn => new DelegateCommand<object>(AddFavorit_Click);
        public DelegateCommand ButtonSearch => new DelegateCommand(ButtonSearch_ClickAsync);
        public DelegateCommand FavoritClick => new DelegateCommand(Favorit_ClickAsync);
        public DelegateCommand Account => new DelegateCommand(AccountClick);


        #endregion


        private async void Favorit_ClickAsync()
        {
            await _navigationService.NavigateAsync("FavoritePage");
        }

        private async void AccountClick()
        {
            _isMenuOpen = false;
            TraslateX = 0;
            ScaleBaseContent = 1;
            RotateBaseContent = 0;
            _changeTheme.SetTheme(Enums.ThemeEnum.Light);
            await _navigationService.NavigateAsync("Account");
        }

        private async void AddFavorit_Click(object obj)
        {
            var a = obj as ProductWithList;

            if (_stateNethwork)
            {
                if (!_favoriteList.Any(s => s == a.Id))
                {

                    _favoriteList.Add(a.Id);
                    _settingsManager.IsCircleFavorit = true;
                    IsCircleFavorit = true;

                    var s = _login.FavoriteList.Trim();

                    if (s.Length > 0)
                    {
                        _login.FavoriteList = s + " " + a.Id.ToString();
                    }
                    else
                    {
                        _login.FavoriteList = a.Id.ToString();
                    }

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
                {//если уже есть в списке просто сообщаем Ок
                    _printMessage.ViewMessage(Resources.Strings.Resource.SaveToFavorite, "#228B22");
                }
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.NotServer);
            }
        }

        private async void ChoiseItem_ClickAsync(object obj)
        {
            var item = obj as ProductWithList;

            NavigationParameters param = new NavigationParameters
            {
                {"item", item},
            };
            await _navigationService.NavigateAsync("Detail_Item", parameters: (param));
        }

        private async void Hamburger_Click()
        {
            Unfocused_Entry();

            if (!_isMenuOpen)
            {
                _changeTheme.SetTheme(Enums.ThemeEnum.Dark);
                AnimationAsync(0, 200, 1);
                _isMenuOpen = true;
            }
            else
            {
                AnimationAsync(199, -1, 1);
                _isMenuOpen = false;
                await Task.Delay(300);
                _changeTheme.SetTheme(Enums.ThemeEnum.Light);
            }
        }

        private async void ButtonSearch_ClickAsync()
        {
            if (SearchText != null && SearchText.Length > 0)
            {
                NavigationParameters param = new NavigationParameters
                {
                {"search", SearchText},
                };
                await _navigationService.NavigateAsync("ProductList", parameters: (param));
            }
        }

        private async void Cart_ClickAsync()
        {
            Unfocused_Entry();
            await _navigationService.NavigateAsync("CartPage");
        }

        private async void Go_SeeAllAsync()
        {
            NavigationParameters param = new NavigationParameters
                {
                    {"seeall", ""}
                };
            await _navigationService.NavigateAsync("ProductList", parameters: param);
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

        private async void AnimationAsync(int start, int end, int duration)
        {
            if (start < end)
            {
                for (int i = start; i < end; i += 10)
                {
                    TraslateX = i;
                    ScaleBaseContent = 1.0 - (double)i / 1000;
                    RotateBaseContent = -(double)i / 20;
                    await Task.Delay(duration);
                }
            }
            else
            {
                for (int i = start; i >= end; i -= 10)
                {
                    TraslateX = i;
                    ScaleBaseContent = 1.0 - (double)i / 1000;
                    RotateBaseContent = -(double)i / 20;
                    await Task.Delay(duration);
                }
            }
        }

        private void GetProductsAsync()
        {
            IsVisibleIndicator = true;

                ProductList = new ObservableCollection<ProductWithList>(_staticList.AsParallel().Where(m => m.IsBestSeller == true));
                ProductListNew = new ObservableCollection<ProductWithList>(_staticList.AsParallel().Where(m => m.IsNew == true));
          
            IsVisibleIndicator = false;
        }

        private void CreateFavorite()
        {
            if (_login.FavoriteList != null && _login.FavoriteList.Length > 0)
            {
                _settingsManager.IsCircleFavorit = true;
                IsCircleFavorit = true;

                var a = _login.FavoriteList;

                List<string> arr = new List<string>();
                string buf = null;

                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] != ' ')
                        {
                            buf += a[i];
                        }
                        if ((a[i] == ' ' || a.Length - 1 == i) && buf != null)
                        {
                            arr.Add(buf);
                            buf = null;
                        }
                    }
              
                _favoriteList = new ObservableCollection<int>();

                foreach (var item in arr)
                {
                    if (item != null && item != "")
                    {
                        _favoriteList.Add(Int16.Parse(item));
                    }
                }
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _isMenuOpen = false;
            _changeTheme.SetTheme(Enums.ThemeEnum.Light);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

            if (!_isOpen)
            {
                _isOpen = true;
                GetProductsAsync();
            }

            _isMenuOpen = false;
            _changeTheme.SetTheme(Enums.ThemeEnum.Light);

            if (_login.ImageAccount != null)
            {
                PhotoPath = _login.ImageAccount;
            }

            IsCircleFavorit = _settingsManager.IsCircleFavorit;
            IsCircleCart = _settingsManager.IsCircleCart;

            if (parameters.ContainsKey("sizes"))
            {
                NavigationParameters param = new NavigationParameters
                {
                    {"sizes", parameters.GetValue<List<string>>("sizes")},
                    {"gender", parameters.GetValue<List<string>>("gender") },
                    {"brands", parameters.GetValue<List<string>>("brands") },
                    {"minPrice", parameters.GetValue<int>("minPrice") },
                    {"maxPrice", parameters.GetValue<int>("maxPrice") }
                };
                await _navigationService.NavigateAsync("ProductList", parameters: (param));
            }

            CreateFavorite();
        }
    }
}
