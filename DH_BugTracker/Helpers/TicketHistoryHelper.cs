using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void RecordHistoryChanges(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.AssignedToUser == null ? "Unassigned" : oldTicket.AssignedToUser.FullName,
                    NewValue = newTicket.AssignedToUser == null ? "UnAssigned" : newTicket.AssignedToUser.FullName,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "AssignedToUserId",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "Description",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.ProjectId != newTicket.ProjectId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.Project.Name,
                    NewValue = newTicket.Project.Name,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "ProjectId",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
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
            if (oldTicket.Title != newTicket.Title)
            {
                var newHistoryRecord = new TicketHistory
                {
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Changed = (DateTime)newTicket.Updated,
                    Property = "Title",
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }

            db.SaveChanges();

        }
    }
}