

using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }

       // [ForeignKey(typeof(Product))]
        public int ProductId { get; set; }
    }
}
