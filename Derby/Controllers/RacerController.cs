using System;
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

namespace Derby.Controllers
{
    [Authorize]
    public class RacerController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Racer/
        public ActionResult Index()
        {
            return View(db.Racers.ToList());
        }

        // GET: /Racer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racer racer = db.Racers.Find(id);
            if (racer == null)
            {
                return HttpNotFound();
            }
            return View(racer);
        }

        // GET: /Racer/Create
        public ActionResult Create(int competitionId, int scoutId)
        {
            Scout _scout = db.Scouts.Find(scoutId);
            if (_scout == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var packId = db.Competitions.FirstOrDefault(x => x.Id == competitionId).PackId;
            CreateRacerViewModel view = new CreateRacerViewModel();
            view.Dens = db.Dens.Where(x => x.PackId == packId).ToList();

            view.CompetitionId = competitionId;
            view.ScoutId = scoutId;
            view.ScoutName = _scout.Name;

            return View(view);
        }

        // POST: /Racer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CarNumber,Weight,DenId,ScoutId,CompetitionId")] Racer racer)
        {
            if (ModelState.IsValid)
            {
                db.Racers.Add(racer);
                db.SaveChanges();

                return RedirectToAction("details", "Competition", new {id = racer.CompetitionId});
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Finals(int? competitionId, int? raceId)
        {
            if (competitionId == null || raceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == competitionId);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CompetitionHelper helper = new CompetitionHelper();
            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            return View(view);


            //Scout _scout = db.Scouts.Find(scoutId);
            //if (_scout == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //var packId = db.Competitions.FirstOrDefault(x => x.Id == competitionId).PackId;
            //CreateRacerViewModel view = new CreateRacerViewModel();
            //view.Dens = db.Dens.Where(x => x.PackId == packId).ToList();

            //view.CompetitionId = competitionId;
            //view.ScoutId = scoutId;
            //view.ScoutName = _scout.Name;

            //return View(view);
        }

        //// POST: /Racer/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Final([Bind(Include = "Id,CarNumber,Weight,DenId,ScoutId,CompetitionId")] Racer racer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Racers.Add(racer);
        //        db.SaveChanges();

        //        return RedirectToAction("details", "Competition", new { id = racer.CompetitionId });
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        // GET: /Racer/Edit/5
        public ActionResult Edit(int id)
        {
            Racer racer = db.Racers.Find(id);
            if (racer == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var user = User.Identity.GetUserId();

            PackAccess pa = new PackAccess();
            if (!pa.CheckScoutMembership(racer.ScoutId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }


            Scout _scout = db.Scouts.Find(racer.ScoutId);
            if (_scout == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var packId = db.Competitions.FirstOrDefault(x => x.Id == racer.CompetitionId).PackId;
            CreateRacerViewModel view = new CreateRacerViewModel();
            view.Dens = db.Dens.Where(x => x.PackId == packId).ToList();

            view.CompetitionId = racer.CompetitionId;
            view.ScoutId = racer.ScoutId;
            view.ScoutName = _scout.Name;
            view.Id = racer.Id;
            view.CarNumber = racer.CarNumber;
            view.Weight = racer.Weight;
            view.DenId = racer.DenId;

            return View(view);
        }

        // POST: /Racer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CarNumber,Weight,DenId,ScoutId,CompetitionId")] Racer racer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(racer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("details", "Competition", new { id = racer.CompetitionId });
            }
            return View(racer);
        }

        // GET: /Racer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racer racer = db.Racers.Find(id);
            if (racer == null)
            {
                return HttpNotFound();
            }
            return View(racer);
        }

        // POST: /Racer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Racer racer = db.Racers.Find(id);
            db.Racers.Remove(racer);
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
