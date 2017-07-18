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

            routes.MapRoute("History", "History", new { controller = "WeatherHistory", action = "GetExtendedHistory" });
     
            routes.MapRoute("OnlyCity", "Weather/{city}", new { controller = "Weather", action = "GetWeather" },
                new { city = new AlphaRouteConstraint() });

            routes.MapRoute("CityAndQtyDays", "Weather/{city}/{qtyDays}",
                defaults: new { controller = "Weather", action = "GetWeather" },
                constraints: new { city = new AlphaRouteConstraint(), qtyDays = new RangeRouteConstraint(1, 16) });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Weather", action = "GetWeather", id = UrlParameter.Optional });
        }
    }
}
