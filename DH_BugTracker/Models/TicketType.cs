﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //nav section
        public virtual ICollection<Ticket> Tickets { get; set; }

        //constructors
        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }

    }
}