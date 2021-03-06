﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class ModifyHeatViewModel
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Tie Breaker")]
        public bool TieBreaker { get; set; }

        public int HeatsNeeded { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public ICollection<Heat> CurrentHeats { get; set; } 
        public ICollection<RacerContestantViewModel> Racers { get; set; }

        public ModifyHeatViewModel()
        {
            Racers = new List<RacerContestantViewModel>();
        }
    }
}