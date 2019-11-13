﻿using DH_BugTracker.Models;
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
                UnRead = true,
                UserId = newTicket.AssignedToUserId,
                Body = $"You have been assigned to Ticket Id number {newTicket.Id}, on Project: " +
                $"{newTicket.Project.Name}. The Ticket title is {newTicket.Title}, and it was created on " +
                $"{newTicket.Created}, with a priority of {newTicket.TicketPriority.Name}."
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
                Body = $"You have been unassigned from Ticket Id number {newTicket.Id}, on Project: " +
                $"{newTicket.Project.Name}. The Ticket title is {newTicket.Title}, and it was created on " +
                $"{newTicket.Created}, with a priority of {newTicket.TicketPriority.Name}."
            };
            db.TicketNotifies.Add(notification);
            db.SaveChanges();
        }

        public static List<TicketNotify> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifies.Include("User").Where(t => t.ReceiverId == currentUserId && t.UnRead).ToList();
        }
    }
}