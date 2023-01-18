

using Shop.Models;
using Shop.Converters;
using Shop.Services.Auth;
using Shop.Services.Interfaces;
using Shop.Services.SettingsManager;
using Shop.Services.VerifyService;

using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class SignUpViewModel : BaseViewModel, INavigatedAware
    {

        public SignUpViewModel(INavigationService navigationService,
                               ICheck_AndroidServives checkAndroid,
                               IVerifyInputService verifyInput,
                               ISettingsManager settingsManager,
                               IUnfocusedEntry unfocusedEntry,
                               IPrintMessage printMessage,
                               IAuth auth)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _unfocusedEntry = unfocusedEntry;
            _printMessage = printMessage;
            _checkAndroid = checkAndroid;
            _verifyInput = verifyInput;
            _auth = auth;

            Name = "";
            Email = "";
            Password = "";
            IsEnabled = false;

            ImagePassword = "ic_eye_off.png";
            IsVisiblePassword = true;

            Color_OkBtn = "LightGrey";
            EmailBorderColor = Color.Parse("White");
            PassBorderColor = Color.Parse("White");
            NameBorderColor = Color.Parse("White");
        }


        #region Public property

        private string _errorEmailText;
        public string ErrorEmailText
        {
            get => _errorEmailText;
            set => SetProperty(ref _errorEmailText, value);
        }


        private string _errorPassText;
        public string ErrorPassText
        {
            get => _errorPassText;
            set => SetProperty(ref _errorPassText, value);
        }


        private string _errorNameText;
        public string ErrorNameText
        {
            get => _errorNameText;
            set => SetProperty(ref _errorNameText, value);
        }


        private Color _emailBorderColor;
        public Color EmailBorderColor
        {
            get => _emailBorderColor;
            set => SetProperty(ref _emailBorderColor, value);
        }


        private Color _passBorderColor;
        public Color PassBorderColor
        {
            get => _passBorderColor;
            set => SetProperty(ref _passBorderColor, value);
        }


        private Color _nameBorderColor;
        public Color NameBorderColor
        {
            get => _nameBorderColor;
            set => SetProperty(ref _nameBorderColor, value);
        }


        private string _color_OkBtn;
        public string Color_OkBtn
        {
            get => _color_OkBtn;
            set => SetProperty(ref _color_OkBtn, value);
        }


        private string _imagePassword;
        public string ImagePassword
        {
            get => _imagePassword;
            set => SetProperty(ref _imagePassword, value);
        }


        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        private bool _isVisiblePassword;
        public bool IsVisiblePassword
        {
            get => _isVisiblePassword;
            set => SetProperty(ref _isVisiblePassword, value);
        }


        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        public DelegateCommand GoBackCommand => new DelegateCommand(GoBackAsync);
        public DelegateCommand SignInCommand => new DelegateCommand(SignInAsync, IsLogInEnable)
                                                            .ObservesProperty(() => IsEnabled);
        public DelegateCommand SignGoogle => new DelegateCommand(Sign_Google);
        public DelegateCommand Btn_IsVisiblePassword => new DelegateCommand(Click_IsVisiblePassword);
        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);

        #endregion


        private void Sign_Google()
        {
            if (_stateNethwork)
            {

            }
        }

        private async void SignInAsync()
        {
            if (_stateNethwork)
            {
                _isPressed = true;
                IsLogInEnable();
                IsVisibleIndicator = true;

                Login log = new Login();
                log.Name = _name;
                log.Email = _email;
                log.Password = Password;
                log.CartStatus = 0;
                log.FavoriteList = "";
                log.Address = "Address";
                log.Tel = "+0000000000";
                log.ImageAccount = ImageToByteArray.ToByteArr("editphoto.png");
                log.DateCreated = DateTime.Now;

                var result = await _auth.RegistrAsync(log);
                Unfocused_Entry();

                IsVisibleIndicator = false;

                if (result.Item1)
                {
                    Input_ErrorColor();
                    _login = log;
                    await _navigationService.NavigateAsync("ShopMain");
                }
                else
                {
                    _printMessage.ViewMessage(result.Item2);
                }
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.Offline);
            }
        }

        private void CheckName()
        {

            if (_name.Length > 0)
            {
                NameBorderColor = Color.Parse("Red");
                ErrorNameText = Resources.Strings.Resource.ErrorText_Name;

                string name = Name;

                if (!_verifyInput.NameVerify(ref name))
                {
                    Name = name;
                }
                else
                {
                    ErrorNameText = "Ok!";
                    NameBorderColor = Color.Parse("White");
                }

                IsLogInEnable();
            }
        }

        private void CheckEmail()
        {

            if (_email.Length > 0)
            {
                EmailBorderColor = Color.Parse("Red");
                ErrorEmailText = Resources.Strings.Resource.ErrorText_Email;

                string email = Email;

                if (!_verifyInput.EmailVerify(ref email))
                {
                    Email = email;
                }
                else
                {
                    if (_verifyInput.IsValidEmail(Email))
                    {
                        ErrorEmailText = "Ok!";
                        EmailBorderColor = Color.Parse("White");
                    }
                }

                IsLogInEnable();
            }
        }

        private void CheckPassword()
        {

            if (_password.Length > 0)
            {
                PassBorderColor = Color.Parse("Red");
                ErrorPassText = Resources.Strings.Resource.ErrorText_Password;

                string password = Password;

                if (!_verifyInput.PasswordCheckin(ref password))
                {
                    Password = password;
                }
                else
                {
                    if (_verifyInput.PasswordVerify(Password) && (Password.Length > 7 && Password.Length < 17))
                    {
                        ErrorPassText = "Ok!";
                        PassBorderColor = Color.Parse("White");
                    }
                }

                IsLogInEnable();
            }
        }

        private void Click_IsVisiblePassword()
        {
            if (IsVisiblePassword)
            {
                IsVisiblePassword = false;
                ImagePassword = "ic_eye.png";
            }
            else
            {
                IsVisiblePassword = true;
                ImagePassword = "ic_eye_off.png";
            }
        }

        private void Input_ErrorColor()
        {
            NameBorderColor = Color.Parse("White");
            EmailBorderColor = Color.Parse("White");
            PassBorderColor = Color.Parse("White");
            ErrorNameText = "";
            ErrorEmailText = "";
            ErrorPassText = "";
            IsLogInEnable();
        }

        private bool IsLogInEnable()//Enable disable "Sign in" Button
        {

            IsEnabled = (!_isPressed && _stateNethwork
                && PassBorderColor == Color.Parse("White")
                && EmailBorderColor == Color.Parse("White")
                && NameBorderColor == Color.Parse("White")
                && _email.Length > 0
                && _name.Length > 0
                && _password.Length > 0)
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

        private async void GoBackAsync()
        {
            Unfocused_Entry();
            await _navigationService.NavigateAsync("MainPage");
        }

        private void Unfocused_Entry()
        {
            _unfocusedEntry.HideKeyboard();
        }


        protected override void IsNethwork(bool state)
        {
            _stateNethwork = state;

            if (_stateNethwork)
            {
                if (_staticList == null || _staticList.Count == 0)
                {
                    _ = Task.Run(async () =>
                    {
                       var temp = await _products.GetProductsAsync();
                       _staticList = new ObservableCollection<ProductWithList>(ToProductWithLists.ConvertToProductWithLists(temp));
                    });
                }
            }

            IsLogInEnable();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "Email":
                    CheckEmail();
                    break;
                case "Password":
                    CheckPassword();
                    break;
                case "Name":
                    CheckName();
                    break;
                default:
                    break;
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _isPressed = false;
            IsLogInEnable();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}
