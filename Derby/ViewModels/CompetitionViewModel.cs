using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Infrastructure;
using Derby.Models;

namespace Derby.ViewModels
{
    public class CompetitionViewModel
    {
        public int Id { get; set; }
        public int PackId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }

        [Required]
        [Display(Name = "Race Type")]
        public RaceType RaceType { get; set; }

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

        public bool Completed { get; set; }

        public List<RacerViewModel> Racers { get; set; }
        public List<Den> Dens { get; set; }

        public CompetitionViewModel(Competition competition)
        {
            Id = competition.Id;
            PackId = competition.PackId;
            Title = competition.Title;
            Location = competition.Location;
            RaceType = competition.RaceType;
            CreatedDate = competition.CreatedDate;
            EventDate = competition.EventDate;
            LaneCount = competition.LaneCount;
            CreatedById = competition.CreatedById;
            Completed = competition.Completed;
            Racers = new List<RacerViewModel>();
            Dens = new List<Den>();
        }
    }
}