

using Shop.Models;
using Shop.Converters;
using Shop.Services.Auth;
using Shop.Services.Db_Cart;
using Shop.Services.Interfaces;
using Shop.Services.Db_Products;
using Shop.Services.SettingsManager;

using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class CartPageViewModel : BaseViewModel, INavigatedAware
    {

        public CartPageViewModel(INavigationService navigationService,
                                 ICheck_AndroidServives checkAndroid,
                                 ISettingsManager settingsManager,
                                 IPrintMessage printMessage,
                                 IProducts products,
                                 IAuth auth,
                                 ICart cart)
        {

            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _printMessage = printMessage;
            _checkAndroid = checkAndroid;
            _products = products;
            _carts = cart;
            _auth = auth;

            IsEnabled = false;
            Color_OkBtn = "LightGrey";
        }


        #region public Property


        private ObservableCollection<CartWithProduct> _buyList;
        public ObservableCollection<CartWithProduct> BuyList
        {
            get => _buyList;
            set => SetProperty(ref _buyList, value);
        }


        private string _color_OkBtn;
        public string Color_OkBtn
        {
            get => _color_OkBtn;
            set => SetProperty(ref _color_OkBtn, value);
        }


        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        private string _totalSum;
        public string TotalSum
        {
            get => _totalSum;
            set => SetProperty(ref _totalSum, value);
        }


        private int _isChange;
        public int IsChange
        {
            get => _isChange;
            set => SetProperty(ref _isChange, value);
        }


        public DelegateCommand ApplyCommand =>
            new DelegateCommand(ApplyAsync, IsApplyEnable).ObservesProperty(() => IsEnabled);
        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);
        public DelegateCommand<object> DeleteBtn => new DelegateCommand<object>(Delete);
        public DelegateCommand<object> ChoiseItem => new DelegateCommand<object>(ChoiseItem_ClickAsync);

        #endregion


        private async void ChoiseItem_ClickAsync(object obj)
        {
            if (!_isPressed)
            {
                _isPressed = true;

                var item = obj as CartWithProduct;

                NavigationParameters param = new NavigationParameters
                {
                   {"item", item.Product},
                };
                await _navigationService.NavigateAsync("Detail_Item", parameters: (param));
            }
        }

        private async void ApplyAsync()
        {
            _isPressed = true;
            IsApplyEnable();

            await _checkAndroid.PermissionsStatus();

            Total_Sum();

            if (_stateNethwork)
            {
                NavigationParameters param = new NavigationParameters
                {
                   {"sum", TotalSum},
                   {"list", BuyList}
                };

                await _navigationService.NavigateAsync("Checkout", parameters: (param));
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.Offline);
            }
        }

        private async void Total_Sum()
        {
            int sum = 0;

            if (BuyList != null && BuyList.Count > 0)
            {
                foreach (var item in BuyList)
                {
                    sum += item.Product.Price * item.BuyCount;
                }
            }

            if (sum == 0)
            {
                _login.CartStatus = 0;

                if (await _auth.UpdateAsync(_login))
                {
                    _settingsManager.IsCircleCart = false;
                }
            }
          
            TotalSum = sum.ToString();
        }

        private void Delete(object obj)
        {
            for (int i = 0; i < BuyList.Count; i++)
            {
                if (BuyList[i].Id == (int)obj)
                {
                    _carts.DeleteProductAsync(BuyList[i].Id);
                    BuyList.RemoveAt(i);
                    break;
                }
            }

            Total_Sum();
            IsApplyEnable();
        }

        private bool IsApplyEnable()//Enable disable "Apply" Button
        {
            IsEnabled = (!_isPressed
                        && BuyList != null
                        && BuyList.Count > 0
                        && TotalSum != null
                        && TotalSum.Length > 0)
                           ? IsEnabled = true : IsEnabled = false;

            if (IsEnabled)
            {
                Color_OkBtn = "#4285F4";
            }
            else
            {
                Color_OkBtn = "LightGrey";
            }

            return IsEnabled;
        }

        private async void Go_BackAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private async Task ConvertToCartWithProductAsync()
        {
            var cart = await _carts.GetListAsync();
          
            int sum = 0;

            for (int i = 0; i < cart.Count; i++)
            {
                var product = _staticList.FirstOrDefault(s => s.Id == cart[i].ProductId);

                if (product != null)
                {

                    BuyList.Add(new CartWithProduct()
                    {
                        BuyCount = cart[i].BuyCount,
                        Id = cart[i].Id,
                        Color = Color.Parse(cart[i].Color),
                        Date = cart[i].Date,
                        Email = cart[i].Email,
                        Product = product,
                        Size = cart[i].Size,
                        Status = cart[i].Status
                    });
                    sum += product.Price * cart[i].BuyCount;

                }
            }
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "IsChange":
                    Total_Sum();
                    IsApplyEnable();
                    break;

                default:
                    break;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _isPressed = false;
            IsApplyEnable();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            _isPressed = true;
            IsVisibleIndicator = true;

            BuyList = new ObservableCollection<CartWithProduct>();
            await ConvertToCartWithProductAsync();
            await Task.Delay(100);

            _isPressed = false;
            IsVisibleIndicator = false;

            Total_Sum();
            IsApplyEnable();
        }
    }
}
