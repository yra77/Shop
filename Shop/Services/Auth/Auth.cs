

using Shop.Models;
using Shop.Services.HttpService;
using Shop.Services.Repository;
using Shop.Services.SettingsManager;
using Shop.Services.VerifyService;


namespace Shop.Services.Auth
{
    internal class Auth : IAuth
    {

        private readonly IRepository _repository;
        private readonly IRestService _restService;
        private readonly IVerifyInputService _verifyInput;
        private readonly ISettingsManager _settingsManager;


        public Auth(IRepository repository,
                    IRestService restService,
                    ISettingsManager settingsManager,
                    IVerifyInputService verifyInputService)
        {
            _verifyInput = verifyInputService;
            _settingsManager = settingsManager;
            _restService = restService;
            _repository = repository;
        }


        public async Task<(bool, (string, Login))> AuthAsync(string password, string email)
        {
            string str = null;

            if (_verifyInput.IsValidEmail(email))
            {
                if (_verifyInput.PasswordVerify(password))
                {
                    Login res = new Login();

                    res = await _restService.GetDataAsync<Login>("Email", email);

                    if (res == null)
                    {
                        res = (await _repository.GetDataAsync<Login>("Login", "Email", email)).FirstOrDefault();
                    }

                    if (res != null)
                    {
                        if (res.Password == password)
                        {
                            _settingsManager.IsCircleCart = (res.CartStatus > 0);
                            return (true, (str, res));
                        }
                        else
                        {
                            str = Resources.Strings.Resource.Alert_Password1;
                        }
                    }
                    else
                    {
                        str = Resources.Strings.Resource.Alert_Email2;
                    }
                }
                else
                {
                    str = Resources.Strings.Resource.Alert_Password2;
                }
            }
            else
            {
                str = Resources.Strings.Resource.Alert_Email3;
            }

            return (false, (str, null));
        }

        public async Task<(bool, string)> RegistrAsync(Login profile)
        {

            string str = null;

            if (_verifyInput.IsValidEmail(profile.Email))
            {
                if (profile.Name.Length > 2
                    && _verifyInput.PasswordVerify(profile.Password))
                {

                    if (await InsertAsync(profile))
                    {
                        _settingsManager.Name = profile.Name;
                        _settingsManager.Email = profile.Email;
                        _settingsManager.Password = profile.Password;
                        _settingsManager.Tel = profile.Tel;
                        _settingsManager.Address = profile.Address;
                        _settingsManager.IsCircleCart = (profile.CartStatus > 0);
                        return (true, str);
                    }
                    else
                    {
                        str = Resources.Strings.Resource.Alert_Email1;
                    }
                }
                else
                {
                    str = Resources.Strings.Resource.Alert_Password2;
                }
            }
            else
            {
                str = Resources.Strings.Resource.Alert_Email3;
            }

            return (false, str);
        }

        public async Task<bool> UpdateAsync(Login log)
        {
            try
            {
                var res = await _restService.GetDataAsync<Login>("Email", _settingsManager.Email);

                if (res != null)
                {
                    res = log;

                    return (await _restService.UpdateDataAsync(res.Id, res)
                            && await _repository.UpdateAsync(res));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update Error " + e.Message);
            }
            return false;
        }

        private async Task<bool> InsertAsync(Login profile)
        {
            var res = await _restService.GetDataAsync<Login>("Email", profile.Email);
            var resLocal = (await _repository.GetDataAsync<Login>("Login", "Email", profile.Email));

            if (res == null && await _restService.InsertAsync<Login>(profile))
            {
                var resNew = await _restService.GetDataAsync<Login>("Email", profile.Email);

                if (!resLocal.Any() && resNew != null
                    && await _repository.InsertAsync<Login>(resNew))
                {
                    return true;
                }
            }

                return false;
        }

    }
}
