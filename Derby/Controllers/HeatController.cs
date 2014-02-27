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
        public ActionResult Create(int raceId, string returnPath)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["returnPath"]))
            {
                TempData["returnPath"] = Request.QueryString["returnPath"];
            }
            
            return View(LoadCreateView(raceId));
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
            RunHeatViewModel runHeatView = new RunHeatViewModel(heat);

            runHeatView.CurrentHeats = db.Heats.Where(x => x.RaceId == _race.Id).ToList();
            List<Contestant> _contestants = db.Contestants.Where(x => x.HeatId == id).ToList();
            foreach (var contestant in _contestants)
            {
                 
            }
            runHeatView.Competition = db.Competitions.Find(_race.CompetitionId);
            runHeatView.CompetitionId = runHeatView.Competition.Id;
            runHeatView.HeatsNeeded = Infrastructure.HeatGenerator.GenerateHeatCount(runHeatView.Competition.LaneCount,
                _race.Racers.Count);
            return View(runHeatView);
        }

        // GET: /Heat/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: /Heat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,RaceId")] Heat heat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(heat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(heat);
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
