using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusFit.Models;
using MusFit.Utilities;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MusFit.Controllers
{
    public class ManageController : Controller
    {
        private MusFitContext _context;

        public ManageController(MusFitContext context)
        {
            _context = context;
        }

        [Authentication]
        public IActionResult Index(string name, int eId)
        {
            TempData["name"] = name;
            TempData["eId"] = eId;
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Manager") == null && HttpContext.Session.GetString("Coach") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private byte[] StringToSHA256(string str)
        {
            StringBuilder builder = new StringBuilder();
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(str);
            byte[] crypto = sha256.ComputeHash(source);
            return crypto;
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            byte[] password_SHA256 = StringToSHA256(password);

            if (HttpContext.Session.GetString("Manager") == null && HttpContext.Session.GetString("Coach") == null)
            {
                if (ModelState.IsValid)
                {
                    var manager = _context.Employees.Where(e => e.EAccount.Equals(username) && e.EPassword.Equals(password_SHA256) && e.EIsCoach == false).FirstOrDefault();
                    var coach = _context.Employees.Where(e => e.EAccount.Equals(username) && e.EPassword.Equals(password_SHA256) && e.EIsCoach == true).FirstOrDefault();
                    if (manager != null)
                    {
                        HttpContext.Session.SetString("Manager", manager.EAccount.ToString());
                        return RedirectToAction("Index", new { name = manager.EName, eId = manager.EId });
                    }
                    else if (coach != null)
                    {
                        HttpContext.Session.SetString("Coach", coach.EAccount.ToString());
                        return RedirectToAction("Index", new { name = coach.EName, eId = coach.EId });
                    }
                    else
                    {
                        ViewBag.Message = "帳號或密碼輸入錯誤!!";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login");
        }

        [Authentication]
        public IActionResult CoachScheduleQuery()
        {
            return View();
        }
        [Authentication]
        public IActionResult CoachSchedule(int id)
        {
            ViewBag.eId = id;
            return View();
        }

        [Authentication]
        public IActionResult VisitorInfo()
        {
            return View();
        }

        [Authentication]
        public IActionResult InBodyRecords(long? id = 1)
        {
            ViewBag.Id = id;
            return View();
        }
        [Authentication]
        public IActionResult InBodyPersonalRecords(long? id)
        {
            ViewBag.Id = id;
            return View();
        }

        [Authentication]
        public IActionResult InBodyRecordQuery()
        {
            return View();
        }


        public IActionResult News()
        {
            var myquery2 = (from myemployee1 in _context.Employees select myemployee1).ToList();
            var myquery = (from Mynews in _context.News select Mynews.NId).ToList();
            ViewBag.querydata = myquery2;
            ViewBag.Myquerydata = myquery;





            var viewModel = _context.News.ToList();

            return View(viewModel);
        }
    }
}
