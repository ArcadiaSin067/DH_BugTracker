using DH_BugTracker.Helpers;
using DH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelp = new UserRolesHelper();

        // GET: Graphing
        public JsonResult Chart1Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var priority in db.TicketPriorities.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.Name;
                data.value = db.Tickets.Where(t => t.TicketPriority.Name == priority.Name).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult Chart2Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var status in db.TicketStatuses.ToList())
            {
                data = new MorrisBarData();
                data.label = status.Name;
                data.value = db.Tickets.Where(t => t.TicketStatus.Name == status.Name).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult Chart3Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var types in db.TicketTypes.ToList())
            {
                data = new MorrisBarData();
                data.label = types.Name;
                data.value = db.Tickets.Where(t => t.TicketType.Name == types.Name).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult Chart4Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var user in roleHelp.UsersInRole("Developer").ToList().Union(roleHelp.UsersInRole("Demo_Developer").ToList()))
            {
                data = new MorrisBarData();
                data.label = user.FullName;
                data.value = db.Tickets.Where(t => t.AssignedToUserId == user.Id).Count();
                myData.Add(data);
            }
            return Json(myData);
        }
    }
}