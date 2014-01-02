using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Derby.Models;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
	public class HomeController : Controller
	{
		DerbyDb db = new DerbyDb();

	    public ActionResult Index()
	    {
            var packs = new List<Pack>();
	        if (!Request.IsAuthenticated)
	        {
	            packs = db.Packs.ToList();
	        }
	        else
	        {
	            var user = User.Identity.GetUserId();
	            var membership = db.PackMemberships.Where(x => x.UserId == user).ToList();

                packs = db.Packs.Where(x => membership.Any(y => y.UserId == user)).ToList();
	        }

            foreach (var pack in packs)
            {
                pack.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList();
                pack.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();
            }

	        return View(packs);
        }

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (db != null)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}