using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Derby.Infrastructure;
using Derby.Models;
using Microsoft.WindowsAzure;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Derby.Services.Email
{
    public class EmailSender
    {
        public static bool Send(PackInvitation invite)
        {

            string urlBase = System.Configuration.ConfigurationManager.AppSettings["Derby.weburl"];
            urlBase += "invite/" + invite.Code;
            
            string messageBody = string.Format(@"Hello,
You've been invited to join Derby for your Packs upcoming race! To join in on the race click the link below. You can also copy and paste it into your browser. 

{0}

Thanks!

If you are receiving this in error, please feel free to ignore the message.", urlBase);

            var email = new InviteEmail
            {
                To = invite.InvitedEmail,
                UniqueCode = invite.Code,
                Message = messageBody
            };


                //email.Send();
                invite.Status = EmailStatus.Sent;
                var test = Execute(email);
            
                using (var db = new DerbyDb())
                {
                    db.PackInvitations.AddOrUpdate(invite);
                    db.SaveChanges();
                }
                return true;

        }

        static bool Execute(InviteEmail invite)
        {//azure_d93cfc4ceb2bf041d31708674420427a@azure.com
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["Sendgrid.APIKey"];
            dynamic sg = new SendGridAPIClient(apiKey);

            SendGrid.Helpers.Mail.Email from = new SendGrid.Helpers.Mail.Email(System.Configuration.ConfigurationManager.AppSettings["Sendgrid.Email"]);
            string subject = "Derby Invitation";
            SendGrid.Helpers.Mail.Email to = new SendGrid.Helpers.Mail.Email(invite.To);
            Content content = new Content("text/plain", invite.Message);
            Mail mail = new Mail(from, subject, to, content);

            try
            {
                sg.client.mail.send.post(requestBody: mail.Get());
                return true;
            }
            catch 
            {
                return false;
            }

        }
    }
}