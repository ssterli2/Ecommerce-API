
using Newtonsoft.Json;

namespace Ecomm.Models
{
    public class OrderItem : BaseEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}