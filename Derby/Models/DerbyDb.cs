using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Derby.Models
{
    public class ApplicationUser : IdentityUser
    {
    }

    public class DerbyDb : IdentityDbContext
    {
        public DerbyDb() : base("name=DerbyDb") { }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Pack> Packs { get; set; }
        public DbSet<Den> Dens { get; set; }
        public DbSet<Scout> Scouts { get; set; }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Racer> Racers { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Heat> Heats { get; set; }
        public DbSet<Contestant> Contestants { get; set; }

        public DbSet<PackMembership> PackMemberships { get; set; }
        public DbSet<PackInvitation> PackInvitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<Derby.ViewModels.CompetitionViewModel> CompetitionViewModels { get; set; }
        public System.Data.Entity.DbSet<Derby.ViewModels.LeaderViewModel> LeaderViewModels { get; set; }

        public System.Data.Entity.DbSet<Derby.ViewModels.PackViewModel> PackViewModels { get; set; }
    }
}