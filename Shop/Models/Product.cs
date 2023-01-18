

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Brand { get; set; }
        public List<Photo> Photos_Large { get; set; }
        public string Sizes { get; set; }
        public string Colors { get; set; }
        public int Count { get; set; }
        public string Gender { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSeller { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
