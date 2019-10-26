using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH_BugTracker.Models;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketNotifyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotify
        public ActionResult Index()
        {
            var ticketNotifies = db.TicketNotifies.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketNotifies.ToList());
        }

        // GET: TicketNotify/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotify ticketNotify = db.TicketNotifies.Find(id);
            if (ticketNotify == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotify);
        }

        // GET: TicketNotify/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketNotify/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,UnRead,TicketId,UserId")] TicketNotify ticketNotify)
        {
            if (ModelState.IsValid)
            {
                db.TicketNotifies.Add(ticketNotify);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotify.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotify.UserId);
            return View(ticketNotify);
        }

        // GET: TicketNotify/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotify ticketNotify = db.TicketNotifies.Find(id);
            if (ticketNotify == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotify.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotify.UserId);
            return View(ticketNotify);
        }

        // POST: TicketNotify/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,UnRead,TicketId,UserId")] TicketNotify ticketNotify)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotify).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotify.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotify.UserId);
            return View(ticketNotify);
        }

        // GET: TicketNotify/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotify ticketNotify = db.TicketNotifies.Find(id);
            if (ticketNotify == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotify);
        }

        // POST: TicketNotify/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotify ticketNotify = db.TicketNotifies.Find(id);
            db.TicketNotifies.Remove(ticketNotify);
            db.SaveChanges();
            return RedirectToAction("Index");
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
