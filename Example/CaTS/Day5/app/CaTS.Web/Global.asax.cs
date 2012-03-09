﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CaTS.Domain;
using CaTS.Init;
using SharpLite.Web.Mvc.ModelBinder;

namespace CaTS.Web
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start() {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolverInitializer.Initialize();
            // Since ContextRegistry requires an HTTP context, I want it to live in the web project.
            // But since CaTS.Init can't have a bi-directional dependency back to CaTS.Web, we need to pass the dependency.
            DependencyResolverInitializer.AddDependency(typeof(IContextRegistry), typeof(ContextRegistry));

            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
        }
    }
}