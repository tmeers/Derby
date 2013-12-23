using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Models
{
	public class Racer
	{
		public int Id { get; set; }
        public string CarNumber { get; set; }

		public int DenId { get; set; }
        public int ScoutId { get; set; }
	}
}