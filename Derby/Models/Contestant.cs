using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Contestant
    {
        public int Id { get; set; }
        public int RacerId { get; set; }
        public int HeatId { get; set; }
        public int Place { get; set; }
    }
}