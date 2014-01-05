using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace Derby.Models
{
    public class Scout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }
        public int PackId { get; set; }
        public int DenId { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreateDateTime { get; set; }

        //TODO Need to add link to be able to list all cars/competitions Scout has been in
    }
}