

using Shop.Models;
using Shop.Converters;
using Shop.Services.Auth;
using Shop.Services.Db_Cart;
using Shop.Services.Interfaces;
using Shop.Services.VerifyService;
using Shop.Services.SettingsManager;

using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class CheckoutViewModel : BaseViewModel, INavigatedAware
    {

        static int _prevLength = -1;//preview length
        static int _prevDateLength = -1;//preview date
        private bool _isEmailEdit;
        private bool _isMenuOpen;


        public CheckoutViewModel(INavigationService navigationService,
                                 ISettingsManager settingsManager,
                                 IVerifyInputService verifyInput,
                                 IUnfocusedEntry unfocusedEntry,
                                 IPrintMessage printMessage,
                                 ICart carts,
                                 IAuth auth)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _unfocusedEntry = unfocusedEntry;
            _printMessage = printMessage;
            _verifyInput = verifyInput;
            _carts = carts;
            _auth = auth;

            TraslateY = 400;
            ScalePayCard = 0;
            _isMenuOpen = false;

            IsEnabled = false;
            IsVisibleEdit = false;
            _isVisibleComplete = false;
            ScaleEdit = 0.0;
            TotalSum = "0$";
            Color_OkBtn = "LightGrey";

            BuyList = new ObservableCollection<CartWithProduct>();
        }


        #region public property


        private double _traslateY;
        public double TraslateY
        {
            get => _traslateY;
            set => SetProperty(ref _traslateY, value);
        }


        private double _scalePayCard;
        public double ScalePayCard
        {
            get => _scalePayCard;
            set => SetProperty(ref _scalePayCard, value);
        }


        private bool _isCardVisible;
        public bool IsCardVisible
        {
            get => _isCardVisible;
            set => SetProperty(ref _isCardVisible, value);
        }


        private string _cardCvv;
        public string CardCvv
        {
            get => _cardCvv;
            set => SetProperty(ref _cardCvv, value);
        }


        private string _cardDate;
        public string CardDate
        {
            get => _cardDate;
            set => SetProperty(ref _cardDate, value);
        }


        private string _cardNum;
        public string CardNum
        {
            get => _cardNum;
            set => SetProperty(ref _cardNum, value);
        }


        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }


        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        private string _tel;
        public string Tel
        {
            get => _tel;
            set => SetProperty(ref _tel, value);
        }


        private string _mapAddress;
        public string MapAddress
        {
            get => _mapAddress;
            set => SetProperty(ref _mapAddress, value);
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


        private string _editStr;
        public string Edit_Str
        {
            get => _editStr;
            set => SetProperty(ref _editStr, value);
        }


        private string _errorEditText;
        public string ErrorEditText
        {
            get => _errorEditText;
            set => SetProperty(ref _errorEditText, value);
        }


        private Color _editBorderColor;
        public Color EditBorderColor
        {
            get => _editBorderColor;
            set => SetProperty(ref _editBorderColor, value);
        }


        private double _scaleEdit;
        public double ScaleEdit
        {
            get => _scaleEdit;
            set => SetProperty(ref _scaleEdit, value);
        }


        private bool _isVisibleEdit;
        public bool IsVisibleEdit
        {
            get => _isVisibleEdit;
            set => SetProperty(ref _isVisibleEdit, value);
        }


        private bool _isVisibleComplete;
        public bool IsVisibleComplete
        {
            get => _isVisibleComplete;
            set => SetProperty(ref _isVisibleComplete, value);
        }


        private ObservableCollection<CartWithProduct> _buyList;
        public ObservableCollection<CartWithProduct> BuyList
        {
            get => _buyList;
            set => SetProperty(ref _buyList, value);
        }


        public DelegateCommand ConfirmCommand =>
           new DelegateCommand(СonfirmAsync, IsСonfirmEnable).ObservesProperty(() => IsEnabled);
        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);
        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);
        public DelegateCommand<string> EditBtn => new DelegateCommand<string>(Edit);
        public DelegateCommand<string> OkCancelEdit => new DelegateCommand<string>(OkCancel_Click);
        public DelegateCommand Complite => new DelegateCommand(Go_ToShopAsync);
        public DelegateCommand CardEdit => new DelegateCommand(OpenCardEdit);

        #endregion


        private async void Go_ToShopAsync()
        {
            await _navigationService.NavigateAsync("ShopMain");
        }

        private async void СonfirmAsync()
        {
            if (_stateNethwork)
            {
                //при покупке изменить в бд кол-во такой обуви
                // если были изменения изменить в cart db
                // Email, Tel, Address, TotalSum, BuyList
                if (!_isPressed)
                {
                    _isPressed = true;
                    IsСonfirmEnable();

                    _settingsManager.Address = Address;
                    _settingsManager.Email = Email;
                    _settingsManager.Tel = Tel;

                    bool isSend = true;

                    foreach (var item in BuyList)
                    {

                        Cart cart = new Cart()
                        {
                            BuyCount = item.BuyCount,
                            Color = ColorToString.Convert(item.Color),
                            Date = DateTime.Now,
                            Email = item.Email,
                            ProductId = item.Product.Id,
                            Size = item.Size,
                            Status = 2,
                            Id = item.Id
                        };

                        if (!await _carts.UpdateAsync(cart))
                        {
                            isSend = false;
                        }
                    }

                    if (isSend)
                    {
                        _login.CartStatus = 2;
                        await _auth.UpdateAsync(_login);// number 2 - the buy is complete

                        IsVisibleComplete = true;

                        for (int i = 0; i < 100; i += 10)
                        {
                            ScaleEdit = (double)i / 100;
                            await Task.Delay(1);
                        }
                    }
                    else
                    {
                        _printMessage.ViewMessage(Resources.Strings.Resource.SaveError);
                    }

                    _isPressed = false;
                }
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.Offline);
            }
        }

        private async void OpenCardEdit()
        {

            if (!_isMenuOpen)
            {
                IsCardVisible = true;
                AnimationAsync(400, 0, 1);
                _isMenuOpen = true;
            }
            else
            {
                Unfocused_Entry();
                AnimationAsync(0, 600, 1);
                _isMenuOpen = false;
                await Task.Delay(300);
                IsCardVisible = false;
            }
        }

        private async void AnimationAsync(int start, int end, int duration)
        {
            if (start < end)
            {
                for (int i = start; i < end; i += 20)
                {
                    TraslateY = i;
                    ScalePayCard = 1.0 - (double)i / 1000;
                    await Task.Delay(duration);
                }
            }
            else
            {
                for (int i = start; i >= end; i -= 20)
                {
                    TraslateY = i;
                    ScalePayCard = 1.0 - (double)i / 1000;
                    await Task.Delay(duration);
                }
            }
        }

        private async void EditAnimationsAsync(int start, int end, int duration)
        {
            if (start < end)
            {
                IsVisibleEdit = true;
                for (int i = start; i < end; i += 10)
                {
                    ScaleEdit = (double)i / 100;
                    await Task.Delay(duration);
                }
            }
            else
            {
                for (int i = start; i >= end; i -= 10)
                {
                    ScaleEdit = (double)i / 100;
                    await Task.Delay(duration);
                }
                IsVisibleEdit = false;
            }
        }

        private void Edit(string param)
        {
            EditAnimationsAsync(0, 100, 1);

            if (param == "email")
            {
                _isEmailEdit = true;
                Edit_Str = Email;
            }
            else
            {
                _isEmailEdit = false;
                Edit_Str = Tel;
            }
        }

        private void OkCancel_Click(string param)
        {
            if (param == "ok")
            {
                if (_isEmailEdit)
                {
                    if (_verifyInput.IsValidEmail(Edit_Str))
                    {
                        Email = Edit_Str;
                        EditAnimationsAsync(100, 0, 1);
                    }
                    else
                    {
                        EditBorderColor = Color.Parse("Red");
                        ErrorEditText = Resources.Strings.Resource.ErrorText_Email;
                    }
                }
                else
                {
                    if (_verifyInput.TelVerify(Edit_Str))
                    {
                        Tel = Edit_Str;
                        EditAnimationsAsync(100, 0, 1);
                    }
                    else
                    {
                        EditBorderColor = Color.Parse("Red");
                        ErrorEditText = Resources.Strings.Resource.ErrorText_Tel;
                    }
                }
            }
            else
            {
                EditAnimationsAsync(100, 0, 1);
            }

            IsСonfirmEnable();
        }

        private void CheckTel()
        {
            if (_editStr.Length > 0)
            {
                EditBorderColor = Color.Parse("Red");
                ErrorEditText = Resources.Strings.Resource.ErrorText_Tel;

                    if (_verifyInput.TelVerify(Edit_Str))
                    {
                        ErrorEditText = "Ok!";
                        EditBorderColor = Color.Parse("White");
                    }
            }
        }

        private void CheckEmail()
        {

            if (_editStr.Length > 0)
            {
                EditBorderColor = Color.Parse("Red");
                ErrorEditText = Resources.Strings.Resource.ErrorText_Email;

                string email = Edit_Str;

                if (!_verifyInput.EmailVerify(ref email))
                {
                    Edit_Str = email;
                }
                else
                {
                    if (_verifyInput.IsValidEmail(Edit_Str))
                    {
                        ErrorEditText = "Ok!";
                        EditBorderColor = Color.Parse("White");
                    }
                }
            }
        }

        private void CheckAddress()
        {
            Address = MapAddress.Replace(';', ' ');
        }

        private bool IsСonfirmEnable()//Enable disable "Apply" Button
        {
            if(!_isPressed
               && BuyList != null && BuyList.Count > 0
               && TotalSum != null && TotalSum.Length > 0
               && Email != null
               && Tel != null
               && Address != null
               && _verifyInput.IsValidEmail(Email)
               && _verifyInput.TelVerify(Tel))
            {
                Color_OkBtn = "#4285F4";
                IsEnabled = true;
            }
            else
            {
                Color_OkBtn = "LightGrey";
                IsEnabled = false;
            }

            return IsEnabled;
        }

        private async void Go_BackAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private void Unfocused_Entry()
        {
            _unfocusedEntry.HideKeyboard();
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "MapAddress":
                    CheckAddress();
                    IsVisibleIndicator = false;
                    _isPressed = false;
                    break;

                case "Edit_Str":
                    if (_isEmailEdit)
                    {
                        CheckEmail();
                    }
                    else
                    {
                        CheckTel();
                    }
                    break;

                case "CardDate":
                    if (_prevDateLength < CardDate.Length)
                    {
                        CardDate = _verifyInput.NumVerify(_cardDate, true);
                    }
                    _prevDateLength = CardDate.Length;
                    break;

                case "CardNum":
                    if (_prevLength < CardNum.Length)
                    {
                        CardNum = _verifyInput.NumCardVerify(_cardNum);
                    }
                    _prevLength = CardNum.Length;
                    break;

                case "CardCvv":
                    CardCvv = _verifyInput.NumVerify(_cardCvv, false);
                    break;

                default:
                    break;
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            IsVisibleEdit = false;
            _isVisibleComplete = false;
            _isPressed = false;
            IsСonfirmEnable();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            IsVisibleIndicator = true;
            _isMenuOpen = false;
            _isPressed = false;
            IsCardVisible = false;

            TotalSum = parameters.GetValue<string>("sum");
            BuyList = parameters.GetValue<ObservableCollection<CartWithProduct>>("list");

            Email = _settingsManager.Email;
            Tel = _settingsManager.Tel ?? "+000000000000";
            Address = _settingsManager.Address ?? "";

            IsСonfirmEnable();
        }
    }
}
