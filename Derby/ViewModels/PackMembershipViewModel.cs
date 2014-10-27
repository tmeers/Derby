using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Derby.Models;

namespace Derby.ViewModels
{
    public class PackMembershipViewModel
    {
        public List<PackViewModel> Packs { get; set; }
        public List<PackMembership> PackMemberships { get; set; }
    }
}