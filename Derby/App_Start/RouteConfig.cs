using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

		}
	}
}
