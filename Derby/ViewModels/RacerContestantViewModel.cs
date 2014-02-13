using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.ViewModels
{
    public class RacerContestantViewModel
    {
        public int ContestantId { get; set; }
        public int RacerId { get; set; }
        public int ScoutId { get; set; }
        public int RaceId { get; set; }
        public string ScoutName { get; set; }
        public string CarNumber { get; set; }
        public bool Selected { get; set; }
        public string Lane { get; set; }
    }
}