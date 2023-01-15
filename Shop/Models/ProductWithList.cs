

namespace Shop.Models
{
    public class ProductWithList
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public string Brand { get; set; }
            public List<Photo> Photos_Large { get; set; }
            public List<string> Sizes { get; set; }
            public List<Color> Color { get; set; }
            public int Count { get; set; }
            public string Gender { get; set; }
            public bool IsNew { get; set; }
            public bool IsBestSeller { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
    }
}
