﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH_BugTracker.Helpers;
using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TicketAttachController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttach
        public ActionResult Index()
        {
            var ticketAttaches = db.TicketAttaches.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttaches.ToList());
        }

        // GET: TicketAttach/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttaches.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttach);
        }

        // GET: TicketAttach/Create
        public ActionResult Create(int id)
        {
            var attachment = new TicketAttach { TicketId = id };
            return View(attachment);
        }

        // POST: TicketAttach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId")] TicketAttach ticketAttach, HttpPostedFileBase file, string attachDesc)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (FileUploadValidator.IsWebFriendlyImage(file) || FileUploadValidator.IsWebFriendlyFile(file))
                    {
                        ticketAttach.Description = attachDesc;
                        var fileName = Path.GetFileName(file.FileName);
                        var justFileName = Path.GetFileNameWithoutExtension(fileName);
                        justFileName = StringUtilities.URLFriendly(justFileName);
                        fileName = $"{justFileName}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                        ticketAttach.FilePath = "/Uploads/" + fileName;
                        ticketAttach.Created = DateTime.Now;
                        ticketAttach.UserId = User.Identity.GetUserId();
                        db.TicketAttaches.Add(ticketAttach);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Details", "Ticket", new { Id = ticketAttach.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // GET: TicketAttach/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttaches.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // POST: TicketAttach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FilePath,Description,Created,TicketId,UserId")] TicketAttach ticketAttach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttach.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttach.UserId);
            return View(ticketAttach);
        }

        // GET: TicketAttach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttach ticketAttach = db.TicketAttaches.Find(id);
            if (ticketAttach == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttach);
        }

        // POST: TicketAttach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttach ticketAttach = db.TicketAttaches.Find(id);
            db.TicketAttaches.Remove(ticketAttach);
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
