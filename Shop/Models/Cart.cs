

namespace Shop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string Email { get; set; } 
        public int ProductId { get; set; }
        public int BuyCount { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Status { get; set; }// 0 - default, 1 - choise, 2 - complete, 3 - sending
        public DateTime Date { get; set; }
    }
}
