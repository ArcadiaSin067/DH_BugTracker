using DH_BugTracker.Helpers;
using DH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH_BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Demo_Admin")]
    [RequireHttps]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper RoleHelper = new UserRolesHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",
                    RoleName = RoleHelper.ListUserRoles(user.Id).FirstOrDefault()
                });
            }

            return View(users);
        }

        // POST: Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
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
}