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
                name: "Pack-Details",
                url: "pack/{id}",
                defaults: new { controller = "Pack", action = "Details" }
            );

            routes.MapRoute(
                name: "Den-Create",
                url: "den/create/{packId}",
                defaults: new { controller = "Den", action = "Create", packId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Competition-Details",
                url: "competition/{id}",
                defaults: new { controller = "Competition", action = "Details" }
            );

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

		}
	}
}
