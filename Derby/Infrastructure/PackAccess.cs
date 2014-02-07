using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
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
                // TODO Need to see about a better way to do this part. 
                packs.AddRange(getMemberships(user).Select(item => db.Packs.FirstOrDefault(x => x.Id == item.PackId)));
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

        private IQueryable<PackMembership> getMemberships(string user)
        {
            return db.PackMemberships.Where(x => x.UserId == user);
        } 

        internal PackViewModel OpenPack(int? id, string user)
        {
            PackAccess access = new PackAccess();
            PackViewModel pack = access.BuildPackListing(user).Find(x => x.Id == id);

            return pack;
        }

        internal PackMembership GetCompetitionMembership(int? competitionId, string user)
        {
            Competition comp = db.Competitions.FirstOrDefault(x => x.Id == competitionId);
            PackMembership member = getMemberships(user).FirstOrDefault(m => m.PackId == comp.PackId);

            return member;
        }

        internal PackMembership GetDenMembership(int? denId, string user)
        {
            Den den = db.Dens.FirstOrDefault(x => x.Id == denId);
            PackMembership member = getMemberships(user).FirstOrDefault(m => m.PackId == den.PackId);

            return member;
        }
    }
}