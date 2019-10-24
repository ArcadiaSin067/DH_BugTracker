using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Display(Name = "Project Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        //nav section
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        //constructors
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }

    }
}