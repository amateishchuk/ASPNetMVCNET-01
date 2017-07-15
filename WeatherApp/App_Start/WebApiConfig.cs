using System.Web.Http;
using System.Web.Http.Routing.Constraints;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Web API configuration and services

        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "WeatherRoute",
            routeTemplate: "api/Weather/{city}/{qtyDays}",
            defaults: new { controller = "Weather" },
            constraints: new { city = new AlphaRouteConstraint(), qtyDays = new RangeRouteConstraint(1, 16) }
        );

        config.Routes.MapHttpRoute(
            name: "DefaultApiId",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional },
            constraints: new { id = new IntRouteConstraint() }
        );

        config.Routes.MapHttpRoute(
            name: "DefaultApiString",
            routeTemplate: "api/{controller}/{name}",
            defaults: new { name = RouteParameter.Optional },
            constraints: new { name = new AlphaRouteConstraint() }
        );

        config.Formatters.Remove(config.Formatters.XmlFormatter);
    }
}