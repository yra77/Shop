

using Shop.Models;
using Shop.Converters;
using Shop.Services.Auth;
using Shop.Services.Db_Cart;
using Shop.Services.Interfaces;
using Shop.Services.SettingsManager;


namespace Shop.ViewModels
{
    class Detail_ItemViewModel : BaseViewModel, INavigatedAware
    {

        private string _email;
        private string _color;
        private string _size;


        public Detail_ItemViewModel(INavigationService navigationService,
                                    ICheck_AndroidServives checkAndroid,
                                    ISettingsManager settingsManager,
                                    IPrintMessage printMessage,
                                    IAuth auth,
                                    ICart carts)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _printMessage = printMessage;
            _checkAndroid = checkAndroid;
            _carts = carts;
            _auth = auth;

            Item = new ProductWithList();
        }


        #region public Property


        private ProductWithList _item;
        public ProductWithList Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private bool _isCircleCart;
        public bool IsCircleCart
        {
            get => _isCircleCart;
            set => SetProperty(ref _isCircleCart, value);
        }


        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set => SetProperty(ref _titleText, value);
        }


        private string _isBestNewText;
        public string IsBestNewText
        {
            get => _isBestNewText;
            set => SetProperty(ref _isBestNewText, value);
        }


        public DelegateCommand CartClick => new DelegateCommand(Cart_ClickAsync);
        public DelegateCommand AddToCartClick => new DelegateCommand(AddToCart_ClickAsync);
        public DelegateCommand GoBack => new DelegateCommand(GoBack_ClickAsync);
        public DelegateCommand<Color> ColorBtn => new DelegateCommand<Color>(Color_Click);
        public DelegateCommand<string> SizeBtn => new DelegateCommand<string>(Size_Click);

        #endregion



        private void Size_Click(string size)
        {
            _size = size;
        }

        private void Color_Click(Color color)
        {
                _color = ColorToString.Convert(color);        
        }

        private async void AddToCart_ClickAsync()
        {
            if (_stateNethwork)
            {
                if (_color != null && _size != null && _size.Length > 0
                    && _item.Count > 0 && _email != null)
                {
                    Cart cart = new Cart()
                    {
                        BuyCount = 1,
                        Color = _color,
                        Date = DateTime.Now,
                        Email = _email,
                        ProductId = Item.Id,
                        Size = _size,
                        Status = 1
                    };

                    _login.CartStatus = 1;

                    if (await _carts.InsertAsync(cart) && await _auth.UpdateAsync(_login))// number 1 - the buy is add to cart
                    {
                        IsCircleCart = true;
                        _settingsManager.IsCircleCart = true;

                        GoBack_ClickAsync();
                    }
                    else
                    {
                        _printMessage.ViewMessage(Resources.Strings.Resource.SaveError);
                    }
                }
                else
                {
                    _printMessage.ViewMessage(Resources.Strings.Resource.AddToCartError);
                }
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.Offline);
            }
        }

        private async void Cart_ClickAsync()
        {
            await _navigationService.NavigateAsync("CartPage");
        }

        private async void GoBack_ClickAsync()
        {
            await _navigationService.GoBackAsync();
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {           
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("item"))
            {
                Item = parameters.GetValue<ProductWithList>("item");

                IsCircleCart = _settingsManager.IsCircleCart;
                _email = _settingsManager.Email;

                if (Item.IsBestSeller)
                {
                    IsBestNewText = "BEST SELLER";
                }
                else
                {
                    IsBestNewText = "BEST CHOISE";
                }

                switch (Item.Gender)
                {
                    case "m":
                        TitleText = Resources.Strings.Resource.Mens;
                        break;
                    case "w":
                        TitleText = Resources.Strings.Resource.Womens;
                        break;
                    case "u":
                    default:
                        TitleText = Resources.Strings.Resource.Uni;
                        break;
                }
            }
        }
    }
}
