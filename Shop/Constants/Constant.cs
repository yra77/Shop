

namespace Shop.Constants
{
    internal class Constant
    {
        public const string REST_Product_URL = $"http://192.168.0.107:5249/api/Products";//адреса product сервера
        public const string REST_Cart_URL = $"http://192.168.0.107:5249/api/Carts";//адреса cart сервера
        public const string Rest_LOGIN_URL = $"http://192.168.0.107:5249/api/LogIns";//адреса login сервера

        public const int UpdateLocal_Db_Hourse = 24;//періодичність оновлення локальної БД в годинах
    }
}
