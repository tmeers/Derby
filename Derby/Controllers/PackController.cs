using System;
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
    public class PackController : Controller
    {
        private DerbyDb _db = new DerbyDb();

        // GET: /Pack/
        public ActionResult Index(int? id)
        {
            var packs = _db.Packs.ToList();
            foreach (var pack in packs)
            {
                pack.Dens = _db.Dens.Where(d => d.PackId == pack.Id).ToList();
                pack.Scouts = _db.Scouts.Where(s => s.PackId == pack.Id).ToList();
            }

            return View(packs);
        }

        // GET: /Pack/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pack pack = _db.Packs.Find(id);
            if (pack == null)
            {
                return HttpNotFound();
            }

            pack.Dens = _db.Dens.Where(d => d.PackId == pack.Id).ToList();
            pack.Scouts = _db.Scouts.Where(s => s.PackId == pack.Id).ToList();

            return View(pack);
        }

        // GET: /Pack/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Pack/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Region")] Pack pack)
        {
            if (ModelState.IsValid)
            {
                pack.CreatedById = User.Identity.GetUserId();
                _db.Packs.Add(pack);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pack);
        }

        // GET: /Pack/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pack pack = _db.Packs.Find(id);
            if (pack == null)
            {
                return HttpNotFound();
            }
            return View(pack);
        }

        // POST: /Pack/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Region")] Pack pack)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(pack).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pack);
        }

        // GET: /Pack/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pack pack = _db.Packs.Find(id);
            if (pack == null)
            {
                return HttpNotFound();
            }
            return View(pack);
        }

        // POST: /Pack/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pack pack = _db.Packs.Find(id);
            _db.Packs.Remove(pack);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
