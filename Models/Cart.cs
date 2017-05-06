
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ecomm.Models
{
    public class Cart : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }

        public List<CartItem> CartItems { get; set; }
 
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public bool ContainsCartItem(Product ProductToAdd)
        {
            return CartItems.Where(i => i.Product == ProductToAdd).Count() > 0;
        }
    }
}