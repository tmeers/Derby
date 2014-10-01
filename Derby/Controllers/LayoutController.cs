using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Derby.Infrastructure;
using Derby.Models;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
    public class LayoutController : Controller
    {
        private DerbyDb db = new DerbyDb();

        public PartialViewResult GetPacks()
        {
            var user = User.Identity.GetUserId();
            Infrastructure.PackAccess packs = new PackAccess();

            return PartialView("_PacksPartial", packs.BuildPackListing(user));
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