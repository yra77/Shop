

using SQLite;


namespace Shop.Models
{
    [Table("Login")]
    public class Login
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        [MaxLength(20)]
        public string Name { get; set; }

        [NotNull]
        [Unique]
        [MaxLength(40)]
        public string Email { get; set; }
        [NotNull]
        [MaxLength(20)]
        public string Password { get; set; }
        public int CartStatus { get; set; }//0 - default, 1 - choise, 2 - complete, 3 - sending

        [MaxLength(55)]
        public string FavoriteList { get; set; }

        [MaxLength(15)]
        public string Tel { get; set; }
        [MaxLength(250)]
        public string Address { get; set; }

        public byte[] ImageAccount { get; set; }
        [NotNull]
        public DateTime DateCreated { get; set; }
    }
}

