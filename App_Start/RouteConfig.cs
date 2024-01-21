using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HilalComputersUnis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            



            routes.MapRoute(
               name: "ExcelExporter",
               url: "ExcelExporter",
               defaults: new { controller = "Home", action = "ExcelExporter" }
            );



            routes.MapRoute(
               name: "StaffDetails",
               url: "StaffDetails",
               defaults: new { controller = "Home", action = "StaffDetails" }
            );


            routes.MapRoute(
               name: "Punching2",
               url: "Punching2",
               defaults: new { controller = "Home", action = "Punching2" }
            );


            routes.MapRoute(
               name: "Punching",
               url: "Punching",
               defaults: new { controller = "Home", action = "Punching" }
            );

            routes.MapRoute(
               name: "Attendance",
               url: "Attendance",
               defaults: new { controller = "Home", action = "Attendance" }
            );

            routes.MapRoute(
              name: "AllUsers",
              url: "AllUsers",
              defaults: new { controller = "Home", action = "AllUsers"  }
          );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
