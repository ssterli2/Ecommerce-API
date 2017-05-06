using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Ecomm.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ecomm.Controllers
{
    public class OrderController : Controller
    {   
        private EcommContext _context;
        public OrderController(
            EcommContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("Cart/makeorder/{UserId}")]
        [Consumes("application/json")]
        public IActionResult MakeOrder(int UserId)
        { 
            Order NewOrder = new Order { CreatedAt = DateTime.Now, UserId = UserId };
            Cart ExistingCart = _context.cart.Include(c => c.CartItems).ThenInclude(i => i.Product).Where(c => c.UserId == UserId && c.Status == "active").SingleOrDefault();
            if (ExistingCart == null)
            {
                return StatusCode(400);
            }
            foreach (var item in ExistingCart.CartItems)
            {
                OrderItem NewOrderItem = new OrderItem { Quantity = item.Quantity, Order = NewOrder, Product = item.Product };
                _context.Add(NewOrderItem);
                item.Product.Quantity -= item.Quantity;
            }
            ExistingCart.Status = "purchased";
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpGet]
        [Route("Orders/{UserId}")]
        [Consumes("application/json")]
        public IActionResult UserOrders(int UserId)
        {
            List<Order> UserOrders = _context.order.Include(o => o.OrderItems).ThenInclude(i => i.Product).Where(o => o.UserId == UserId).ToList();
            if (UserOrders.Count == 0)
            {
                string response = "This user has no orders";
                return Json(response);
            }
            return Json(UserOrders);
        }
    }
}