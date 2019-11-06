using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void RecordHistoryChanges(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "TicketPriorityId",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "TicketStatusId",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "TicketTypeId",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
        }
    }
}