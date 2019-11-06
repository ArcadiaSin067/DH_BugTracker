using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH_BugTracker.Helpers;
using DH_BugTracker.Models;

namespace DH_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        //
        // GET: /Project/ManageUsers
        [Authorize(Roles ="Admin, Demo_Admin, Project Manager, Demo_Project Manager")]
        public ActionResult ManageUsers(int Id)
        {
            var admId = projectHelper.ListUsersOnProjectInRole(Id, "Admin").FirstOrDefault();
            var pmId = projectHelper.ListUsersOnProjectInRole(Id, "Project Manager").FirstOrDefault();
            var devId = projectHelper.ListUsersOnProjectInRole(Id, "Developer");
            var subId = projectHelper.ListUsersOnProjectInRole(Id, "Submitter");

            ViewBag.ProjectId = Id;
            ViewBag.ProjectName = db.Projects.Find(Id).Name;
            ViewBag.AdminId = new SelectList(roleHelper.UsersInRole("Admin").Union(roleHelper.UsersInRole("Demo_Admin")), "Id", "Email", admId);
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager").Union(roleHelper.UsersInRole("Demo_Project Manager")), "Id", "Email", pmId);
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer").Union(roleHelper.UsersInRole("Demo_Developer")), "Id", "Email", devId);
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter").Union(roleHelper.UsersInRole("Demo_Submitter")), "Id", "Email", subId);

            var firstProject = db.Projects.Min(p => p.Id);
            return View(firstProject);
        }

        //
        // POST: /Project/ManageUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUsers(int projectId, string adminId, string projectManagerId, List<string> developers, List<string> submitters)
        {
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            if (!string.IsNullOrEmpty(adminId))
            {
                projectHelper.AddUserToProject(adminId, projectId);
            }
            if (!string.IsNullOrEmpty(projectManagerId))
            {
                projectHelper.AddUserToProject(projectManagerId, projectId);
            }
            if (developers != null)
            {
                foreach (var developerId in developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }
            if (submitters != null)
            {
                foreach (var submitterId in submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }

            return RedirectToAction("ManageUsers", new { id = projectId });
        }


        // GET: Project
        public ActionResult Index()
        {
            return View(projectHelper.ListMyProjects());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        [Authorize(Roles = "Admin, Demo_Admin, Project Manager, Demo_Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Project/Edit/5
        [Authorize(Roles = "Admin, Demo_Admin, Project Manager, Demo_Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Project/Delete/5
        [Authorize(Roles = "Admin, Demo_Admin, Project Manager, Demo_Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
