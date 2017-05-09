using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Infrustructure
{
	public partial class RouteProvider : IRouteProvider
	{
		public void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute(
				"Plugins.WidgetsNivoSliderLocalized.DeleteSlide",
				"Plugins/WidgetsNivoSliderLocalized/DeleteSlide",
				new { controller = "WidgetsNivoSliderLocalized", action = "DeleteSlide" },
				new[] { "Nop.Plugin.Widgets.NivoSliderLocalized.Controllers" }
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
