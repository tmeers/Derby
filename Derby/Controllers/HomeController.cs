using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Derby.Infrastructure;
using Derby.Models;
using Derby.ViewModels;
using Microsoft.AspNet.Identity;

namespace Derby.Controllers
{
	public class HomeController : Controller
	{
		DerbyDb db = new DerbyDb();

	    public ActionResult Index()
	    {
            var user = User.Identity.GetUserId();
            Infrastructure.PackAccess packs = new PackAccess();

            return View(packs.BuildPackListing(user));
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