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
            var racesView = new List<RaceViewModel>();

            foreach (var race in races)
            {
                var _race = new RaceViewModel(race);
                _race.Den = db.Dens.Find(race.DenId);
                _race.Competition = db.Competitions.Find(race.CompetitionId);
                _race.Racers = db.Racers.Where(r => r.CompetitionId == competitionId).ToList();

                racesView.Add(_race);
            }

            return View(racesView);
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
        public ActionResult Create(int competitionId, int? denId)
        {
            var raceView = BuildRaceCreateView(competitionId, denId);


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
                var check = db.Races.FirstOrDefault(x => x.DenId == race.DenId && x.CompetitionId == race.CompetitionId);
                if (check != null || check.Id > 0)
                {
                    var raceView = BuildRaceCreateView(race.CompetitionId, race.DenId);
                    raceView.Message = "A Race has already been created for this Den. Additional races will not save.";

                    return View(raceView);
                }

                race.CreatedDate = DateTime.Now;
                race.CompletedDate = DateTime.Now;
                db.Races.Add(race);
                db.SaveChanges();
                return RedirectToAction("Index", "Race", new { competitionId = race.CompetitionId });
            }

            return View(race);
        }

        private RaceViewModel BuildRaceCreateView(int competitionId, int? denId)
        {
            var raceView = new RaceViewModel();
            raceView.Competition = db.Competitions.FirstOrDefault(c => c.Id == competitionId);
            raceView.Dens = db.Dens.Where(d => d.PackId == raceView.Competition.PackId).ToList();
            //raceView.DenList = new ArrayList();
            //foreach (var den in raceView.Dens)
            //{
            //    raceView.DenList.Add(new SelectListItem {Text = den.Name, Value = den.Id.ToString(), Selected = false});
            //}
            ViewBag.DenId = 0;

            if (denId != null)
            {
                ViewBag.DenId = denId;
                var check = db.Races.FirstOrDefault(x => x.DenId == denId && x.CompetitionId == competitionId);
                if (check != null || check.Id > 0)
                {
                    raceView.Message = "A Race has already been created for this Den. Additional races will not save.";
                }

            }

            return raceView;
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
            var _competitionId = race.CompetitionId;

            db.Races.Remove(race);
            db.SaveChanges();

            return RedirectToAction("Index", "Race", new {competitionId = _competitionId});
        }


        public ActionResult Generate(int competitionId, int denId)
        {
            RaceViewModel view = new RaceViewModel();
            view.CompetitionId = competitionId;
            view.Competition = db.Competitions.FirstOrDefault(x => x.Id == competitionId);

            var _den = db.Dens.FirstOrDefault(x => x.Id == denId);
            if (_den == null)
            {
                return RedirectToAction("Details", "Competition", new { id = competitionId });
            }

            return View(view);
        }

        [HttpPost, ActionName("Generate")]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateConfirmed(int competitionId, int denId)
        {
            Competition competition = db.Competitions.Find(competitionId);
            var user = User.Identity.GetUserId();
            CompetitionHelper helper = new CompetitionHelper();

            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            var _den = db.Dens.FirstOrDefault(x => x.Id == denId);
            if (_den == null)
            {
                return RedirectToAction("Details", "Competition", new {id = competitionId});
            }

            var generator = new HeatGenerator(view);
            var race = generator.GenerateRace(_den);


            return RedirectToAction("Details", "Competition", new { id = competitionId });
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
