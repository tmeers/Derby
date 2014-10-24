﻿using System;
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
    public class InvitationController : Controller
    {
        private DerbyDb db = new DerbyDb();

        // GET: /Invitation/
        public ActionResult Index()
        {
            return View(db.PackInvitations.ToList());
        }

        // GET: /Invitation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackInvitation packinvitation = db.PackInvitations.Find(id);
            if (packinvitation == null)
            {
                return HttpNotFound();
            }
            return View(packinvitation);
        }

        public PartialViewResult CreateInvite()
        {
            PackInvitation packinvitation = new PackInvitation();
            return PartialView("_InvitePartial", packinvitation);
        }
        // GET: /Invitation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Invitation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CreatedDate,ExpiryOffset,Code,InvitedEmail,Accepted,AcceptedUserId")] PackInvitation packinvitation)
        {
            if (ModelState.IsValid)
            {
                db.PackInvitations.Add(packinvitation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packinvitation);
        }

        // GET: /Invitation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackInvitation packinvitation = db.PackInvitations.Find(id);
            if (packinvitation == null)
            {
                return HttpNotFound();
            }
            return View(packinvitation);
        }

        // POST: /Invitation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CreatedDate,ExpiryOffset,Code,InvitedEmail,Accepted,AcceptedUserId")] PackInvitation packinvitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packinvitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packinvitation);
        }

        // GET: /Invitation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackInvitation packinvitation = db.PackInvitations.Find(id);
            if (packinvitation == null)
            {
                return HttpNotFound();
            }
            return View(packinvitation);
        }

        // POST: /Invitation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackInvitation packinvitation = db.PackInvitations.Find(id);
            db.PackInvitations.Remove(packinvitation);
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
