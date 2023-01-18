

using Shop.Helpers;
using Shop.Services.Auth;
using Shop.Services.Interfaces;
using Shop.Services.SettingsManager;
using Shop.Services.VerifyService;

using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class PassEmailEditViewModel : BaseViewModel, INavigatedAware
    {


        public PassEmailEditViewModel(INavigationService navigationService,
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
            _verifyInput = verifyInput;
            _auth = auth;

            Name = "";
            Email = "";
            Password = "";
            _isPressed = false;
            IsEnabled = false;

            ImagePassword = "ic_eye_off.png";
            IsVisiblePassword = true;

            Color_OkBtn = "LightGray";
            EmailBorderColor = Color.Parse("White");
            PassBorderColor = Color.Parse("White");
            NameBorderColor = Color.Parse("White");
        }


        #region Public Property


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


        public DelegateCommand PhotoEdit => new DelegateCommand(Photo_Edit);
        public DelegateCommand GoBack => new DelegateCommand(GoBackAsync);
        public DelegateCommand ConfirmCommand => new DelegateCommand(ConfirmAsync, IsConfirmEnable)
                                                            .ObservesProperty(() => IsEnabled);
        public DelegateCommand Btn_IsVisiblePassword => new DelegateCommand(Click_IsVisiblePassword);
        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);


        #endregion


        private async void ConfirmAsync()
        {

            if (_stateNethwork)
            {
                _isPressed = true;
                IsConfirmEnable();
                IsVisibleIndicator = true;

                _login.Name = Name;
                _login.Email = Email;
                _login.Password = Password;
                _login.ImageAccount = PhotoPath;


                Input_ErrorColor();
                _settingsManager.Name = Name;
                _settingsManager.Email = Email;
                _settingsManager.Password = Password;

                IsVisibleIndicator = false;


                if (!await _auth.UpdateAsync(_login))
                {
                    _printMessage.ViewMessage(Resources.Strings.Resource.SaveError);
                }
                else
                {
                    GoBackAsync();
                }

                IsVisibleIndicator = false;
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.Offline);
            }

            _isPressed = false;
        }

        private async void Photo_Edit()
        {
            var photo = await MediaPhoto.TakePhoto();

            if (photo != null && photo.Length > 0)
            {
                PhotoPath = photo;
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
            IsConfirmEnable();
        }

        private bool IsConfirmEnable()//Enable disable "Sign in" Button
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
                Color_OkBtn = "LightGray";
            }

            return IsEnabled;
        }

        private async void GoBackAsync()
        {
            Unfocused_Entry();
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
            IsConfirmEnable();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _isPressed = false;
            IsConfirmEnable();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            IsVisibleIndicator = false;
            _isPressed = false;
            PhotoPath = _login.ImageAccount;
            Name = _login.Name;
            Email = _login.Email;
            Password = _login.Password;
        }
    }
}
