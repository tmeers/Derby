using System;
using System.Collections.Generic;
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