

using Shop.Models;
using Shop.Services.SettingsManager;
using System.ComponentModel;


namespace Shop.ViewModels
{
    internal class FilterModalViewModel : BaseViewModel, INavigatedAware
    {

        private List<string> _selectSizes;
        private List<string> _selectBrands;
        private List<string> _gender;


        public FilterModalViewModel(INavigationService navigationService,
                                     ISettingsManager settingsManager)
        {
            _navigationService = navigationService;
            _settingsManager = settingsManager;

            _selectSizes = new List<string>();
            _selectBrands = new List<string>();
            _gender = new List<string>();
            Size_s = new Sizes();
            Brand_s = new Brands();
        }


        #region public property


        private Sizes _sizes;
        public Sizes Size_s
        {
            get => _sizes;
            set => SetProperty(ref _sizes, value);
        }


        private Brands _brands;
        public Brands Brand_s
        {
            get => _brands;
            set => SetProperty(ref _brands, value);
        }


        private int _maxPrice;
        public int MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }


        private int _minPrice;
        public int MinPrice
        {
            get => _minPrice;
            set => SetProperty(ref _minPrice, value);
        }


        public DelegateCommand<string> BrandBtn => new DelegateCommand<string>(Brand_Click);
        public DelegateCommand<string> SizeBtn => new DelegateCommand<string>(Sizes_Click);
        public DelegateCommand<string> GendersChoise => new DelegateCommand<string>(GenderChoise);
        public DelegateCommand ApplyCommand => new DelegateCommand(ApplyAsync);


        #endregion


        private async void ApplyAsync()
        {
            if (_selectSizes.Count == 0
                && _gender.Count == 0
                && MinPrice == 0
                && MaxPrice == 0
                && _selectBrands.Count == 0)
            {
                await _navigationService.GoBackAsync();
            }
            else
            {
                NavigationParameters param = new NavigationParameters
                {
                    {"sizes", _selectSizes},
                    {"gender", _gender },
                    {"brands", _selectBrands },
                    {"minPrice", MinPrice },
                    {"maxPrice", MaxPrice }
                };
                await _navigationService.GoBackAsync(parameters: (param));
            }
        }

        private void Brand_Click(string item)
        {
            if (_selectBrands.Count > 0
                && _selectBrands.Contains(item))
            {
                _selectBrands.Remove(item);
            }
            else
            {
                _selectBrands.Add(item);
            }
        }

        private void Sizes_Click(string item)
        {
            if (_selectSizes.Count > 0
                && _selectSizes.Contains(item))
            {
                _selectSizes.Remove(item);
            }
            else
            {
                _selectSizes.Add(item);
            }
        }

        private void GenderChoise(string item)
        {
            if (_gender.Count > 0
                && _gender.Contains(item))
            {
                _gender.Remove(item);
            }
            else
            {
                _gender.Add(item[0].ToString());
            }
        }



        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "MinPrice":
                   // Console.WriteLine(MinPrice);
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
