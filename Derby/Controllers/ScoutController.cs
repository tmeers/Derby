using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Derby.Models;

namespace Derby.Controllers
{
    [Authorize]
    public class ScoutController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Scout/
        public ActionResult Index(int packId)
        {
            var scouts = db.Scouts.Where(s => s.PackId == packId);
            return View(scouts.ToList());
        }

        // GET: /Scout/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scout scout = db.Scouts.Find(id);
            if (scout == null)
            {
                return HttpNotFound();
            }
            return View(scout);
        }

        public PartialViewResult AddScout(int id)
        {
            return PartialView("_AddScoutPartial", new Scout() { PackId = id });
        }
        // TODO: this sucks. 
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult AddScout([Bind(Include = "Id,Name,PackId")] Scout scout)
        {
            if (ModelState.IsValid)
            {
                saveScout(scout);
                return Json(scout);
            }

            return Json("false");
        }

        // GET: /Scout/Create
        public ActionResult Create(int packId)
        {
            ViewBag.PackId = packId;
            return View();
        }

        // POST: /Scout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,PackId")] Scout scout)
        {
            if (ModelState.IsValid)
            {
                saveScout(scout);
                return RedirectToAction("Index", new { packId = scout.PackId });
            }

            return View(scout);
        }

        private void saveScout(Scout scout)
        {
            scout.CreateDateTime = DateTime.Now;
            scout.Inactive = false;

            db.Scouts.Add(scout);
            db.SaveChanges();
        }

        // GET: /Scout/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scout scout = db.Scouts.Find(id);
            if (scout == null)
            {
                return HttpNotFound();
            }
            return View(scout);
        }

        // POST: /Scout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Inactive")] Scout scout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scout);
        }

        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scout scout = db.Scouts.Find(id);
            if (scout == null)
            {
                return HttpNotFound();
            }

            scout.Inactive = false;

            db.Entry(scout).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { packId = scout.PackId });
        }

        // GET: /Scout/Delete/5
        public ActionResult Inactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scout scout = db.Scouts.Find(id);
            if (scout == null)
            {
                return HttpNotFound();
            }
            return View(scout);
        }

        // POST: /Scout/Delete/5
        [HttpPost, ActionName("Inactivate")]
        [ValidateAntiForgeryToken]
        public ActionResult InactivateConfirmed(int id)
        {
            Scout scout = db.Scouts.Find(id);
            scout.Inactive = true;

            db.Entry(scout).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { packId = scout.PackId });
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
