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
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    [Authorize]
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CompetitionHelper helper = new CompetitionHelper();
            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            return View(view);
        }

        public ActionResult Dashboard(int? id)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CompetitionHelper helper = new CompetitionHelper();
            CompetitionViewModel view = helper.LoadCompetition(competition, user);


            return View(view);
        }

        private CompetitionViewModel loadCompetitionViewModel(Competition competition)
        {
            var user = User.Identity.GetUserId();
            
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                return null;
            }

            CompetitionHelper helper = new CompetitionHelper();
            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            return view;
        }

        public PartialViewResult Leaderboard(int? id)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return null;
            }

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            CompetitionViewModel view = loadCompetitionViewModel(competition);

            if (competition == null || view == null)
            {
                return null;
            }

            return PartialView("_LeaderboardPartial", view);
        }
        public ActionResult Leaders(int? id)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            CompetitionViewModel view = loadCompetitionViewModel(competition);

            if (competition == null || view == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(view);
        }

        public ActionResult Results(int id, int? raceId)
        {
            if (id == null || !Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.Find(id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CompetitionHelper helper = new CompetitionHelper();
            CompetitionViewModel view = helper.LoadCompetition(competition, user);

            int? _raceId = 0;
            if (raceId != null)
            {
                _raceId = raceId;
            }

            ViewBag.RaceId = _raceId;

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
                competition.RegistrationLocked = false;

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
            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
            PackAccess pa = new PackAccess();
            var user = User.Identity.GetUserId();

            if (ModelState.IsValid && !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                db.Entry(competition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackId = new SelectList(db.Packs, "Id", "Name", competition.PackId);
            return View(competition);
        }

        public ActionResult Lock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(competition);
        }

        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public ActionResult LockConfirmed(int id)
        {
            PackAccess pa = new PackAccess();
            var user = User.Identity.GetUserId();
            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Contributor))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                competition.RegistrationLocked = true;
                db.Entry(competition).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Competition", new {id = competition.Id});
            }

            return RedirectToAction("Details", "Competition", new { id = id });
        }
        // GET: /Competition/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = User.Identity.GetUserId();

            Competition competition = db.Competitions.FirstOrDefault(x => x.Id == id);
            PackAccess pa = new PackAccess();

            if (competition == null || !pa.CheckCompetitionMembership(competition.PackId, user, OwnershipType.Guest))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
