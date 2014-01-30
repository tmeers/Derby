using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Antlr.Runtime;
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
        private CompetitionViewModel currentCompetition { get; set; }
        private List<Contestant> usedContestants = new List<Contestant>();
        private List<Racer> racers = new List<Racer>(); 

        public HeatGenerator(CompetitionViewModel competition)
        {
            this.currentCompetition = competition;
        }

        /* http://www.rahul.net/mcgrew/derby/methods.html#chaotic
         *      
         * Derby rules: https://gist.github.com/tmeers/8701826
         */
        public Race GenerateRace(Den den)
        {
            racers = db.Racers.Where(x => x.CompetitionId == currentCompetition.Id && x.DenId == den.Id).ToList();
            int totalHeats = GenerateHeatCount(currentCompetition.LaneCount, racers.Count);

            //ICollection<int> lanes = GetLanes(currentCompetition.LaneCount);

            var _race = new Race();
            _race.CompetitionId = currentCompetition.Id;
            _race.CreatedDate = DateTime.Now;
            _race.DenId = den.Id;


            db.Races.Add(_race);
            db.SaveChanges();

            //bool firstHeat = true;
            for (int i = 1; i <= totalHeats; i++)
            {
                var _heat = new Heat();
                _heat.RaceId = _race.Id;
                _heat.CreatedDate = DateTime.Now;
                db.Heats.Add(_heat);
                db.SaveChanges();

                foreach (var lane in FillLineup(racers, _heat.Id))
                {
                    var _lane = lane;
                    Contestant _contestant = new Contestant();
                    _contestant.HeatId = _heat.Id;
                    _contestant.RacerId = _lane.RacerId;
                    _contestant.Lane = _lane.LaneNumber;

                    db.Contestants.Add(_contestant);
                    db.SaveChanges();

                    usedContestants.Add(_contestant);
                }
                //firstHeat = false;
            }

            return _race;
        }

        // Heat count based off of numbers from known working spreadsheets (ugh spreadsheets) here: https://sites.google.com/site/pinewoodscore/Spreadsheets
        private int GenerateHeatCount(int laneCount , int racersCount )
        {
            if (laneCount == 4)
                return racersCount;

            if (laneCount == 6 && ((racersCount <= 7 && racersCount > 3) || racersCount == 9 || racersCount == 10 || racersCount == 12))
                return racersCount;

            if (laneCount == 6 && (racersCount <= 3))
                return 4;

            return laneCount;
        }

        private ICollection<int> GetLanes()
        {
            ICollection<int> lanes = new Collection<int>();
            for (int i = 1; i <= currentCompetition.LaneCount; i++)
            {
                lanes.Add(i);
            }
            return lanes;
        }

        private IEnumerable<Lane> FillLineup(List<Racer> racers, int heatId)
        {
            List<Contestant> previousHeat = usedContestants.Where(x => x.HeatId == heatId).ToList();
            var lanes = GetLanes();

            Random r = new Random();
            List<Racer> topRacers = racers.OrderBy(x => r.Next()).ToList();
            foreach (var item in lanes)
            {
                // loop over lanes 
                // look up next racer
                // assign lane
                var lane = item;
                bool racerAssigned = false;
                Racer _racer = new Racer();
                while (!racerAssigned)
                {
                    _racer = topRacers.Take(1).First();
                    var previousRaces = usedContestants.Where(x => x.RacerId == _racer.Id).ToList();
                    if (previousRaces.Count() < 3)
                    {

                    }
                }

            }
            return lineup;
        }

    }
}