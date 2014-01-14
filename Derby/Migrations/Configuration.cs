using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Derby.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Derby.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Derby.Models.DerbyDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Derby.Models.DerbyDb context)
        {
            var user = new ApplicationUser() { UserName = "demo" };
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var result = userManager.Create(user, "demo");
            //context.Packs.AddOrUpdate(p => p.Name,
            //    new Pack
            //    {
            //        Name = "000",
            //        Region = "My Region",
            //        Dens = new List<Den> 
            //        { 
            //            new Den { Name = "Tigers"},
            //            new Den { Name = "Wolves"},
            //            new Den { Name = "Bears"},
            //            new Den { Name = "Weblows"} 
            //        }
            //    });

            //context.Scouts.AddOrUpdate(s => s.Name,
            //    new Scout
            //    {
            //        Name = "John Doe",
            //        Inactive = false
            //    });
        }
    }
}
