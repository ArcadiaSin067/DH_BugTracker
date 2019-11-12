using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public int SetDefaultTicketStatus()
        {
            return db.TicketStatuses.FirstOrDefault(ts => ts.Name == "New").Id;
        }

        public int SetDefaultTicketPriority()
        {
            return db.TicketPriorities.FirstOrDefault(tp => tp.Name == "None").Id;
        }

        public List<Ticket> ListMyTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myTickets = new List<Ticket>();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "Demo_Admin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "Project Manager":
                case "Demo_Project Manager":
                    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "Demo_Developer":
                    myTickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId));
                    break;
                case "Submitter":
                case "Demo_Submitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.OwnerUserId == userId));
                    break;
            }
            return myTickets;
        }

        public string ListMyTicketAccess()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var viewTickets = "";
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                case "Demo_Admin":
                    viewTickets = "in the database.";
                    break;
                case "Project Manager":
                case "Demo_Project Manager":
                    viewTickets = "for your Projects.";
                    break;
                case "Developer":
                case "Demo_Developer":
                    viewTickets = "for the Projects you are assigned to.";
                    break;
                case "Submitter":
                case "Demo_Submitter":
                    viewTickets = "submitted.";
                    break;
            }
            return viewTickets;
        }
    }
}