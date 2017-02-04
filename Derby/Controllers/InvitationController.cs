using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Derby.Infrastructure;
using Derby.Models;
using Derby.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

        public PartialViewResult SentInvitations()
        {
            string user = User.Identity.GetUserId();
            List<SentInvitationViewModel> packinvitation = new List<SentInvitationViewModel>();
            packinvitation.AddRange((from invite in db.PackInvitations
                                        where invite.InvitedBy.Id == user
                                    select new SentInvitationViewModel
                                    {
                                        Id = invite.Id,
                                        CreatedDate = invite.CreatedDate,
                                        ExpiryOffset = invite.ExpiryOffset,
                                        InvitedBy = invite.InvitedBy,
                                        InvitedEmail = invite.InvitedEmail,
                                        Accepted = invite.Accepted,
                                        Pack = invite.Pack,
                                        Status = invite.Status
                                    }).ToList());

            return PartialView("_InviteListPartial", packinvitation);
        }

        public PartialViewResult Create()
        {
            PackInvitationViewModel packinvitation = new PackInvitationViewModel(User.Identity.GetUserId());
            return PartialView("_InvitePartial", packinvitation);
        }

        // POST: /Invitation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvitedEmail,PackId")] PackInvitationViewModel invite)
        {
            if (ModelState.IsValid)
            {
                PackInvitation _invite = new PackInvitation();
                UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                // TODO Will need to move this into an admin setting at some point
                _invite.ExpiryOffset = 365;
                _invite.Accepted = false;
                _invite.AcceptedUserId = string.Empty;
                _invite.CreatedDate = DateTime.Now;
                _invite.Code = Infrastructure.GuidEncoder.Encode(Guid.NewGuid().ToString());
                _invite.InvitedBy = manager.FindById(User.Identity.GetUserId());
                //string _id = ViewBag.PackId.ToString();
                //var date = invite.PackList;
                _invite.Pack = db.Packs.FirstOrDefault(x => x.Id == invite.PackId);
                _invite.InvitedEmail = invite.InvitedEmail;
                _invite.Status = EmailStatus.Pending;

                db.PackInvitations.Add(_invite);
                db.SaveChanges();

                if (Services.Email.EmailSender.Send(_invite))
                {
                    return RedirectToAction("Index", "Membership");
                }
            }

            return View(invite);
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
