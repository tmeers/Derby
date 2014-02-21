using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class RaceViewModel
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        public int DenId { get; set; }
        public Den Den { get; set; }

        public Competition Competition { get; set; }
        public ICollection<Den> Dens { get; set; }

        public List<Racer> Racers { get; set; }
        public List<Heat> Heats { get; set; }
        public string Message { get; set; }

        public RaceViewModel() { }

        public RaceViewModel(Race race)
        {
            Id = race.Id;
            CompetitionId = race.CompetitionId;
            CreatedDate = race.CreatedDate;
            CompletedDate = race.CompletedDate;
            DenId = race.DenId;
            Den = new Den();
            Competition = new Competition();
            Dens = new Collection<Den>();
            Racers = new List<Racer>();
            Heats = new List<Heat>();
        }
    }
}