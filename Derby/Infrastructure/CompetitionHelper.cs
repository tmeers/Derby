using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Infrastructure;
using Derby.Models;
using Derby.ViewModels;
using System.Security;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace Derby.Infrastructure
{
    public class CompetitionHelper
    {
        DerbyDb db = new DerbyDb();

        public CompetitionViewModel LoadCompetition(Competition competition, string user)
        {
            CompetitionViewModel view = new CompetitionViewModel(competition);

            PackAccess access = new PackAccess();
            PackViewModel pack = access.BuildPackListing(user).Find(x => x.Id == competition.PackId);

            view.Pack = pack;//db.Packs.FirstOrDefault(p => p.Id == view.PackId);


            var _racers = db.Racers.Where(r => r.CompetitionId == view.Id).ToList();
            view.Scouts = db.Scouts.Where(r => r.PackId == view.PackId).ToList();

            var _dens = db.Dens.Where(p => p.PackId == competition.PackId);

            foreach (var den in _dens)
            {
                var _den = den;
                //var denView = new DenCompetitionViewModel(_den);
                foreach (var racer in _racers.Where(d => d.DenId == _den.Id))
                {
                    var racerView = new RacerViewModel(racer);
                    racerView.Den = _den;
                    racerView.Scout = view.Scouts.FirstOrDefault(s => s.Id == racer.ScoutId);

                    view.Racers.Add(racerView);
                    //denView.Racers.Add(racerView);
                }

                view.Dens.Add(_den);
            }

            var races = db.Races.Where(x => x.CompetitionId == competition.Id).ToList();
            view.Races = new List<RaceViewModel>();

            foreach (var race in races)
            {
                var _race = new RaceViewModel(race);
                _race.Heats = db.Heats.Where(x => x.RaceId == race.Id).ToList();

                view.Races.Add(_race);
            }


            return view;
        }

    }
}