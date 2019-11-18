using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public static class IdentityExtensions
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetAvatarPath(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue("AvatarPath");
            }
            return null;
            //var user = db.Users.Find(identity.GetUserId());
            //return user.AvatarPath;
        }
        public static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue("DisplayName");
            }
            return null;
            //var user = db.Users.Find(identity.GetUserId());
            //return user.DisplayName;

        }
    }
}