using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Models;

namespace Derby.Controllers
{
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
            if (!races.Any())
            {
                return HttpNotFound();
            }

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
        public ActionResult Create()
        {
            return View();
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
