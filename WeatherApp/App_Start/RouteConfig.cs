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

            routes.MapRoute("OnlyCity", "{city}", new { controller = "Weather", action = "ShowWeather" },
                new { city = new AlphaRouteConstraint() });
            
            routes.MapRoute(
                name: "Default",
                url: "{city}/{qtyDays}",
                defaults: new { controller = "Weather", action = "ShowWeather" },
                constraints: new { city = new AlphaRouteConstraint(), qtyDays = new RangeRouteConstraint(1, 17)}
            );

            routes.MapRoute("Empty", "", new { controller = "Weather", action = "ShowWeather" });
        }
    }
}
