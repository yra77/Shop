

using Shop.Models;
using Shop.Converters;
using Shop.Services.Auth;
using Shop.Services.Interfaces;
using Shop.Services.Db_Products;
using Shop.Services.VerifyService;
using Shop.Services.SettingsManager;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Shop.ViewModels;

internal class MainPageViewModel : BaseViewModel, INavigatedAware
{


    public MainPageViewModel(INavigationService navigationService,
                             ICheck_AndroidServives checkAndroid,
                             ISettingsManager settingsManager,
                             IVerifyInputService verifyInput,
                             IChangeThemeService changeTheme,
                             IUnfocusedEntry unfocusedEntry,
                             IPrintMessage printMessage,
                             IProducts products,
                             IAuth auth)
    {

        _navigationService = navigationService;
        _settingsManager = settingsManager;
        _unfocusedEntry = unfocusedEntry;
        _checkAndroid = checkAndroid;
        _printMessage = printMessage;
        _changeTheme = changeTheme;
        _verifyInput = verifyInput;
        _products = products;
        _auth = auth;

        Email = "";
        Password = "";
        IsEnabled = false;

        ImagePassword = "ic_eye_off.png";
        IsVisiblePassword = true;

        Color_OkBtn = "LightGrey";
        EmailBorderColor = Color.Parse("White");
        PassBorderColor = Color.Parse("White");
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


    public DelegateCommand<string> SignUpCommand => new DelegateCommand<string>(ToSignUpAsync);
    public DelegateCommand SignInCommand =>
        new DelegateCommand(SignInAsync, IsSignInEnable).ObservesProperty(() => IsEnabled);
    public DelegateCommand SignGoogle => new DelegateCommand(Sign_Google);
    public DelegateCommand Btn_IsVisiblePassword => new DelegateCommand(Click_IsVisiblePassword);
    public DelegateCommand UnfocusedCommand => new DelegateCommand(Unfocused_Entry);

    #endregion


    private void Sign_Google()
    {
        if(_stateNethwork)
        {

        }
    }

    private async void SignInAsync()
    {
        _isPressed = true;
        IsSignInEnable();
        IsVisibleIndicator = true;

        var result = await _auth.AuthAsync(Password, Email);
        Unfocused_Entry();

        IsVisibleIndicator = false;

        if (result.Item1)
        {
            _settingsManager.Email = Email;
            _settingsManager.Password = Password;

            _login = result.Item2.Item2;

            await _navigationService.NavigateAsync("ShopMain");
        }
        else
        {
            _printMessage.ViewMessage(result.Item2.Item1);
            _isPressed = false;
        }
    }

    private void CheckEmail()
    {

        if (_email.Length > 0)
        {
            EmailBorderColor = Color.Parse("Red");
            ErrorEmailText = Resources.Strings.Resource.ErrorText_Email; ;

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

            IsSignInEnable();
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

            IsSignInEnable();
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

    private bool IsSignInEnable()//Enable disable "Sign in" Button
    {
        IsEnabled = (!_isPressed && _stateNethwork
            && PassBorderColor == Color.Parse("White")
            && EmailBorderColor == Color.Parse("White")
                && _email.Length > 0
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

    private async void ToSignUpAsync(string param)
    {
        Unfocused_Entry();
        if (_stateNethwork)
        {
            await _navigationService.NavigateAsync(param);
        }
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
            default:
                break;
        }
    }

    protected override void IsNethwork(bool state)
    {
        _stateNethwork = state;
        
        if (_stateNethwork)
        {
            _ = Task.Run(async () =>
            {
                if (_staticList == null || _staticList.Count == 0)
                {
                    var temp = await _products.GetProductsAsync();
                    _staticList = new ObservableCollection<ProductWithList>(ToProductWithLists.ConvertToProductWithLists(temp));
                }
            });
        }

        IsSignInEnable();
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        _isPressed = false;
        IsSignInEnable();
    }

    public async void OnNavigatedTo(INavigationParameters parameters)
    {
        await Task.Delay(250);

        await _checkAndroid.CheckServices(_changeNethwork);

        if (_settingsManager.Email != null
            && _settingsManager.Password != null)
        {
            Email = _settingsManager.Email;
            Password = _settingsManager.Password;
        }
    }
}
