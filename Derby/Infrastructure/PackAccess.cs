using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Derby.Models;
using Derby.ViewModels;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace Derby.Infrastructure
{
    public class PackAccess
    {
        DerbyDb db = new DerbyDb();

        public List<PackViewModel> BuildPackListing(string user)
        {
            var packs = new List<Pack>();

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                packs = db.Packs.ToList();
            }
            else
            {
                var membership = db.PackMemberships.Where(x => x.UserId == user);
                // TODO Need to see about a better way to do this part. 
                foreach (var item in membership)
                {
                    packs.Add(db.Packs.FirstOrDefault(x => x.Id == item.PackId));
                }
            }

            var packsView = new List<PackViewModel>();
            foreach (var pack in packs)
            {
                var view = new PackViewModel(pack);
                view.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList();
                view.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();
                view.Membership = db.PackMemberships.FirstOrDefault(x => x.UserId == user);
                view.Competitions = db.Competitions.Where(c => c.PackId == pack.Id).ToList();

                packsView.Add(view);
            }

            db.Dispose();

            return packsView;
        }
    }
}