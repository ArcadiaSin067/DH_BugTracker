using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Models
{
    public class TicketNotify
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public bool UnRead { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        //nav section
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}