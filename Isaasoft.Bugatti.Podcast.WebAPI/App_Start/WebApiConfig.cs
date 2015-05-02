using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Isaasoft.Bugatti.Podcast.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register( HttpConfiguration config )
        {
            // Web API configuration and services

            var cors = new EnableCorsAttribute( "*", "*", "*" );

            config.EnableCors( cors );

            // Web API routes

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "RssItemApi",
                routeTemplate: "rss/{id}/item",
                defaults: new { controller = "RssItem", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "RssItemDetailApi",
                routeTemplate: "rss/{id}/detail",
                defaults: new { controller = "RssItemDetail", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
