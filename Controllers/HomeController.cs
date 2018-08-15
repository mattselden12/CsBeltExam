using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsBeltExam.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CsBeltExam.Controllers
{
    public class HomeController : Controller
    {
        private CsBeltExamContext _context;
		public HomeController(CsBeltExamContext context)
		{
			_context = context;

        }

        [HttpGet("")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [HttpGet("addnew")]
        public IActionResult AddNew()
        {
            return View("AddNew");
        }

        [HttpGet("objectinfo")]
        public IActionResult ObjectInfo()
        {
            return View("ObjectInfo");
        }

        // [HttpPost]
        // [Route("registerprocess")]
        // public IActionResult RegisterProcess(UserValidator newuser)
        // {
        //     if(ModelState.IsValid)
        //     {
        //         User DBUser = _context.users.SingleOrDefault(u=>u.Email == newuser.Email);
        //         if(DBUser != null)
        //         {
        //             ViewBag.Error = "Email already exists in Database";
        //             return View("Login");
        //         }
        //         PasswordHasher<UserValidator> Hasher = new PasswordHasher<UserValidator>();
        //         newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
        //         User this_user = new User
        //         {
        //             FirstName = newuser.FirstName,
        //             LastName = newuser.LastName,
        //             Email = newuser.Email,
        //             Password = newuser.Password,
        //         };
        //         _context.Add(this_user);
        //         _context.SaveChanges();
        //         HttpContext.Session.SetInt32("UserId", this_user.UserId);
        //         HttpContext.Session.SetString("UserFirstName", this_user.FirstName);
        //         return RedirectToAction("Dashboard");
        //     }
        //     else{
        //         return View("Login");
        //     }
        // }

        // [HttpPost]
        // [Route("loginprocess")]
        // public IActionResult LoginProcess(string LEmail, string LPassword)
        // {
        //     User myUser = _context.users.SingleOrDefault(u => u.Email == LEmail);
        //     if(myUser != null && LPassword != null)
        //     {
        //         var Hasher = new PasswordHasher<User>();
        //         if(0 != Hasher.VerifyHashedPassword(myUser, myUser.Password, LPassword))
        //         {
        //             HttpContext.Session.SetInt32("UserId", myUser.UserId);
        //             HttpContext.Session.SetString("UserFirstName", myUser.FirstName);
        //             return RedirectToAction("Dashboard");
        //         }
        //         else
        //         {
        //             ViewBag.BadPass = "Password Incorrect.";
        //             return View("Login");
        //         }
        //     }
        //     else{
        //         if(myUser == null)
        //         {
        //             ViewBag.NoUser = "Could not locate user with that email.";
        //         }
        //         if(LPassword == null)
        //         {
        //             ViewBag.PassNull = "You must enter a password.";
        //         }
        //         return View("Login");
        //     }
        // }

        // [HttpGet]
        // [Route("logout")]
        // public IActionResult Logout()
        // {
        //     HttpContext.Session.Clear();
        //     return RedirectToAction("Login");
        // }










        // [HttpPost("addproduct")]
        // public IActionResult AddProduct(Product this_product)
        // {
        //     Product new_product = new Product
        //     {
        //         Name = this_product.Name,
        //         Description = this_product.Description,
        //         Price = this_product.Price
        //     };
        //     _context.products.Add(new_product);
        //     _context.SaveChanges();
        //     return Redirect("~/products/"+new_product.ProductId);
        // }













        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}











namespace CsBeltExam
{
	public static class SessionExtensions
	{
		public static void SetObjectAsJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T GetObjectFromJson<T>(this ISession session, string key)
		{
			string value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
