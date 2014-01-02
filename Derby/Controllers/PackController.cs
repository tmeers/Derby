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
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    public class PackController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Pack/
        public ActionResult Index(int? id)
        {
            var user = User.Identity.GetUserId();
            var memberships = db.PackMemberships.Where(x => x.UserId == user).ToList();

            var packs = new List<Pack>();
            foreach (var membership in memberships)
            {
                var pack = db.Packs.FirstOrDefault(x => x.Id == membership.PackId);
                if (pack != null)
                {
                    pack.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList(); 
                    pack.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();

                    packs.Add(pack);
                }
            }

            return View(packs);
        }

        // GET: /Pack/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var user = User.Identity.GetUserId();
            var memberships = db.PackMemberships.Where(x => x.UserId == user).ToList();

            Pack pack = db.Packs.FirstOrDefault(x => x.Id == id && x.CreatedById == user);
            if (pack == null)
            {
                return HttpNotFound();
            }

            pack.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList();
            pack.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();

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
                db.Packs.Add(pack);
                db.SaveChanges();
                
                var member = new PackMembership();
                member.PackId = pack.Id;
                member.UserId = pack.CreatedById;
                member.AccessLevel = OwnershipType.Owner;
                
                db.PackMemberships.Add(member);
                db.SaveChanges();

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
            Pack pack = db.Packs.Find(id);
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
                db.Entry(pack).State = EntityState.Modified;
                db.SaveChanges();
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
            Pack pack = db.Packs.Find(id);
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
            Pack pack = db.Packs.Find(id);
            db.Packs.Remove(pack);
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
