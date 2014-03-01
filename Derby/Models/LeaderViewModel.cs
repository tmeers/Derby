using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class LeaderViewModel
    {
        public string Name { get; set; }
        public string CarNumber { get; set; }
        public int Points { get; set; }
        public int DenId { get; set; }
        public string DenLogo { get; set; }
    }
}