using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WeatherApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("OnlyCity", "{city}", new { controller = "Weather", action = "ShowWeather" });

            routes.MapRoute(
                name: "Default",
                url: "{city}/{qtyDays}",
                defaults: new { controller = "Weather", action = "ShowWeather" }
            );
            routes.MapRoute("Empty", "", new { controller = "Weather", action = "ShowWeather" });
        }
    }
}
