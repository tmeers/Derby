using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Infrastructure;

namespace Derby.Models
{
    public class PackMembership
    {
        [Key]
        public int Id { get; set; }
        public OwnershipType AccessLevel { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Pack Pack { get; set; }


        public PackMembership()
        {
            AccessLevel = OwnershipType.None;
        }
    }
}