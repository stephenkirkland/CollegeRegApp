﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CollegeRegApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //The Welcome Page
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "RegApp", action = "Login" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "RegApp", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
