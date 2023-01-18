

using Shop.Services.Interfaces;
using Shop.Services.SendEmail;
using Shop.Services.VerifyService;
using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class RecoveryPasswordViewModel : BaseViewModel, INavigatedAware
    {

        private readonly ISendEmail _sendEmail;


        public RecoveryPasswordViewModel(INavigationService navigationService,
                                         IUnfocusedEntry unfocusedEntry,
                                         IVerifyInputService verifyInput,
                                         ISendEmail sendEmail)
        {
            _navigationService = navigationService;
            _unfocusedEntry = unfocusedEntry;
            _verifyInput = verifyInput;
            _sendEmail = sendEmail;

            Email = "";
            IsEnabled = false;

            Color_OkBtn = "LightGrey";
            EmailBorderColor = Color.Parse("White");
        }


        #region Public Property


        private string _errorEmailText;
        public string ErrorEmailText
        {
            get => _errorEmailText;
            set => SetProperty(ref _errorEmailText, value);
        }


        private Color _emailBorderColor;
        public Color EmailBorderColor
        {
            get => _emailBorderColor;
            set => SetProperty(ref _emailBorderColor, value);
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


        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);
        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);
        public DelegateCommand ContinueCommand => new DelegateCommand(SendPassAsync,
                                       IsOkEnable).ObservesProperty(() => IsEnabled);

        #endregion


        private async void SendPassAsync()
        {
            Unfocused_Entry();

            if (_email != null && _stateNethwork)
            {
                _sendEmail.SendPasswordAsync(Email);
                await _navigationService.NavigateAsync("MainPage");
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

                IsOkEnable();
            }
        }

        private bool IsOkEnable()//Enable disable "Sign in" Button
        {
            IsEnabled = (_stateNethwork
                         && EmailBorderColor == Color.Parse("White")
                         && _email.Length > 0)
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
                default:
                    break;
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
    }
}
