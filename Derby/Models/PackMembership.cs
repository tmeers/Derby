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
        public int PackId { get; set; }
        public string UserId { get; set; }
        public OwnershipType AccessLevel { get; set; } 
    }
}