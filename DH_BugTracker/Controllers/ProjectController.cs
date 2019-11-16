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
using Microsoft.AspNet.Identity;

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
        [Authorize(Roles = "Admin,Demo_Admin,Project Manager,Demo_Project Manager")]
        public ActionResult ManageUsers(int Id)
        {
            Project project = new Project { Users = projectHelper.UsersOnProject(Id).ToList() };
            var admId = projectHelper.ListUsersOnProjectInRole(Id, "Admin").Union(projectHelper.ListUsersOnProjectInRole(Id,"Demo_Admin")).FirstOrDefault();
            var pmId = projectHelper.ListUsersOnProjectInRole(Id, "Project Manager").Union(projectHelper.ListUsersOnProjectInRole(Id, "Demo_Project Manager")).FirstOrDefault();
            var devId = projectHelper.ListUsersOnProjectInRole(Id, "Developer").Union(projectHelper.ListUsersOnProjectInRole(Id, "Demo_Developer"));
            var subId = projectHelper.ListUsersOnProjectInRole(Id, "Submitter").Union(projectHelper.ListUsersOnProjectInRole(Id, "Demo_Submitter"));

            ViewBag.ProjectId = Id;
            ViewBag.ProjectName = db.Projects.Find(Id).Name;
            ViewBag.AdminId = new SelectList(roleHelper.UsersInRole("Admin").Union(roleHelper.UsersInRole("Demo_Admin")), "Id", "FullName", admId);
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager").Union(roleHelper.UsersInRole("Demo_Project Manager")), "Id", "FullName", pmId);
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer").Union(roleHelper.UsersInRole("Demo_Developer")), "Id", "FullName", devId);
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter").Union(roleHelper.UsersInRole("Demo_Submitter")), "Id", "FullName", subId);

            return View(project);
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

        // GET: Project/Index
        public ActionResult Index()
        {
            return View(projectHelper.ListMyProjects());
        }

        // GET: Project/Details
        public ActionResult Details(int? id)
        {
            Project project = new Project {
                Users = projectHelper.UsersOnProject((int)id).ToList(),
                Tickets = db.Projects.Find((int)id).Tickets.ToList(),
                Description = db.Projects.Find((int)id).Description,
                Name = db.Projects.Find((int)id).Name,
                Id = (int)id
            };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        [Authorize(Roles = "Admin,Demo_Admin,Project Manager,Demo_Project Manager")]
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
                if (User.IsInRole("Project Manager") || User.IsInRole("Demo_Project Manager"))
                {
                    var userId = User.Identity.GetUserId();
                    projectHelper.AddUserToProject(userId, project.Id);
                }
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Project/Edit
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

        // POST: Project/Edit
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

        // GET: Project/Delete
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

        // POST: Project/Delete
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
