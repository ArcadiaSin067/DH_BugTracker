using DH_BugTracker.Helpers;
using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DH_BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Demo_Admin, Project Manager, Demo_Project Manager")]
    [RequireHttps]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper RoleHelper = new UserRolesHelper();
        private ProjectHelper projectsHelper = new ProjectHelper();

        //
        // GET: /Admin/ManageRoles
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "FullName");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    RoleName = RoleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    Email = user.Email
                });
            }
            return View(users);
        }

        // POST: /Admin/ManageRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            if (User.IsInRole("Demo_Admin"))
            {
                return RedirectToAction("ManageRoles", "Admin");
            }
            else
            {
                foreach (var userId in userIds)
                {
                    var userRole = RoleHelper.ListUserRoles(userId).FirstOrDefault();
                    if(userRole != null)
                    {
                        RoleHelper.RemoveUserFromRole(userId, userRole);
                    }
                }
                if (!string.IsNullOrEmpty(role))
                {
                    foreach (var userId in userIds)
                    {
                        RoleHelper.AddUserToRole(userId, role);
                    }
                }
                return RedirectToAction("ManageRoles", "Admin");
            }
        }

        //
        // GET: /Admin/ManageProjects
        public ActionResult ManageProjects()
        {
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");
            ViewBag.Developers = new MultiSelectList(RoleHelper.UsersInRole("Developer").Union(RoleHelper.UsersInRole("Demo_Developer")), "Id", "FullName");
            ViewBag.Submitters = new MultiSelectList(RoleHelper.UsersInRole("Submitter").Union(RoleHelper.UsersInRole("Demo_Submitter")), "Id", "FullName");

            if (User.IsInRole("Admin") || User.IsInRole("Demo_Admin"))
            {
                ViewBag.AdminId = new SelectList(RoleHelper.UsersInRole("Admin").Union(RoleHelper.UsersInRole("Demo_Admin")), "Id", "FullName");
                ViewBag.ProjectManagerId = new SelectList(RoleHelper.UsersInRole("Project Manager").Union(RoleHelper.UsersInRole("Demo_Project Manager"))
                    , "Id", "FullName");
            }

            var myData = new List<ManageProjectsViewModel>();
            ManageProjectsViewModel userVM = null;

            foreach (var user in db.Users.ToList())
            {
                userVM = new ManageProjectsViewModel
                {
                    Email = user.Email,
                    Role = RoleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    Name = $"{user.FirstName} {user.LastName}",
                    ProjectNames = projectsHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                };
                if(userVM.ProjectNames.Count() == 0) {userVM.ProjectNames.Add("N/A");}

                myData.Add(userVM);
            }
            return View(myData);
        }

        //
        // POST: /Admin/ManageProjects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(List<int> projects, string adminId, string projectManagerId, List<string> developers, List<string> submitters)
        {
            if (projects != null)
            {
                foreach (var projectId in projects)
                {
                    foreach (var user in projectsHelper.UsersOnProject(projectId).ToList())
                    {
                        projectsHelper.RemoveUserFromProject(user.Id, projectId);
                    }

                    if (!string.IsNullOrEmpty(adminId))
                    {
                        projectsHelper.AddUserToProject(adminId, projectId);
                    }

                    if (!string.IsNullOrEmpty(projectManagerId))
                    {
                        projectsHelper.AddUserToProject(projectManagerId, projectId);
                    }

                    if (developers != null)
                    {
                        foreach (var developerId in developers)
                        {
                            projectsHelper.AddUserToProject(developerId, projectId);
                        }
                    }

                    if (submitters != null)
                    {
                        foreach (var submitterId in submitters)
                        {
                            projectsHelper.AddUserToProject(submitterId, projectId);
                        }
                    }
                }
            }
            return RedirectToAction("ManageProjects");
        }
    }
}