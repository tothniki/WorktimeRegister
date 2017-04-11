using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorktimeRegister
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //The routing engine will not try to process a request
            //that going to reach a real file on the file system
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Example if the url is:  /SomeExpression/someParameter
            //routes.MapRoute(
            //    name: "Cuisine",
            //    url: "cuisine/{name}", //the url starts with the word: cuisine
            //    defaults: new { controller = "Cuisine", action = "Search", name = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}