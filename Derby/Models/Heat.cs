using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Models
{
    public class Heat
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Contestant> Contestants { get; set; }
    }
}