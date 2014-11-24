using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Derby.Infrastructure;
using Derby.Models;

namespace Derby.ViewModels
{
    public class SentInvitationViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ExpiryOffset { get; set; }

        public virtual ApplicationUser InvitedBy { get; set; }
        public string InvitedEmail { get; set; }
        public bool Accepted { get; set; }
        public virtual Pack Pack { get; set; }
        public EmailStatus Status { get; set; }
    }
}