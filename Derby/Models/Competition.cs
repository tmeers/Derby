using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public int PackId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }

        [Required]
        [Display(Name = "Race Type")]
        public int RaceType { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        [Display(Name = "Number of Lanes")]
        public int LaneCount { get; set; }

        public string CreatedById { get; set; }

        public Pack Pack { get; set; }
    }
}