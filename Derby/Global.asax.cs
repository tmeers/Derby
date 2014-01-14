using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Derby.Models;

namespace Derby
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //DerbyDb db = new DerbyDb();

        protected void Application_Start()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DerbyDb>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DerbyDb, Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
