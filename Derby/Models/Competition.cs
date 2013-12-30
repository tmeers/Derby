using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public int PackId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public int RaceType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EventDate { get; set; }
        public string CreatedById { get; set; }

        public Pack Pack { get; set; }
    }
}