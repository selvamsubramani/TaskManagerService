using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TaskManager.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "taskmanagerservice/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "taskmanagerservice/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
