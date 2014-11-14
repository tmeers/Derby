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
    public class PackInvitationViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ExpiryOffset { get; set; }

        [Index("InvitationCode", IsUnique = true)]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Code { get; set; }
        public virtual ApplicationUser InvitedBy { get; set; }
        public string InvitedEmail { get; set; }
        public bool Accepted { get; set; }
        public string AcceptedUserId { get; set; }
        public virtual Pack Pack { get; set; }
        public virtual List<PackViewModel> PackList { get; set; }
        public int PackId { get; set; }

        public PackInvitationViewModel()
        {
        }

        public PackInvitationViewModel(string user)
        {
            Pack = new Pack();
            PackList = new List<PackViewModel>();
            Infrastructure.PackAccess packs = new PackAccess();

            PackList = packs.BuildPackListing(user);
        }
    }
}