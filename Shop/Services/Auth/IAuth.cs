

using Shop.Models;


namespace Shop.Services.Auth
{
    public interface IAuth
    {
        Task<(bool, (string, Login))> AuthAsync(string password, string email);
        Task<(bool, string)> RegistrAsync(Login profile);
        Task<bool> UpdateAsync(Login log);
    }
}
