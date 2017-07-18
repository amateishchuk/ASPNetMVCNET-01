using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;

namespace WeatherApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Empty", "", new { controller = "Weather", action = "GetWeather" });

            routes.MapRoute("History", "History", new { controller = "WeatherHistory", action = "GetExtendedHistory" });

            routes.MapRoute("WeatherOnlyCity", "Weather/{city}", new { controller = "Weather", action = "GetWeather" }, new { city = new AlphaRouteConstraint() });

            routes.MapRoute("FullWeather", "Weather/{city}/{qtyDays}", new { controller = "Weather", action = "GetWeather" }, new { city = new AlphaRouteConstraint(), qtyDays = new RangeRouteConstraint(1, 16) });

            routes.MapRoute(
                name: "Default", 
                url: "{controller}/{action}/{id}", 
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
