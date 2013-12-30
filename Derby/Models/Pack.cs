using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Derby.Models
{
	public class Pack
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
        public string CreatedById { get; set; }

        public ICollection<Den> Dens { get; set; }
        public ICollection<Competition> Competitions { get; set; }
        public ICollection<Scout> Scouts { get; set; }
	}
}