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
    public class AccountController : Controller
    {   
        private EcommContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(
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
        [Route("Account/Login")]
        public IActionResult Login()
        {   
            User CurrentUser = GetCurrentUser();
            if (CurrentUser != null)
            {
                return RedirectToAction("Index", "Ecommerce");
            }
            return View("LoginPage");
        }

        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> ToLogin(string Email, string Password)
        {   
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(Email, Password, isPersistent: false, lockoutOnFailure: false);
            if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Ecommerce");
                }
            ViewBag.error = "Your signin information is invalid.";
            return View("LoginPage");
        }

        [HttpGet]
        [Route("Account/Register")]
        public IActionResult Register()
        {   
            User CurrentUser = GetCurrentUser();
            if (CurrentUser != null)
            {   
                return RedirectToAction("Index", "Ecommerce");
            }
            @ViewBag.CurrentUser = null;
            return View("RegisterPage");
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User NewUser = new User { FirstName = model.FirstName, LastName = model.LastName, CreatedAt = DateTime.Now, Email = model.Email, UserName = model.Email };
                IdentityResult result = await _userManager.CreateAsync(NewUser, model.Password);
                if(result.Succeeded)
                {   
                    await _signInManager.SignInAsync(NewUser, isPersistent: false);
                    return RedirectToAction("Index", "Ecommerce");
                }
                foreach( var error in result.Errors )
                {
                    ModelState.AddModelError(string.Empty, error.Description);
    
                }
            }
            return View("RegisterPage", model);
        }

        [HttpGet]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {   
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("Account/Delete/{id}")]
        public async Task<IActionResult>  Delete(string id)
        {   
            User RetrievedUser = _context.user.SingleOrDefault(u => u.Id == id);
            await _userManager.DeleteAsync(RetrievedUser);
            return RedirectToAction("Customers", "Ecommerce");
        }
    }
}

