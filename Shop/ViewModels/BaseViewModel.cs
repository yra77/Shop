

using Shop.Models;
using Shop.Services.Auth;
using Shop.Services.Db_Cart;
using Shop.Services.Db_Products;
using Shop.Services.Interfaces;
using Shop.Services.SettingsManager;
using Shop.Services.VerifyService;
using System.Collections.ObjectModel;


namespace Shop.ViewModels
{

    public delegate void ChangeNethwork(bool state);

    internal class BaseViewModel : BindableBase
    {

        protected static ObservableCollection<ProductWithList> _staticList;
        protected static ObservableCollection<int> _favoriteList;
        protected static bool _stateNethwork;
        protected static Login _login;

        protected bool _isPressed;

        protected INavigationService _navigationService;
        protected ISettingsManager _settingsManager;
        protected IVerifyInputService _verifyInput;
        protected ICheck_AndroidServives _checkAndroid;
        protected IUnfocusedEntry _unfocusedEntry;
        protected IChangeThemeService _changeTheme;
        protected IPrintMessage _printMessage;
        protected IProducts _products;
        protected IAuth _auth;
        protected ICart _carts;

        protected ChangeNethwork _changeNethwork;


        static BaseViewModel()
        {
            _stateNethwork = true;
            _staticList = new ObservableCollection<ProductWithList>();
            _favoriteList = new ObservableCollection<int>();
            _login = new Login();
        }

        public BaseViewModel()
        {
            _changeNethwork = IsNethwork;
            _isPressed = false;
        }


        #region public property


        private bool _isVisibleIndicator;
        public bool IsVisibleIndicator
        {
            get => _isVisibleIndicator;
            set => SetProperty(ref _isVisibleIndicator, value);
        }


        #endregion


        protected virtual void IsNethwork(bool state)
        {
            _stateNethwork = state;
        }
    }
}
