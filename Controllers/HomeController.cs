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
            ViewBag.NotLoggedIn = TempData["NotLoggedIn"];
            return View("Login");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                List<Auction> AllAuctions = _context.auctions.Include(a => a.Creator).Include(a => a.TopBidder).OrderBy(a => a.EndDate).ToList();
                foreach(var auct in AllAuctions){
                    if(auct.EndDate < DateTime.Today)
                    {
                        auct.Creator.Balance += auct.TopBid;
                        auct.TopBidder.Balance -= auct.TopBid;
                        _context.auctions.Remove(auct);
                    }
                }
                _context.SaveChanges();
                User this_user = _context.users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.CurrentBalance = this_user.Balance;
                ViewBag.CurFirstName = this_user.FirstName;
                return View("Dashboard", AllAuctions);
            }
            else
            {
                TempData["NotLoggedIn"] = "You must be logged in to view Auctions";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("addnew")]
        public IActionResult AddNew()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                return View("AddNew");
            }
            else
            {
                TempData["NotLoggedIn"] = "You must be logged in to view Auctions";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("objectinfo/{auctionid}")]
        public IActionResult ObjectInfo(int auctionid)
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                Auction this_auction = _context.auctions.Include(a => a.Creator).Include(a => a.TopBidder).SingleOrDefault(a => a.AuctionId == auctionid);
                ViewBag.Balance = TempData["Balance"];
                ViewBag.TopBid = TempData["TopBid"];
                return View("ObjectInfo", this_auction);
            }
            else
            {
                TempData["NotLoggedIn"] = "You must be logged in to view Auctions";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("registerprocess")]
        public IActionResult RegisterProcess(UserValidator newuser)
        {
            if(ModelState.IsValid)
            {
                User DBUser = _context.users.SingleOrDefault(u=>u.Username == newuser.Username);
                if(DBUser != null)
                {
                    ViewBag.Error = "Username already exists in Database";
                    return View("Login");
                }
                PasswordHasher<UserValidator> Hasher = new PasswordHasher<UserValidator>();
                newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                User this_user = new User
                {
                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    Username = newuser.Username,
                    Password = newuser.Password,
                    Balance = 1000.00,
                    Available = 1000.00
                };
                _context.Add(this_user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", this_user.UserId);
                HttpContext.Session.SetString("UserFirstName", this_user.FirstName);
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Login");
            }
        }

        [HttpPost]
        [Route("loginprocess")]
        public IActionResult LoginProcess(string LUsername, string LPassword)
        {
            User myUser = _context.users.SingleOrDefault(u => u.Username == LUsername);
            if(myUser != null && LPassword != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(myUser, myUser.Password, LPassword))
                {
                    HttpContext.Session.SetInt32("UserId", myUser.UserId);
                    HttpContext.Session.SetString("UserFirstName", myUser.FirstName);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.BadPass = "Password Incorrect.";
                    return View("Login");
                }
            }
            else{
                if(myUser == null)
                {
                    ViewBag.NoUser = "Could not locate user with that email.";
                }
                if(LPassword == null)
                {
                    ViewBag.PassNull = "You must enter a password.";
                }
                return View("Login");
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Route("addauction")]
        public IActionResult AddAuction(Auction new_auction)
        {
            if(ModelState.IsValid)
            {
                if(new_auction.EndDate < DateTime.Today || new_auction.TopBid < 0)
                {
                    if(new_auction.EndDate < DateTime.Today)
                    {
                        ModelState.AddModelError("EndDate", "End Date must be in the future.");
                    }
                    if(new_auction.TopBid < 0)
                    {
                        ModelState.AddModelError("TopBid", "Starting bid must be greater than $0");
                    }
                    return View("AddNew");
                }
                else
                {
                    Auction this_auction = new Auction
                    {
                        ProductName = new_auction.ProductName,
                        Description = new_auction.Description,
                        EndDate = new_auction.EndDate,
                        TopBid = new_auction.TopBid,
                        CreatorId = (int) HttpContext.Session.GetInt32("UserId"),
                        TopBidderId = (int) HttpContext.Session.GetInt32("UserId")

                    };
                    _context.Add(this_auction);
                    _context.SaveChanges();
                    return Redirect("~/objectinfo/"+this_auction.AuctionId);
                }
            }
            else
            {
                if(new_auction.EndDate < DateTime.Today)
                {
                    ModelState.AddModelError("EndDate", "End Date must be in the future.");
                }
                if(new_auction.TopBid <= 0)
                {
                    ModelState.AddModelError("TopBid", "Starting bid must be greater than $0");
                }
                return View("AddNew");
            }
        }

        [HttpPost("placebid")]
        public IActionResult PlaceBid(Double NewBid, int auctionid)
        {
            User this_user = _context.users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            Auction this_auction = _context.auctions.Include(a => a.Creator).Include(a => a.TopBidder).SingleOrDefault(a => a.AuctionId == auctionid);
            if(NewBid > this_user.Available || NewBid <= this_auction.TopBid)
            {
                if(NewBid > this_user.Available)
                {
                    TempData["Balance"] = "You do not have enough available money to place that bid.";
                }
                if(NewBid <= this_auction.TopBid)
                {
                    TempData["TopBid"] = "Your bid must be greater than the Current Highest Bid.";
                }
                return Redirect("~/objectinfo/"+this_auction.AuctionId);
            }
            else{
                if(this_auction.TopBidder.UserId != this_auction.Creator.UserId)
                {
                    this_auction.TopBidder.Available += this_auction.TopBid;
                }
                this_auction.TopBid = NewBid;
                this_auction.TopBidderId = this_user.UserId;
                this_user.Available -= NewBid;
                _context.SaveChanges();
                return Redirect("~/objectinfo/"+this_auction.AuctionId);
            }
        }

        [HttpPost("deleteauction")]
        public IActionResult DeleteAuction(int auctionid)
        {
            Auction this_auction = _context.auctions.SingleOrDefault(a => a.AuctionId == auctionid);
            _context.auctions.Remove(this_auction);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
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
