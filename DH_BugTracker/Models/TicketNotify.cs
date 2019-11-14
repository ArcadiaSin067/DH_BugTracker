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
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }

        public int TicketId { get; set; }
        public string ReceiverId { get; set; }
        public string UserId { get; set; }

        //nav section
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}