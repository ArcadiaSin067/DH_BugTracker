using DH_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var ticketHasBeenUnassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var ticketHasBeenReassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId != null && oldTicket.AssignedToUserId != newTicket.AssignedToUserId;

            if (ticketHasBeenAssigned)
            {
                AddAssignmentNotification(oldTicket,newTicket);
            }
            else if (ticketHasBeenUnassigned)
            {
                AddUnassignmentNotification(oldTicket, newTicket);

            }
            else if (ticketHasBeenReassigned)
            {
                AddUnassignmentNotification(oldTicket, newTicket);
                AddAssignmentNotification(oldTicket, newTicket);
            }
        }


        private void AddAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotify
            {
                TicketId = newTicket.Id,
                UnRead = true,
                UserId = newTicket.AssignedToUserId,
                Body = $"You have been assigned to a Ticket, Id number {newTicket.Id}, on Project {newTicket.Project.Name}. The Ticket title is {newTicket.Title} and "
            };
            db.TicketNotifies.Add(notification);
            db.SaveChanges();
        }

        private void AddUnassignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotify
            {
                TicketId = newTicket.Id,
                UnRead = true,
                UserId = newTicket.AssignedToUserId,
                Body = $"You have been unassigned to a Ticket, Id number {newTicket.Id}, on Project {newTicket.Project.Name}. The Ticket title is {newTicket.Title} and "
            };
            db.TicketNotifies.Add(notification);
            db.SaveChanges();
        }

    }
}