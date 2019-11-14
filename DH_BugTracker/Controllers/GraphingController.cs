using DH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH_BugTracker.Controllers
{
    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Graphing
        //public JsonResult Chart1Data()
        //{
        //    var myData = new List<MorrisBarData>();
        //    MorrisBarData data = null;
        //    foreach (var priority in db.TicketPriorities.ToList())
        //    {
        //        data.Label = priority.Name;
        //        data.Value = db.Tickets.Where(t => t.TicketPriority.Name == priority.Name).Count();
        //        myData.Add(data);
        //    }
        //    return Json(myData);
        //}
    }
}