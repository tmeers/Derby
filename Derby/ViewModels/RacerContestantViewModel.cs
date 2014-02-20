using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class RacerContestantViewModel
    {
        public int Id { get; set; }
        [Key]
        public int RacerId { get; set; }
        public int ScoutId { get; set; }
        public int RaceId { get; set; }
        public string ScoutName { get; set; }
        public string CarNumber { get; set; }
        public bool Selected { get; set; }
       
        [StringLength(1, ErrorMessage = "The {0} must be at least {2} number long.", MinimumLength = 1)]
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter only numbers")]
        public string Lane { get; set; }


        public RacerContestantViewModel() { }

        public RacerContestantViewModel(Racer racer)
        {
            RacerId = racer.Id;
            ScoutId = racer.ScoutId;
            CarNumber = racer.CarNumber;
        }
    }
}