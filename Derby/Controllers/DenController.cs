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
    public class DenController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Den/
        public ActionResult Index()
        {
            return View(db.Dens.ToList());
        }

        // GET: /Den/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Den den = db.Dens.Find(id);
            if (den == null)
            {
                return HttpNotFound();
            }
            return View(den);
        }

        // GET: /Den/Create
        public ActionResult Create(int packId)
        {
            return View();
        }

        // POST: /Den/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,PackId")] Den den)
        {
            if (ModelState.IsValid)
            {
                db.Dens.Add(den);
                db.SaveChanges();
                return RedirectToAction("Details", "Pack", new {id = den.PackId} );
            }

            return View(den);
        }

        // GET: /Den/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Den den = db.Dens.Find(id);
            if (den == null)
            {
                return HttpNotFound();
            }
            return View(den);
        }

        // POST: /Den/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,PackId")] Den den)
        {
            if (ModelState.IsValid)
            {
                db.Entry(den).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(den);
        }

        // GET: /Den/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Den den = db.Dens.Find(id);
            if (den == null)
            {
                return HttpNotFound();
            }
            return View(den);
        }

        // POST: /Den/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Den den = db.Dens.Find(id);
            db.Dens.Remove(den);
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
