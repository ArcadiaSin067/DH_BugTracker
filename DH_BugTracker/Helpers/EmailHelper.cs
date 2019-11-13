using DH_BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace DH_BugTracker.Helpers
{
    public class EmailHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        private static string ConfiguredEmail = WebConfigurationManager.AppSettings["emailfrom"];
        public static async Task ComposeEmailAsync(EmailModel email)
        {
            try
            {
                var senderEmail = $"{email.FromEmail}<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, ConfiguredEmail)
                {
                    Subject = email.Subject,
                    Body = $"<strong>{email.FromName} has sent you the following message</strong> <hr /> {email.Body}",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        public static async Task ComposeEmailAsync(RegisterViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Bug Tracker Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Confirm your account",
                    Body = $"Please confirm your account by clicking <a href=\"{callbackUrl}\">here</a>",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }
        public static async Task ComposeEmailAsync(ForgotPasswordViewModel model, string callbackUrl)
        {
            try
            {
                var senderEmail = $"Bug Tracker Admin<{ConfiguredEmail}>";
                var mailMsg = new MailMessage(senderEmail, model.Email)
                {
                    Subject = "Reset Password",
                    Body = $"Please reset your password by clicking <a href=\"{callbackUrl}\">here</a>",
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
        }

        public async Task AssignDevToTicket_Email(Ticket oldTicket,Ticket newTicket, string callbackUrl)
        {
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId && newTicket.AssignedToUserId != null)
            {
                try
                {
                    EmailService ems = new EmailService();
                    IdentityMessage msg = new IdentityMessage();
                    ApplicationUser user = db.Users.Find(newTicket.AssignedToUserId);
                    msg.Body = "You have been assigned a new Ticket for one of your Projects." + Environment.NewLine +
                    "Please click the following link to view the details " +
                    "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";
                    msg.Destination = user.Email;
                    msg.Subject = "Bug_Tracker Tickets";
                    await ems.SendMailAsync(msg);
                }
                catch (Exception ex)
                {
                    await Task.FromResult(0);
                }
            }




        }


    }
}