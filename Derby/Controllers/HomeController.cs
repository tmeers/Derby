using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Derby.Models;

namespace Derby.Controllers
{
	public class HomeController : Controller
	{
		DerbyDb _db = new DerbyDb();

        public ActionResult Index()
        {
            var packs = _db.Packs.ToList();
            foreach (var pack in packs)
            {
                pack.Dens = _db.Dens.Where(d => d.PackId == pack.Id).ToList();
                pack.Scouts = _db.Scouts.Where(s => s.PackId == pack.Id).ToList();
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
			if (_db != null)
			{
				_db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}