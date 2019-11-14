using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var ticketHasBeenUnassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var ticketHasBeenReassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId != null
                && oldTicket.AssignedToUserId != newTicket.AssignedToUserId;

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
                IsRead = false,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                ReceiverId = newTicket.AssignedToUserId,
                Created = DateTime.Now,
                Body = $"You have been assigned to Ticket Id# '{newTicket.Id}', on Project: " +
                $"'{newTicket.Project.Name}'.\nThe title is '{newTicket.Title}', and it was created on " +
                $"'{newTicket.Created}'\nwith a priority of '{newTicket.TicketPriority.Name}'."
            };
            db.TicketNotifies.Add(notification);
            db.SaveChanges();
        }

        private void AddUnassignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotify
            {
                TicketId = newTicket.Id,
                IsRead = false,
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                ReceiverId = oldTicket.AssignedToUserId,
                Created = DateTime.Now,
                Body = $"You have been unassigned from the Ticket titled '{newTicket.Title}'\n" +
                $"with Id# '{newTicket.Id}' for Project: '{newTicket.Project.Name}'."
            };
            db.TicketNotifies.Add(notification);
            db.SaveChanges();
        }

        public static List<TicketNotify> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var notifications = db.TicketNotifies.Include("User").Where(t => t.ReceiverId == currentUserId && !t.IsRead).ToList();
            return notifications;
        }
    }
}