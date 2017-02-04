using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;

namespace Derby
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default-Home",
				url: "",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

            routes.MapRoute(
                name: "Default-About",
                url: "about/",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login/",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Packs",
                url: "packs/",
                defaults: new { controller = "Pack", action = "Index" }
            );

            routes.MapRoute(
                name: "Pack-Edit",
                url: "pack/create/",
                defaults: new { controller = "Pack", action = "Create" }
            );

            routes.MapRoute(
                name: "Pack-Details",
                url: "pack/details/{id}",
                defaults: new { controller = "Pack", action = "Details" }
            );
            routes.MapRoute(
                name: "Pack-Info",
                url: "pack/{id}",
                defaults: new { controller = "Pack", action = "Info" }
            );

            routes.MapRoute(
                name: "Scout-Index",
                url: "scout/list/{packId}",
                defaults: new { controller = "Scout", action = "Index", packId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Scout-Create",
                url: "scout/create/{packId}",
                defaults: new { controller = "Scout", action = "Create", packId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Scout-QuickCreate",
                url: "scout/addscout/{id}",
                defaults: new { controller = "Scout", action = "AddScout", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Scout-Details",
                url: "scout/{id}",
                defaults: new { controller = "Scout", action = "Details", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Pack-Edit",
            //    url: "pack/edit/{id}",
            //    defaults: new { controller = "Pack", action = "Edit" }
            //);
            //routes.MapRoute(
            //    name: "Pack-Edit",
            //    url: "pack/delete/{id}",
            //    defaults: new { controller = "Pack", action = "Delete" }
            //);
            routes.MapRoute(
                name: "Den-Create",
                url: "den/create/{packId}",
                defaults: new { controller = "Den", action = "Create", packId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Competition-Create",
                url: "competition/create/{packId}",
                defaults: new { controller = "Competition", action = "Create" }
            );

            routes.MapRoute(
                name: "Competition-Details",
                url: "competition/details/{id}",
                defaults: new { controller = "Competition", action = "Details" }
            );

            routes.MapRoute(
                name: "Competition-Dashboard",
                url: "competition/{id}",
                defaults: new { controller = "Competition", action = "Dashboard" }
            );

            routes.MapRoute(
                name: "Competition-List",
                url: "competition/list/{packId}",
                defaults: new { controller = "Competition", action = "Index", packId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Competition-Results",
                url: "competition/results/{id}/{raceId}",
                defaults: new { controller = "Competition", action = "Results", raceId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Racer-Create",
                url: "racer/create/{competitionId}/{scoutId}",
                defaults: new { controller = "Racer", action = "Create" }
            );

            routes.MapRoute(
                name: "Race-Index",
                url: "race/{competitionId}",
                defaults: new { controller = "Race", action = "Index", competitionId = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Race-Create",
                url: "race/create/{competitionId}/{denId}",
                defaults: new { controller = "Race", action = "Create", competitionId = UrlParameter.Optional, denId = UrlParameter.Optional  }
            );

            routes.MapRoute(
                name: "Racer-Finals-Create",
                url: "racer/finals/{competitionId}/{raceId}",
                defaults: new { controller = "Racer", action = "Finals", competitionId = UrlParameter.Optional, denId = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Race-Generate",
            //    url: "race/generate/{competitionId}/{denId}",
            //    defaults: new { controller = "Race", action = "Generate" }
            //);

            routes.MapRoute(
                name: "Heat-Create",
                url: "heat/create/{raceId}",
                defaults: new { controller = "Heat", action = "Create" }
            );

            routes.MapRoute(
                name: "Invite-Accept",
                url: "invite/{id}",
                defaults: new { controller = "Heat", action = "Create" }
            );

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

		}
	}
}
