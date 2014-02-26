using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class RunHeatViewModel
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
        public ICollection<ContestantViewModel> Contestants { get; set; }

        public RunHeatViewModel()
        {
            Contestants = new List<ContestantViewModel>();
        }
    }
}