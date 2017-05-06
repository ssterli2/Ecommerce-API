using Microsoft.AspNetCore.Mvc;
using Ecomm.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ecomm.Controllers
{
    public class CartController : Controller
    {   
        private EcommContext _context;
        public CartController(
            EcommContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Cart/add")]
        [Consumes("application/json")]
        public IActionResult AddToCart([FromBody] CartItemViewModel model)
        { 
            Product ProductToAdd = _context.product.Where(p => p.ProductId == model.ProductId).SingleOrDefault();
            Cart ActiveCart;
            Cart ExistingCart = _context.cart.Include(c => c.CartItems).Where(c => c.UserId == model.UserId && c.Status == "active").SingleOrDefault();
            if (ExistingCart == null)
            {
                Cart NewCart = new Cart { UserId = model.UserId, Status = "active" };
                _context.Add(NewCart);
                _context.SaveChanges();
                ActiveCart = NewCart;
            }
            else
            {
                ActiveCart = ExistingCart;
            }
            if (ActiveCart.ContainsCartItem(ProductToAdd) == false)
            {   
                if (model.Quantity <= ProductToAdd.Quantity)
                {
                    CartItem NewCartItem = new CartItem { Quantity = model.Quantity, Cart = ActiveCart, Product = ProductToAdd };
                    _context.Add(NewCartItem);
                    _context.SaveChanges();
                    return StatusCode(201);
                }
                else
                {
                    string response = "This quantity is not available";
                    return Json(response);
                }
            }
            else
            {
                string response = "This product is already in your cart. Please update quantity in cart.";
                return Json(response);
            }
        }


        [HttpPost]
        [Route("Cart/remove/{CartItemId}")]
        [Consumes("application/json")]
        public IActionResult RemoveFromCart(int CartItemId)
        {
            CartItem ItemToRemove = _context.cartItem.Where(i => i.Id == CartItemId).SingleOrDefault();
            if (ItemToRemove != null)
            {
                ItemToRemove.CartId = null;
                _context.SaveChanges();
                return StatusCode(203);
            }
            return StatusCode(400);
        }

        [HttpPost]
        [Route("Cart/update")]
        [Consumes("application/json")]
        public IActionResult UpdateCartItem([FromBody] CartItemViewModel model)
        {
            CartItem ItemToUpdate = _context.cartItem.Include(i => i.Product).Where(i => i.Id == model.CartItemId).SingleOrDefault();
            if (ItemToUpdate != null)
            {
                if (model.Quantity <= ItemToUpdate.Product.Quantity)
                {
                     ItemToUpdate.Quantity = model.Quantity;
                    _context.SaveChanges();
                    return StatusCode(201);
                }
                else
                {
                    string response = "This quantity is not available";
                    return Json(response);
                }  
            }
            return StatusCode(400);
        }

        [HttpGet]
        [Route("Cart/{UserId}")]
        [Consumes("application/json")]
        public IActionResult UserCart(int UserId)
        {
            Cart UserCart = _context.cart.Include(c => c.CartItems).ThenInclude(i => i.Product).Where(c => c.UserId == UserId && c.Status == "active").SingleOrDefault();
            if (UserCart == null)
            {
                string response = "There are no items in your cart";
                return Json(response);
            }
            return Json(UserCart);
        }
    }
}