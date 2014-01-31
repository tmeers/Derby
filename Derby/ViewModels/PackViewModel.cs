using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Derby.Infrastructure;
using Derby.Models;

namespace Derby.ViewModels
{
    public class PackViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreateDateTime { get; set; }

        public PackMembership Membership { get; set; }
        public ICollection<Den> Dens { get; set; }
        public ICollection<Competition> Competitions { get; set; }
        public ICollection<Scout> Scouts { get; set; }

        public PackViewModel()
        {
            Membership.AccessLevel = OwnershipType.None;
        }

        public PackViewModel(Pack pack)
        {
            Id = pack.Id;
            Name = pack.Name;
            Region = pack.Region;
            CreatedById = pack.CreatedById;
            CreateDateTime = pack.CreateDateTime;
        }
    }
}