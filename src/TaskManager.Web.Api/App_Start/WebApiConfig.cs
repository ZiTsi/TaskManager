﻿using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using System.Web.Http.Tracing;
using TaskManager.Common.Logging;
using TaskManager.Web.Common;
using TaskManager.Web.Common.ErrorHandling;
using TaskManager.Web.Common.Routing;

namespace TaskManager.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // ---Auto Created-------------------------------------- \\
            //// Web API configuration and services

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }                
            //);
            // ----------------------------------------------------- \\

            var constraintsResolver = new DefaultInlineConstraintResolver();
            constraintsResolver.ConstraintMap.Add("apiVersionConstraint", typeof (ApiVersionConstraint));
            config.MapHttpAttributeRoutes(constraintsResolver);
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            // config.EnableSystemDiagnosticsTracing();
            config.Services.Replace(typeof(ITraceWriter),
                    new SimpleTraceWriter(WebContainerManager.Get<ILogManager>()));

            config.Services.Add(typeof(IExceptionLogger),
                    new SimpleExceptionLogger(WebContainerManager.Get<ILogManager>()));

            config.Services.Replace(typeof(IExceptionHandler), 
                    new GlobalExceptionHandler());
        }
    }
}
