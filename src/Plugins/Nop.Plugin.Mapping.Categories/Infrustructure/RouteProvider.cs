using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Mapping.Categories.Infrustructure
{
	public partial class RouteProvider : IRouteProvider
	{
		public void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute(
				"Plugins.MappingCategories.ExportPromUaXmlAll",
				"Plugins/MappingCategories/ExportPromUaXmlAll",
				new { controller = "MappingCategories", action = "ExportPromUaXmlAll" },
				new[] { "Nop.Plugin.Mapping.Categories.Controllers" }
			);

			routes.MapRoute(
				"Plugins.MappingCategories.ExportPromUaXmlSelected",
				"Plugins/MappingCategories/ExportPromUaXmlSelected",
				new { controller = "MappingCategories", action = "ExportPromUaXmlSelected" },
				new[] { "Nop.Plugin.Mapping.Categories.Controllers" }
			);

		}
		public int Priority
		{
			get
			{
				return 0;
			}
		}
	}
}
