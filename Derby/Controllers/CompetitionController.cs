using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Models;

namespace Derby.Controllers
{
    public class CompetitionController : Controller
    {
        DerbyDb _db = new DerbyDb();

        //
        // GET: /Competition/
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Competition _competition = _db.Competitions.Find(id);

            if (_competition == null)
            {
                return HttpNotFound();
            }

            _competition.Pack = _db.Packs.Find(id);
            _competition.Pack.Dens = _db.Dens.Where(d => d.PackId == _competition.Pack.Id).ToList();
            _competition.Pack.Scouts = _db.Scouts.Where(s => s.PackId == _competition.Pack.Id).ToList();

            return View(_competition);
        }

        //
        // GET: /Competition/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Competition/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Competition/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
