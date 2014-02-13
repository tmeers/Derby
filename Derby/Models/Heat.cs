using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Heat
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        
        public virtual ICollection<Contestant> Contestants { get; set; }
    }
}