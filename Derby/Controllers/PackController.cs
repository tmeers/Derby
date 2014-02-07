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
    public class PackController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Pack/
        public ActionResult Index(int? id)
        {
            var user = User.Identity.GetUserId();
            Infrastructure.PackAccess packs = new PackAccess();

            return View(packs.BuildPackListing(user));
        }

        // GET: /Pack/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var user = User.Identity.GetUserId();
            PackAccess pa = new PackAccess();
            PackViewModel pack = pa.OpenPack(id, user, OwnershipType.Contributor);

            if (pack == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //LoadPackData(pack);
            return View(pack);
        }

        public ActionResult Info(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var user = User.Identity.GetUserId();
            PackAccess pa = new PackAccess();
            PackViewModel pack = pa.OpenPack(id, user, OwnershipType.Guest);

            if (pack == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            //LoadPackData(pack);

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
            if (ModelState.IsValid && Request.IsAuthenticated)
            {
                pack.CreatedById = User.Identity.GetUserId();
                pack.CreateDateTime = DateTime.Now;
                db.Packs.Add(pack);
                db.SaveChanges();
                
                var member = new PackMembership();
                member.PackId = pack.Id;
                member.UserId = pack.CreatedById;
                member.AccessLevel = OwnershipType.Owner;
                
                db.PackMemberships.Add(member);
                db.SaveChanges();

                db.Dens.Add(new Den() { Name = "Tigers", CreatedDateTime = DateTime.Now, PackId = pack.Id, InactiveStatus = false, CanCompeteInFinals = true, IsSystemPlaceholder = false, LogoPath = "/content/tiger.jpg"});
                db.Dens.Add(new Den() { Name = "Wolves", CreatedDateTime = DateTime.Now, PackId = pack.Id, InactiveStatus = false, CanCompeteInFinals = true, IsSystemPlaceholder = false, LogoPath = "/content/wolf.jpg" });
                db.Dens.Add(new Den() { Name = "Bears", CreatedDateTime = DateTime.Now, PackId = pack.Id, InactiveStatus = false, CanCompeteInFinals = true, IsSystemPlaceholder = false, LogoPath = "/content/bear.jpg" });
                db.Dens.Add(new Den() { Name = "Weblows", CreatedDateTime = DateTime.Now, PackId = pack.Id, InactiveStatus = false, CanCompeteInFinals = true, IsSystemPlaceholder = false, LogoPath = "/content/webelos.png" });
                db.Dens.Add(new Den() { Name = "Finals", CreatedDateTime = DateTime.Now, PackId = pack.Id, InactiveStatus = false, CanCompeteInFinals = true, IsSystemPlaceholder = true, LogoPath = "" });

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

            var user = User.Identity.GetUserId();
            PackAccess pa = new PackAccess();
            PackViewModel pack = pa.OpenPack(id, user, OwnershipType.Contributor);

            if (pack == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
            if (ModelState.IsValid && Request.IsAuthenticated)
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

            var user = User.Identity.GetUserId();
            PackAccess pa = new PackAccess();
            PackViewModel pack = pa.OpenPack(id, user, OwnershipType.Owner);

            if (pack == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
