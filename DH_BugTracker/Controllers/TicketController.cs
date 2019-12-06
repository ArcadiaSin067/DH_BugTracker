using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH_BugTracker.Models;
using DH_BugTracker.Helpers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper tktHelper = new TicketHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHistoryHelper tktHistHelp = new TicketHistoryHelper();
        private ProjectHelper projectHelp = new ProjectHelper();
        private NotificationHelper notify = new NotificationHelper();

        // GET: Ticket
        public ActionResult Index()
        {
            return View(tktHelper.ListMyTickets());
        }

        // GET: Ticket/Details
        public ActionResult Details(int? id)
        {
            var ticket = db.Tickets.Find(id);
            ViewBag.DevId = (ticket.AssignedToUserId != null ? ticket.AssignedToUser.FullName : "Unassigned");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Ticket/Create
        [Authorize(Roles ="Submitter, Demo_Submitter")]
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.ProjectName = db.Projects.Find(id).Name.ToString();
                ViewBag.HasId = true;
                ViewBag.ProjectId = (int)id;
            }
            else
            {
                ViewBag.ProjectId = new SelectList(projectHelp.ListMyProjects(), "Id", "Name");
                ViewBag.HasId = false;
            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = tktHelper.SetDefaultTicketStatus();
                
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(projectHelp.ListMyProjects(), "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            if (roleHelper.ListUserRoles(userId).FirstOrDefault().Contains("Developer"))
            {
                if (db.Tickets.Find(id).AssignedToUserId != userId)
                {
                    //used for sweetalert2 for no messing with others tickets
                    TempData["Message"] = "Sorry you cannot edit tickets you are not assigned to.";
                    return RedirectToAction("Index");
                }
            }
            if (roleHelper.ListUserRoles(userId).FirstOrDefault().Contains("Submitter"))
            {
                if (db.Tickets.Find(id).OwnerUserId != userId)
                {
                    //used for sweetalert2 for no messing with others tickets
                    TempData["Message"] = "Sorry you cannot edit tickets you did not create.";
                    return RedirectToAction("Index");
                }
            }
            var projectId = db.Tickets.Find((int)id).ProjectId;
            var projectUsers = projectHelp.ListUsersOnProjectInRole(projectId, "Developer")
                                            .Union(projectHelp.ListUsersOnProjectInRole(projectId, "Demo_Developer"));
            var DevOnProjectList = new List<ApplicationUser>();
            foreach (var dudeId in projectUsers)
            {
                DevOnProjectList.Add(db.Users.Find(dudeId));
            }
            ViewBag.AssignedToUserId = new SelectList(DevOnProjectList, "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.DevId = (ticket.AssignedToUserId != null ? ticket.AssignedToUser.FullName : "Unassigned" );
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                EmailHelper help = new EmailHelper();
                var callbackUrl = Url.Action("Details", "Ticket", new { id = ticket.Id },
                                    protocol: Request.Url.Scheme);

                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                tktHistHelp.RecordHistoryChanges(oldTicket, newTicket);
                await help.AssignDevToTicket_Email(oldTicket, newTicket, callbackUrl);
                notify.ManageNotifications(oldTicket, newTicket);
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer").Union(roleHelper.UsersInRole("Demo_Developer")), "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.DevId = (ticket.AssignedToUserId != null ? ticket.AssignedToUser.FullName : "Unassigned");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
