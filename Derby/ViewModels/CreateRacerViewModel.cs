using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class CreateRacerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Car Number")]
        public string CarNumber { get; set; }

        [Display(Name = "Car Weight")]
        public double Weight { get; set; }

        public ICollection<Den> Dens { get; set; } 

        public int DenId { get; set; }
        public int ScoutId { get; set; }
        public int CompetitionId { get; set; }
    }
}