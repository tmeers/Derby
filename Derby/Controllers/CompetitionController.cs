using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Models;
using Derby.ViewModels;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    public class CompetitionController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Competition/List/
        public ActionResult Index(int packId)
        {
            var competitions = db.Competitions.Where(c => c.PackId == packId);
            return View(competitions.ToList());
        }

        // GET: /Competition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            if (competition == null)
            {
                return HttpNotFound();
            }

            CompetitionViewModel view = new CompetitionViewModel(competition);
            view.Pack = db.Packs.FirstOrDefault(p => p.Id == view.PackId);


            var _racers = db.Racers.Where(r => r.CompetitionId == view.Id).ToList();
            var _scouts = db.Scouts.Where(r => r.PackId == view.PackId).ToList();

            foreach (var den in db.Dens.Where(p => p.PackId == competition.PackId))
            {
                var _den = den;
                //var denView = new DenCompetitionViewModel(_den);
                foreach (var racer in _racers.Where(d => d.DenId == _den.Id))
                {
                    var racerView = new RacerViewModel(racer);
                    racerView.Den = _den;
                    racerView.Scout = _scouts.FirstOrDefault(s => s.Id == racer.ScoutId);

                    view.Racers.Add(racerView);
                    //denView.Racers.Add(racerView);
                }

                view.Dens.Add(_den);
            }

            return View(view);
        }

        // GET: /Competition/Create
        public ActionResult Create(int packId)
        {
            ViewBag.PackId = packId; //new SelectList(db.Packs, "Id", "Name");
            return View();
        }

        // POST: /Competition/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Location,RaceType,EventDate,LaneCount")] Competition competition, int packId)
        {
            if (ModelState.IsValid)
            {
                competition.CreatedById = User.Identity.GetUserId();
                competition.CreatedDate = DateTime.Now;
                competition.PackId = packId;

                db.Competitions.Add(competition);
                db.SaveChanges();
                return RedirectToAction("Details", "Pack", new { id = competition.PackId });
            }

            ViewBag.PackId = new SelectList(db.Packs, "Id", "Name", competition.PackId);
            return View(competition);
        }

        // GET: /Competition/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackId = new SelectList(db.Packs, "Id", "Name", competition.PackId);
            return View(competition);
        }

        // POST: /Competition/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PackId,Title,Location,RaceType,EventDate,LaneCount")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackId = new SelectList(db.Packs, "Id", "Name", competition.PackId);
            return View(competition);
        }

        // GET: /Competition/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competition competition = db.Competitions.Find(id);
            if (competition == null)
            {
                return HttpNotFound();
            }
            return View(competition);
        }

        // POST: /Competition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competition competition = db.Competitions.Find(id);
            db.Competitions.Remove(competition);
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
