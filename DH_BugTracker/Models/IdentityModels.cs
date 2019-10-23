using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DH_BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="First Name]")]
        [StringLength(50, MinimumLength = 10, ErrorMessage ="Must have minimum length of 10 characters and maximum length of 50.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name]")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Must have minimum length of 10 characters and maximum length of 50.")]
        public string LastName { get; set; }

        [Display(Name = "Display Name]")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Must have minimum length of 10 characters and maximum length of 50.")]
        public string DisplayName { get; set; }

        //navigation section
        public virtual ICollection<TicketAttach> TicketAttaches { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotify> TicketNotifies { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        //constructor
        public ApplicationUser()
        {
            TicketAttaches = new HashSet<TicketAttach>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifies = new HashSet<TicketNotify>();
            Projects = new HashSet<Project>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}