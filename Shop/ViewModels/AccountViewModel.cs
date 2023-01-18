

using Shop.Services.Auth;
using Shop.Services.Interfaces;
using Shop.Services.Db_Products;
using Shop.Services.VerifyService;
using Shop.Services.SettingsManager;

using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class AccountViewModel : BaseViewModel, INavigatedAware
    {


        public AccountViewModel(INavigationService navigationService,
                                ICheck_AndroidServives checkAndroid,
                                ISettingsManager settingsManager,
                                IVerifyInputService verifyInput,
                                IUnfocusedEntry unfocusedEntry,
                                IPrintMessage printMessage,
                                IProducts products,
                                IAuth auth)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _unfocusedEntry = unfocusedEntry;
            _printMessage = printMessage;
            _checkAndroid = checkAndroid;
            _verifyInput = verifyInput;
            _products = products;
            _auth = auth;
        }



        #region Public Property


       

        public DelegateCommand PassEmail => new DelegateCommand(PassEmailEdit);
        public DelegateCommand DeleteAccount => new DelegateCommand(Delete_Account);
        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);

        #endregion


        private async void PassEmailEdit()
        {
            await _navigationService.NavigateAsync("PassEmailEdit");
        }

        private void Delete_Account()
        {
            
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
               

                default:
                    break;
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _isPressed = false;
        }
    }
}
