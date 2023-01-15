using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Constants
{
    internal class Google
    {
        // CLIENT_ID = "491900135539-lvgih34mvhkgdj3vldgji5k7u2a5qurk.apps.googleusercontent.com";

        public static string AppName = "Shop";

        // For Google login, configure at https://console.developers.google.com/
        public static string AndroidClientId = "491900135539-lvgih34mvhkgdj3vldgji5k7u2a5qurk.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.491900135539-lvgih34mvhkgdj3vldgji5k7u2a5qurk:/oauth2redirect";
    }
}
