
using Newtonsoft.Json;

namespace Ecomm.Models
{
    public class CartItem : BaseEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? CartId { get; set; }
        
        [JsonIgnore]
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}