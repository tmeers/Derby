using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Derby.Models
{
	public class Den
	{
		public int Id { get; set; }
		public string Name { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDateTime { get; set; }
		public int PackId { get; set; }
        public string LogoPath { get; set; }
        //public ICollection<Scout> Scouts { get; set; }
	}
}