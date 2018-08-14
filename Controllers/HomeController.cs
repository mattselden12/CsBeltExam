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
