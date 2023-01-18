

namespace Shop.Models
{
    
    public class Login
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }
        public int CartStatus { get; set; }//0 - default, 1 - choise, 2 - complete, 3 - sending

        public string FavoriteList { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }

        public byte[] ImageAccount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

