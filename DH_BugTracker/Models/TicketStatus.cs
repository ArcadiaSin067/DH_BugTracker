using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }

        [Display(Name = "Ticket Status")]
        public string Name { get; set; }
        public string Description { get; set; }

        //nav section
        public virtual ICollection<Ticket> Tickets { get; set; }

        //constructors
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }

    }
}