using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Derby.Models
{
	public class DerbyDb : DbContext
	{
		public DerbyDb() : base("name=DerbyDb") { }

        public DbSet<Pack> Packs { get; set; }
		public DbSet<Den> Dens { get; set; }
        public DbSet<Scout> Scouts { get; set; }

        public DbSet<Competition> Competitions { get; set; }
		public DbSet<Racer> Racers { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Heat> Heats { get; set; }
        public DbSet<Contestant> Contestants { get; set; }

        public DbSet<PackMembership> PackMemberships { get; set; }
	}
}