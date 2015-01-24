using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Infrastructure;
using Derby.Models;
using Derby.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    public class HeatController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Heat/
        public ActionResult Index()
        {
            return View(db.Heats.ToList());
        }

        // GET: /Heat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Heat heat = db.Heats.Find(id);
            if (heat == null)
            {
                return HttpNotFound();
            }
            return View(heat);
        }

        // GET: /Heat/Create
        public ActionResult Create(int? raceId, string returnPath)
        {
            if (raceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["returnPath"]))
            {
                TempData["returnPath"] = Request.QueryString["returnPath"];
            }
            
            return View(LoadCreateView(raceId.Value));
        }

        // POST: /Heat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RaceId,Racers,TieBreaker,CompetitionId")] ModifyHeatViewModel heat)
        {
            if (ModelState.IsValid)
            {
                if (!heat.Racers.Any())
                {
                    return View(LoadCreateView(heat.RaceId));
                }

                Heat _heat = new Heat();
                _heat.CreatedDate = DateTime.Now;
                _heat.RaceId = heat.RaceId;
                _heat.TieBreaker = heat.TieBreaker;

                db.Heats.Add(_heat);
                db.SaveChanges();

                foreach (var racer in heat.Racers.Where(x => x.Selected == true))
                {
                    var _racer = racer;
                    Contestant contestant = new Contestant();
                    contestant.HeatId = _heat.Id;
                    contestant.Lane = Convert.ToInt32(_racer.Lane);
                    contestant.RacerId = _racer.RacerId;

                    db.Contestants.Add(contestant);
                    db.SaveChanges();
                }

                object returnPath = string.Empty;
                TempData.TryGetValue("returnPath", out returnPath);
                string returnPathStr = returnPath as string;

                if (!string.IsNullOrEmpty(returnPathStr) && returnPathStr == "competition")
                {
                    return RedirectToAction("Details", "Competition", new { id = heat.CompetitionId });
                }

                return RedirectToAction("Index", "Race", new { competitionId = heat.CompetitionId });
            }

            return View(LoadCreateView(heat.RaceId));
        }

        private ModifyHeatViewModel LoadCreateView(int raceId)
        {
            Race race = db.Races.Find(raceId);
            Competition competition = db.Competitions.Find(race.CompetitionId);
            race.CompetitionId = competition.Id;

            ModifyHeatViewModel view = new ModifyHeatViewModel();
            view.RaceId = raceId;
            view.Racers = new Collection<RacerContestantViewModel>();
            view.Competition = competition;
            view.CurrentHeats = db.Heats.Where(x => x.RaceId == raceId).ToList();
            view.CompetitionId = competition.Id;

            List<Scout> scouts = new List<Scout>(db.Scouts.Where(x => x.PackId == competition.PackId));
            if (scouts.Any())
            {
                foreach (var item in db.Racers.Where(x => x.DenId == race.DenId).ToList())
                {
                    var racer = new RacerContestantViewModel(item);
                    racer.ScoutName = scouts.FirstOrDefault(x => x.Id == item.ScoutId).Name;

                    view.Racers.Add(racer);
                }
            }

            view.HeatsNeeded = Infrastructure.HeatGenerator.GenerateHeatCount(competition.LaneCount, view.Racers.Count());

            return view;
        }

        private ModifyHeatViewModel LoadEditView(int raceId, int heatId)
        {
            Race race = db.Races.Find(raceId);
            Competition competition = db.Competitions.Find(race.CompetitionId);
            race.CompetitionId = competition.Id;

            ModifyHeatViewModel view = new ModifyHeatViewModel();
            view.RaceId = raceId;
            view.Racers = new Collection<RacerContestantViewModel>();
            view.Competition = competition;
            view.CurrentHeats = db.Heats.Where(x => x.RaceId == raceId).ToList();
            view.CompetitionId = competition.Id;
            view.TieBreaker = db.Heats.FirstOrDefault(x => x.Id == heatId).TieBreaker;

            List<Scout> scouts = new List<Scout>(db.Scouts.Where(x => x.PackId == competition.PackId));
            if (scouts.Any())
            {
                foreach (var item in db.Racers.Where(x => x.DenId == race.DenId).ToList())
                {
                    var racer = new RacerContestantViewModel(item);
                    racer.ScoutName = scouts.FirstOrDefault(x => x.Id == item.ScoutId).Name;
                    
                    // Check to see if Racer was already selected
                    var _existingContestant = db.Contestants.FirstOrDefault(x => x.HeatId == heatId && x.RacerId == racer.RacerId);
                    if (_existingContestant != null && _existingContestant.Id != 0)
                    {
                        racer.Selected = true;
                        racer.Lane = _existingContestant.Lane.ToString();
                    }

                    view.Racers.Add(racer);
                }
            }

            view.HeatsNeeded = Infrastructure.HeatGenerator.GenerateHeatCount(competition.LaneCount, view.Racers.Count());

            return view;
        }

        public ActionResult Run(int id)
        {
            Heat heat = db.Heats.FirstOrDefault(x => x.Id == id);
            if (heat == null)
            {
                return HttpNotFound();
            }

            var user = User.Identity.GetUserId();

            PackAccess pa = new PackAccess();

            if (!pa.CheckRaceMembership(heat.RaceId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Race _race = db.Races.Find(heat.RaceId);


            return View(loadHeatRun(heat, _race));
        }

        private RunHeatViewModel loadHeatRun(Heat heat, Race race)
        {
            RunHeatViewModel runHeatView = new RunHeatViewModel(heat);
            runHeatView.Competition = db.Competitions.Find(race.CompetitionId);
            runHeatView.CompetitionId = runHeatView.Competition.Id;

            runHeatView.CurrentHeats = db.Heats.Where(x => x.RaceId == race.Id).ToList();
            List<Contestant> _contestants = db.Contestants.Where(x => x.HeatId == heat.Id).ToList();
            var _scouts = db.Scouts.Where(s => s.PackId == runHeatView.Competition.PackId);
            var _racers = db.Racers.Where(s => s.CompetitionId == runHeatView.Competition.Id);
            SortedDictionary<string, string> _places = new SortedDictionary<string, string>();
            for (var i = 1; i <= runHeatView.Competition.LaneCount; i++)
            {
                _places.Add(Convert.ToString(i), Convert.ToString(i));
            }

            foreach (var contestant in _contestants.OrderBy(x => x.Lane))
            {
                var _contestant = contestant;
                var _contestantView = new ContestantViewModel();
                _contestantView.Id = _contestant.Id;
                _contestantView.RacerId = _contestant.RacerId;

                var _racer = _racers.FirstOrDefault(x => x.Id == _contestant.RacerId);
                var _scout = _scouts.FirstOrDefault(x => x.Id == _racer.ScoutId);

                _contestantView.ScoutId = _racer.ScoutId;
                _contestantView.RaceId = heat.RaceId;
                _contestantView.ScoutName = _scout.Name;
                _contestantView.CarNumber = _racer.CarNumber;
                _contestantView.Lane = _contestant.Lane;
                _contestantView.Place = Convert.ToString(_contestant.Place);
                _contestantView.Places = new SortedDictionary<string, string>(_places);

                runHeatView.Contestants.Add(_contestantView);
            }


            runHeatView.HeatsNeeded = Infrastructure.HeatGenerator.GenerateHeatCount(runHeatView.Competition.LaneCount,
                _racers.Count(x => x.DenId == race.DenId));

            return runHeatView;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Run([Bind(Include = "Id,CompetitionId,Contestants")] RunHeatViewModel heat)
        {
            if (ModelState.IsValid)
            {
                if (heat.Contestants.Any(x => Convert.ToString(x.Place) == ""))
                {
                    Heat _heatError = db.Heats.FirstOrDefault(x => x.Id == heat.Id);
                    Race _race = db.Races.Find(heat.RaceId);

                    RunHeatViewModel _runHeatViewError = new RunHeatViewModel(_heatError);
                    _runHeatViewError.Message = "Some or all of your contestants did not have a Place assigned.";

                    return View(loadHeatRun(_heatError, _race));
                }

                foreach (var contestant in heat.Contestants)
                {
                    Contestant _contestant = db.Contestants.Find(contestant.Id);

                    if (_contestant == null)
                    {
                        return HttpNotFound();
                    }

                    _contestant.Place = Convert.ToInt32(contestant.Place);

                    db.Entry(_contestant).State = EntityState.Modified;
                    db.SaveChanges();
                }

                Heat _heat = db.Heats.Find(heat.Id);

                _heat.IsCompleted = true;

                db.Entry(_heat).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Competition", new {id = heat.CompetitionId});
            }

            return RedirectToAction("Details", "Competition", new { id = heat.CompetitionId });
        }

        // GET: /Heat/Edit/5
        public ActionResult Edit(int? id, int? raceId)
        {
            if (raceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["returnPath"]))
            {
                TempData["returnPath"] = Request.QueryString["returnPath"];
            }

            return View(LoadEditView(raceId.Value, id.Value));
        }

        // POST: /Heat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RaceId,Racers,TieBreaker,CompetitionId")] ModifyHeatViewModel heat)
        {
            if (ModelState.IsValid)
            {
                if (!heat.Racers.Any())
                {
                    return View(LoadCreateView(heat.RaceId));
                }

                Heat _existingHeat = db.Heats.FirstOrDefault(x => x.Id == heat.Id);
                var _newHeat = _existingHeat;
                _newHeat.TieBreaker = heat.TieBreaker;

                db.Heats.Attach(_existingHeat);

                var entry = db.Entry(_newHeat);
                entry.State = EntityState.Modified;

                entry.Property(e => e.CreatedDate).IsModified = false;
                entry.Property(e => e.IsCompleted).IsModified = false;

                db.SaveChanges(); 

                // First kill all the existing records just because it's easier
                db.Contestants.RemoveRange(db.Contestants.Where(x => x.HeatId == heat.Id));
                db.SaveChanges();

                
                foreach (var racer in heat.Racers.Where(x => x.Selected == true))
                {
                    var _racer = racer;
                    Contestant contestant = new Contestant();
                    contestant.HeatId = heat.Id;
                    contestant.Lane = Convert.ToInt32(_racer.Lane);
                    contestant.RacerId = _racer.RacerId;

                    db.Contestants.Add(contestant);
                    db.SaveChanges();
                }

                object returnPath = string.Empty;
                TempData.TryGetValue("returnPath", out returnPath);
                string returnPathStr = returnPath as string;

                return RedirectToAction("Details", "Competition", new { id = heat.CompetitionId });
            }

            return View(LoadCreateView(heat.RaceId));
        }

        // GET: /Heat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Heat heat = db.Heats.Find(id);
            if (heat == null)
            {
                return HttpNotFound();
            }
            return View(heat);
        }

        // POST: /Heat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Heat heat = db.Heats.Find(id);
            db.Heats.Remove(heat);
            db.SaveChanges();
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
