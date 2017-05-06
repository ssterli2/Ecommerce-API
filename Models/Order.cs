using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Ecomm.Models
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
 
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
