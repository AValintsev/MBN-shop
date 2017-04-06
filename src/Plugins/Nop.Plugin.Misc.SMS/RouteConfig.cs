using Nop.Plugin.Misc.SMS.Infrustructure;
using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Misc.SMS
{
    class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get { return 0; }
        }
        public void RegisterRoutes(RouteCollection routes)
        {

            ViewEngines.Engines.Insert(0, new CustomViewEngine());

            routes.MapRoute("Plugin.Misc.SMS.Manage",
                "SMS/Manage",
                new { controller = "SMS", action = "Manage" },
                new[] {"Nop.Plugin.Misc.SMS.Controllers"});

            routes.MapRoute("Plugin.Misc.SMS.Configure",
                "SMS/Configure",
                new { controller = "SMS", action = "Configure" },
                new[] { "Nop.Plugin.Misc.SMS.Controllers" });
            

        }
    }
}
