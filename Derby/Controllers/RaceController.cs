using System;
using System.Collections;
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
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    [Authorize]
    public class RaceController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Race/
        public ActionResult Index(int? competitionId)
        {
            if (competitionId == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var races = db.Races.Where(x => x.CompetitionId == competitionId).ToList();


            foreach (var race in races)
            {
                race.Racers = db.Racers.Where(r => r.CompetitionId == competitionId).ToList();
            }

            return View(races);
        }

        // GET: /Race/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }

            race.Racers = db.Racers.Where(r => r.CompetitionId == race.CompetitionId).ToList();
            
            return View(race);
        }

        // GET: /Race/Create
        public ActionResult Create(int competitionId)
        {
            var raceView = new RaceViewModel();
            raceView.Competition = db.Competitions.FirstOrDefault(c => c.Id == competitionId);
            raceView.Dens = db.Dens.Where(d => d.PackId == raceView.Competition.PackId).ToList();
            raceView.DenList = new ArrayList();
            foreach (var den in raceView.Dens)
            {
                raceView.DenList.Add(new SelectListItem {Text = den.Name, Value = den.Id.ToString(), Selected = false});
            }

            return View(raceView);
        }

        // POST: /Race/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CompetitionId,DenId")] Race race)
        {
            if (ModelState.IsValid)
            {
                race.CreatedDate = DateTime.Now;
                race.CompletedDate = DateTime.Now;
                db.Races.Add(race);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(race);
        }

        // GET: /Race/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // POST: /Race/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CompetitionId,CreatedDate,CompletedDate,DenId")] Race race)
        {
            if (ModelState.IsValid)
            {
                db.Entry(race).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(race);
        }

        // GET: /Race/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // POST: /Race/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Race race = db.Races.Find(id);
            db.Races.Remove(race);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Generate(int competitionId)
        {
            RaceViewModel view = new RaceViewModel();
            view.CompetitionId = competitionId;
            view.Competition = db.Competitions.FirstOrDefault(x => x.Id == competitionId);

            return View(view);
        }

        [HttpPost, ActionName("Generate")]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateConfirmed(int id)
        {
            Competition competition = db.Competitions.Find(id);
            var user = User.Identity.GetUserId();
            CompetitionHelper helper = new CompetitionHelper();

            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            var racers = db.Racers.Where(x => x.CompetitionId == view.Id).ToList();

            foreach (var den in view.Dens)
            {
                var _den = den;

                var _race = new Race();
                _race.CompetitionId = competition.Id;
                _race.CreatedDate = DateTime.Now;
                _race.DenId = _den.Id;

                db.Races.Add(_race);
                db.SaveChanges();

                var _denRacers = racers.Where(x => x.DenId == _den.Id).ToList();

                int _totalHeats = HeatGenerator.GenerateHeatCount(competition.LaneCount, _denRacers.Count);
                /*
                 * Get number of heats based on LaneCount and RacerCount
                 * Get number of lanes per heat based on LaneCount, HeatCount, and RacerCount
                 * 
                 * Add each Heat
                 *  - For each Heat
                 *    Select random number of Racers
                 *    Each Racer must race N times, but what is N?
                 *       Or should there be an elimination point level? In orde rto move to next heat you must get N points?
                 */
                for (int i = 0; i <= _totalHeats; i++)
                {
                    var _heat = new Heat();
                    _heat.RaceId = _race.Id;
                    _heat.CreatedDate = DateTime.Now;
                    db.Heats.Add(_heat);
                    db.SaveChanges();


                    foreach (var racer in _denRacers)
                    {
                        var _racer = racer;
                        Contestant _contestant = new Contestant();
                        _contestant.HeatId = _heat.Id;
                        _contestant.RacerId = _racer.Id;
                        _contestant.Lane = 
                    }
                }
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
