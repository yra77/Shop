

using SQLite;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shop.Models
{
   // [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int Price { get; set; }
        public string Brand { get; set; }

      //  [OneToMany]
        public List<Photo> Photos_Large { get; set; }

        [NotNull]
        public string Sizes { get; set; }

        [NotNull]
        public string Colors { get; set; }

        [NotNull]
        public int Count { get; set; }

        [NotNull]
        public string Gender { get; set; }

        [NotNull]
        public bool IsNew { get; set; }

        [NotNull]
        public bool IsBestSeller { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public DateTime Date { get; set; }
    }
}
