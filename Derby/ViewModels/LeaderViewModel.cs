using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.ViewModels
{
    public class LeaderViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarNumber { get; set; }
        public int Points { get; set; }
        public int DenId { get; set; }
        public string DenLogo { get; set; }
        public int ScoutId { get; set; }
        public double Weight { get; set; }

        public bool Selected { get; set; }
    }
}