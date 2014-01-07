using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
	public class Racer
	{
		public int Id { get; set; }
        [Display(Name = "Car Number")]
        public string CarNumber { get; set; }

        [Display(Name = "Car Weight")]
        public double Weight { get; set; }

		public int DenId { get; set; }
        public int ScoutId { get; set; }
	}
}