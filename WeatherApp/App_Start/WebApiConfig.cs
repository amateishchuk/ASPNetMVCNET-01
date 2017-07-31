using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing.Constraints;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Web API configuration and services
        var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
        config.EnableCors(cors);
        config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();        
        config.Formatters.Remove(config.Formatters.XmlFormatter);
        

        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "WeatherRoute",
            routeTemplate: "api/Weather/{city}/{qtyDays}",
            defaults: new { controller = "Weather" },
            constraints: new {
                city = new AlphaRouteConstraint(),
                qtyDays = new RangeRouteConstraint(1, 16)
            }
        );


        config.Routes.MapHttpRoute(
            name: "DefaultApiString",
            routeTemplate: "api/{controller}/{name}",
            defaults: new { name = RouteParameter.Optional },
            constraints: new { name = new AlphaRouteConstraint() }
        );

        config.Routes.MapHttpRoute(
            name: "DefaultApiId",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional },
            constraints: new { id = new MinRouteConstraint(1) }
        );
    }
}