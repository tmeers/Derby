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
	            foreach (var pack in packs)
	            {
	                pack.Dens = db.Dens.Where(d => d.PackId == pack.Id).ToList();
	                pack.Scouts = db.Scouts.Where(s => s.PackId == pack.Id).ToList();
	            }
	        }
	        else
	        {
	            var user = User.Identity.GetUserId();
	            var memberships = db.PackMemberships.Where(x => x.UserId == user).ToList();

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