﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Models;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    public class CompetitionController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Competition/
        public ActionResult Index()
        {
            var competitions = db.Competitions.Include(c => c.Pack);
            return View(competitions.ToList());
        }

        // GET: /Competition/Details/5
        public ActionResult Details(int? id)
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

        // GET: /Competition/Create
        public ActionResult Create()
        {
            ViewBag.PackId = new SelectList(db.Packs, "Id", "Name");
            return View();
        }

        // POST: /Competition/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PackId,Title,Location,RaceType,CreatedDate,EventDate")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                competition.CreatedById = User.Identity.GetUserId();
                db.Competitions.Add(competition);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include="Id,PackId,Title,Location,RaceType,CreatedDate,EventDate,CreatedById")] Competition competition)
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