using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm.Models
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
