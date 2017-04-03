using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EventsPlannerMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Events", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            "Votes",                                              // Route name
            "{controller}/{action}/{memberID}/{ideaID}",                           // URL with parameters
            new { controller = "Home", action = "Index", memberID = "", ideaID = "" }  // Parameter defaults
        );
        }
    }
}
