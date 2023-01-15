

namespace Shop.Services.SendEmail
{
    public interface ISendEmail
    {
        void SendPasswordAsync(string email);
    }
}
