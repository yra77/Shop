

using Shop.Models;
using Shop.Services.HttpService;
using Shop.Services.SettingsManager;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Shop.Services.SendEmail
{
    internal class SendEmail : ISendEmail
    {

        private readonly IRestService _restService;
        private readonly ISettingsManager _settingsManager;
        private readonly INavigationService _navigationService;


        public SendEmail(ISettingsManager settingsManager,
                         IRestService restService,
                         INavigationService navigationService)
        {
            _restService = restService;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
        }

        public async void SendPasswordAsync(string email)
        {

            _settingsManager.Email = email;
            _settingsManager.Password = await PasswordFromDb(email);

            await _navigationService.NavigateAsync("MainPage");


            //нужен ssl сертификат на сервере

            //string subject = "Recovery password";
            //string body = await PasswordFromDb(email);

            //var mailMessage = new MimeMessage();
            //mailMessage.From.Add(new MailboxAddress("Shop", "kceni.kceni.19@gmail.com"));
            //mailMessage.To.Add(new MailboxAddress("to name", email));
            //mailMessage.Subject = subject;
            //mailMessage.Body = new TextPart("plain")
            //{
            //    Text = body
            //};

            //using (var smtpClient = new SmtpClient())
            //{

            //    smtpClient.Connect("smtp.mailtrap.io", 465, false);
            //    smtpClient.Authenticate("d3e9abeba38410", "3217f8a6f6e43b");
            //    smtpClient.Send(mailMessage);
            //    smtpClient.Disconnect(true);
            //}

        }

        private async Task<string> PasswordFromDb(string email)
        {
           Login res = await _restService.GetDataAsync<Login>("Email", email);

            return res.Password;
        }

    }
}
