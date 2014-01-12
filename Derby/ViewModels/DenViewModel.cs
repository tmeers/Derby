using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class DenViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDateTime { get; set; }
        public int PackId { get; set; }
        public string LogoPath { get; set; }

        public ICollection<Racer> Racers { get; set; }

        public DenViewModel(Den den)
        {
            Id = den.Id;
            Name = den.Name;
            CreatedDateTime = den.CreatedDateTime;
            PackId = den.PackId;
            LogoPath = den.LogoPath;
        }
    }
}