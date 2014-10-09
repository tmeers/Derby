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
                packs.AddRange(getMemberships(user).Select(item => db.Packs.FirstOrDefault(x => x.Id == item.Pack.Id)));
            }

            var packsView = new List<PackViewModel>();
            foreach (var pack in packs)
            {
                var view = new PackViewModel(pack);

                view.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList();
                view.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();
                view.Membership = db.PackMemberships.FirstOrDefault(x => x.User.Id == user);
                view.Competitions = db.Competitions.Where(c => c.PackId == pack.Id).ToList();

                packsView.Add(view);
            }

            db.Dispose();

            return packsView;
        }

        internal PackViewModel OpenPack(int? id, string user, OwnershipType minimuLevel)
        {
            PackAccess access = new PackAccess();
            PackViewModel pack = access.BuildPackListing(user).Find(x => x.Id == id);

            if (pack != null && checkAccess(pack.Membership.AccessLevel, minimuLevel))
                return pack;
            
            return null;
        }

        internal bool CheckCompetitionMembership(int? packId, string user, OwnershipType minimuLevel)
        {
            //Competition comp = db.Competitions.FirstOrDefault(x => x.Id == competitionId);
            PackMembership member = getMemberships(user).FirstOrDefault(m => m.Pack.Id == packId);

            if (member != null && checkAccess(member.AccessLevel, minimuLevel))
                return true;

            return false;
        }

        internal bool CheckDenMembership(int? denId, string user, OwnershipType minimuLevel)
        {
            Den den = db.Dens.FirstOrDefault(x => x.Id == denId);
            PackMembership member = getMemberships(user).FirstOrDefault(m => m.Pack.Id == den.PackId);

            if (member != null && checkAccess(member.AccessLevel, minimuLevel))
                return true;

            return false;
        }

        internal bool CheckScoutMembership(int? scoutId, string user, OwnershipType minimuLevel)
        {
            Scout den = db.Scouts.FirstOrDefault(x => x.Id == scoutId);
            PackMembership member = getMemberships(user).FirstOrDefault(m => m.Pack.Id == den.PackId);

            if (member != null && checkAccess(member.AccessLevel, minimuLevel))
                return true;

            return false;
        }

        internal bool CheckRaceMembership(int? raceId, string user, OwnershipType minimuLevel)
        {
            Race race = db.Races.FirstOrDefault(x => x.Id == raceId);
            return CheckCompetitionMembership(db.Competitions.FirstOrDefault(c => c.Id == race.CompetitionId).PackId,
                user, minimuLevel);
        }

        private IQueryable<PackMembership> getMemberships(string user)
        {
            return db.PackMemberships.Where(x => x.User.Id == user);
        }

        private bool checkAccess(OwnershipType given, OwnershipType minimum)
        {
            if (minimum == OwnershipType.Guest && (given == OwnershipType.Guest || given == OwnershipType.Contributor || given == OwnershipType.Owner))
            {
                return true;
            }
            else if (minimum == OwnershipType.Contributor && (given == OwnershipType.Contributor || given == OwnershipType.Owner))
            {
                return true;
            }
            else if (minimum == OwnershipType.Owner && given == OwnershipType.Owner)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}