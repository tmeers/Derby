using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class RacerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Car Number")]
        public string CarNumber { get; set; }

        [Display(Name = "Car Weight")]
        public double Weight { get; set; }

        public Den Den { get; set; }
        public Scout Scout { get; set; }
        public int CompetitionId { get; set; }

        public RacerViewModel(Racer racer)
        {
            Id = racer.Id;
            CarNumber = racer.CarNumber;
            Weight = racer.Weight;
            CompetitionId = racer.CompetitionId;
        }
    }
}