using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DH_BugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name="First Name")]
        [StringLength(35, MinimumLength = 3, ErrorMessage ="Must have minimum length of 3 characters and maximum length of 50.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "Must have minimum length of 3 characters and maximum length of 50.")]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Must have minimum length of 3 characters and maximum length of 50.")]

        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }


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
            userIdentity.AddClaim(new Claim("AvatarPath", this.AvatarPath));
            userIdentity.AddClaim(new Claim("DisplayName", this.DisplayName));

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

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttach> TicketAttaches { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketNotify> TicketNotifies { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

    }
}