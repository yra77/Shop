

using Shop.Services.Interfaces;
using Shop.Services.Db_Products;
using Shop.Services.SettingsManager;
using Shop.Models;
using System.Collections.ObjectModel;
using Shop.Services.Auth;

namespace Shop.ViewModels
{
    internal class FavoritePageViewModel : BaseViewModel, INavigatedAware
    {


        public FavoritePageViewModel(INavigationService navigationService,
                                     ICheck_AndroidServives checkAndroid,
                                     ISettingsManager settingsManager,
                                     IProducts products,
                                     IPrintMessage printMessage,
                                     IAuth auth)
        {

            _navigationService = navigationService;
            _settingsManager = settingsManager;
            _checkAndroid = checkAndroid;
            _printMessage = printMessage;
            _products = products;
            _auth = auth;

            FavoriteList = new ObservableCollection<ProductWithList>();
        }


        #region Public Property


        private bool _isCircleFavorit;
        public bool IsCircleFavorit
        {
            get => _isCircleFavorit;
            set => SetProperty(ref _isCircleFavorit, value);
        }


        public ObservableCollection<ProductWithList> _favorite;
        public ObservableCollection<ProductWithList> FavoriteList
        {
            get => _favorite;
            set => SetProperty(ref _favorite, value);
        }


        public DelegateCommand GoBack => new DelegateCommand(Go_BackAsync);
        public DelegateCommand<object> ChoiseItem => new DelegateCommand<object>(ChoiseItem_ClickAsync);
        public DelegateCommand<object> DeleteBtn => new DelegateCommand<object>(DeleteItem_ClickAsync);


        #endregion


        private async void DeleteItem_ClickAsync(object obj)
        {
            var item = obj as ProductWithList;

            string ids = "";

            for (int i = 0; i < FavoriteList.Count; i++)
            {
                if (FavoriteList[i].Id == item.Id)
                {
                    _favoriteList.RemoveAt(i);
                    FavoriteList.RemoveAt(i);
                }
                else
                {
                    ids += _favoriteList[i].ToString() + " ";
                }
            }

            ids = ids.Trim();
            _login.FavoriteList = ids;

            if(FavoriteList == null || FavoriteList.Count == 0)
            {
                _settingsManager.IsCircleFavorit = false;
                IsCircleFavorit = false;
            }

            if (!await _auth.UpdateAsync(_login))
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.SaveError);
            }
            else
            {
                _printMessage.ViewMessage(Resources.Strings.Resource.DeleteFavorite, "#228B22");
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

        private async void Go_BackAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private void CreateFavorite()
        {
            if(_favoriteList.Count > 0)
            {
                foreach (var item in _favoriteList)
                {
                    FavoriteList.Add(_staticList.FirstOrDefault(s=>s.Id == item));
                }
            }
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            IsCircleFavorit = _settingsManager.IsCircleFavorit;

            if (FavoriteList.Count == 0)
            {
                CreateFavorite();
            }
        }
    }
}
