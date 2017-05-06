using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Ecomm.Models;
using System.Linq;
using System;
using System.Reflection;

namespace Ecomm.Controllers
{
    public class ProductController : Controller
    {   
        private EcommContext _context;
        public ProductController(
            EcommContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Products/makeproduct")]
        [Consumes("application/json")]
        public IActionResult MakeProduct([FromBody] ProductViewModel model)
        { 
            if (ModelState.IsValid)
            { 
                List<Product> ExistingProd = _context.product.Where(p => p.Name == model.Name).ToList();
                if (ExistingProd.Count == 0)
                {
                    Product NewProduct = new Product { CreatedAt = DateTime.Now, Name = model.Name, Quantity = model.Quantity, Description = model.Description};
                    _context.Add(NewProduct);
                    _context.SaveChanges();
                    return StatusCode(201);
                }
            }
            return StatusCode(400);
        }

        [HttpPost]
        [Route("Products/updateinv")]
        [Consumes("application/json")]
        public IActionResult UpdateInv([FromBody] ProductViewModel model)
        { 
            if (ModelState.IsValid)
            {
                Product ExistingProd = _context.product.Where(p => p.ProductId == model.ProductId).SingleOrDefault();
                if (ExistingProd != null)
                {
                    ExistingProd.Quantity += model.Quantity;
                    _context.SaveChanges();
                    return StatusCode(201);
                }
            }
            return StatusCode(400);
        }

        [HttpGet]
        [Route("Products/search")]
        public IActionResult Search([FromQuery] string SearchCriteria, [FromQuery] string Filter, [FromQuery] int PageNumber = 1, [FromQuery] int ItemsPerPage = 5)
        { 
            List<Product> SearchProd = new List<Product>();
            if (Filter != null)
            {
                SearchProd = _context.product.OrderBy(p => p.GetType().GetProperty(Filter)).Where(p => p.Name.ToLower().Contains(SearchCriteria.ToLower()) || p.Description.ToLower().Contains(SearchCriteria.ToLower())).Skip(ItemsPerPage * (PageNumber-1)).Take(ItemsPerPage).ToList();
                //TODO: filter bug
            }
            else 
            {
               SearchProd = _context.product.Where(p => p.Name.ToLower().Contains(SearchCriteria.ToLower()) || p.Description.ToLower().Contains(SearchCriteria.ToLower())).Skip(ItemsPerPage * (PageNumber-1)).Take(ItemsPerPage).ToList();
            }
            if (SearchProd.Count == 0)
            {
                string response = "No products match that search";
                return Json(response);
            }
            return Json(SearchProd);
        }
    }
}
