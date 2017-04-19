using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Ecomm.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System;
 
namespace Ecomm.Controllers
{
    public class EcommerceController : Controller
    {   
        private EcommContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public EcommerceController(
            EcommContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

         private User GetCurrentUser()
        {
            var result =  _userManager.GetUserAsync(HttpContext.User);
            result.Wait();
            return result.Result;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {   
            ViewBag.Users = _context.user.Take(3).ToList();
            ViewBag.Products = _context.product.Take(5).ToList();
            ViewBag.Orders = _context.order.Take(3).ToList();
            return View("Dashboard");
        }

        [HttpGet]
        [Route("Orders")]
        public IActionResult Orders()
        {   
            ViewBag.Orders = _context.order.ToList();
            ViewBag.Users = _context.user.ToList();
            ViewBag.Products = _context.product.ToList();
            return View("Order");
        }

        [HttpPost]
        [Route("Orders/makeorder")]
        public IActionResult MakeOrder(string Customer, int Product, int Quantity)
        { 
            User ForUser = _context.user.Where(u => u.Id == Customer).SingleOrDefault();
            Product ThisProduct = _context.product.Where(p => p.ProductId == Product).SingleOrDefault();
            if (Quantity > 0 && Quantity <= ThisProduct.Quantity)
            {
                 Order NewOrder = new Order { CreatedAt = DateTime.Now, User = ForUser, Quantity = Quantity, Product = ThisProduct};
                _context.Add(NewOrder);
                ThisProduct.Quantity = ThisProduct.Quantity - Quantity;
                _context.SaveChanges();
                return RedirectToAction("Orders");
            }
            ViewBag.errors = "We do not have that quantity in stock.";
            ViewBag.Users = _context.user.ToList();
            ViewBag.Products = _context.product.ToList();
            ViewBag.Orders = _context.order.ToList();
            return View("Order");
        }

        [HttpGet]
        [Route("Orders/Filter")]
        public IActionResult Filter(string Filter)
        {   
            List<Order> FilteredOrders = _context.order.OrderBy(o => Filter).ToList();
            ViewBag.Orders = FilteredOrders;
            ViewBag.Users = _context.user.ToList();
            ViewBag.Products = _context.product.ToList();
            return View("Order");
        }

        [HttpGet]
        [Route("Customers")]
        public IActionResult Customers()
        {   
            ViewBag.Users = _context.user.ToList();
            return View("Customer");
        }

        [HttpGet]
        [Route("Products/{page}")]
        public IActionResult Products(int page)
        {   
            List<Product> AllProducts = _context.product.ToList();
            if (AllProducts.Count - (page * 3) >= 0)
            {
                ViewBag.next = page + 1;
                ViewBag.Products = _context.product.Skip(3*(page-1)).Take(3).ToList();
            }
            ViewBag.Products = _context.product.Skip(3*(page-1)).Take(3).ToList();
            return View("Product");
        }

        [HttpPost]
        [Route("Products/makeproduct")]
        public IActionResult MakeProduct(string Name, string Description, int Quantity)
        { 
            List<Product> ExistingProd = _context.product.Where(p => p.Name == Name).ToList();
            if (ExistingProd.Count == 0)
            {
                Product NewProduct = new Product { CreatedAt = DateTime.Now, Name = Name, Quantity = Quantity, Description = Description};
                _context.Add(NewProduct);
                _context.SaveChanges();
                return RedirectToAction("Products", new {page = 1});
            }
            ViewBag.errors = "This product already exists in our inventory.";
            ViewBag.Products = _context.product.Take(15).ToList();
            return View("Product");
        }

        [HttpPost]
        [Route("Products/search")]
        public IActionResult Search(string Search)
        { 
            List<Product> SearchProd = _context.product.Where(u => u.Name.Contains(Search)).ToList();
            if (SearchProd.Count == 0)
            {
                ViewBag.Products = _context.product.Take(15).ToList();
                ViewBag.response = "No products match that search";
                return View("Product");
            }
            ViewBag.SearchProducts = SearchProd;
            return View("Product");
        }
    }
}