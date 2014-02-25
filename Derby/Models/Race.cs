using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Race
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        public int DenId { get; set; }

        public virtual ICollection<Racer> Racers { get; set; }
        public virtual ICollection<Heat> Heats { get; set; }
    }
}