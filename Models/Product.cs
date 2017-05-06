using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Ecomm.Models
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }
 
        public Product()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}