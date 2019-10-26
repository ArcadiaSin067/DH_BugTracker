using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {

        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Demo_Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}