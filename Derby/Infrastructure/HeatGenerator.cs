using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Derby.Infrastructure;
using Derby.Models;
using Derby.ViewModels;
using Microsoft.AspNet.Identity;

namespace Derby.Infrastructure
{
    public class HeatGenerator
    {
        private DerbyDb db = new DerbyDb();

        public int HeatCount { get; set; }
        private CompetitionViewModel Competition { get; set; }

        public HeatGenerator(CompetitionViewModel competition)
        {
            this.Competition = competition;
        }

        /* http://www.rahul.net/mcgrew/derby/methods.html#chaotic
         * Get number of heats based on LaneCount and RacerCount
         * Get number of lanes per heat based on LaneCount, HeatCount, and RacerCount
         * 
         * Add each Heat
         *  - For each Heat
         *    Select random number of Racers based on "Den" assignment
         *    Each Racer must race 3 times
         *    
        
         */
        public Race GenerateRace(Den den)
        {
            var racers = db.Racers.Where(x => x.CompetitionId == Competition.Id && x.DenId == den.Id).ToList();
            int totalHeats = racers.Count; //HeatGenerator.GenerateHeatCount(Competition.LaneCount, racers.Count);

            var _race = new Race();
            _race.CompetitionId = Competition.Id;
            _race.CreatedDate = DateTime.Now;
            _race.DenId = den.Id;


            db.Races.Add(_race);
            db.SaveChanges();


            for (int i = 0; i <= totalHeats; i++)
            {
                var _heat = new Heat();
                _heat.RaceId = _race.Id;
                _heat.CreatedDate = DateTime.Now;
                db.Heats.Add(_heat);
                db.SaveChanges();


                foreach (var racer in racers)
                {
                    var _racer = racer;
                    Contestant _contestant = new Contestant();
                    _contestant.HeatId = _heat.Id;
                    _contestant.RacerId = _racer.Id;
                    //_contestant.Lane =  
                }
            }

            return _race;
        }


    }
}