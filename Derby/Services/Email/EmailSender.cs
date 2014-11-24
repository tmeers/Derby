using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Derby.Infrastructure;
using Derby.Models;

namespace Derby.Services.Email
{
    public class EmailSender
    {
        public static bool Send(PackInvitation invite)
        {
            var email = new InviteEmail
            {
                To = invite.InvitedEmail,
                UniqueCode = invite.Code
            };

            try
            {
                email.Send();
                invite.Status = EmailStatus.Sent;

                using (var db = new DerbyDb())
                {
                    db.PackInvitations.AddOrUpdate(invite);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            return false;
        }

    }
}