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
    public class MembershipController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Membership/
        public ActionResult Index()
        {
            return View(db.PackMemberships.ToList());
        }

        // GET: /Membership/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackMembership packmembership = db.PackMemberships.Find(id);
            if (packmembership == null)
            {
                return HttpNotFound();
            }
            return View(packmembership);
        }

        // GET: /Membership/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Membership/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PackId,UserId,AccessLevel")] PackMembership packmembership)
        {
            if (ModelState.IsValid)
            {
                db.PackMemberships.Add(packmembership);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packmembership);
        }

        // GET: /Membership/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackMembership packmembership = db.PackMemberships.Find(id);
            if (packmembership == null)
            {
                return HttpNotFound();
            }
            return View(packmembership);
        }

        // POST: /Membership/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,PackId,UserId,AccessLevel")] PackMembership packmembership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packmembership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packmembership);
        }

        // GET: /Membership/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackMembership packmembership = db.PackMemberships.Find(id);
            if (packmembership == null)
            {
                return HttpNotFound();
            }
            return View(packmembership);
        }

        // POST: /Membership/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackMembership packmembership = db.PackMemberships.Find(id);
            db.PackMemberships.Remove(packmembership);
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
