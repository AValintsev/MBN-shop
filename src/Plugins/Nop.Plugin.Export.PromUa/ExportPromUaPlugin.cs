using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.IO;
using System.Linq;
using System.Web.Routing;

namespace Nop.Plugin.Export.PromUa
{
	public class ExportPromUaPlugin : BasePlugin, IMiscPlugin
	{
		private readonly ISettingService _settingService;
		private readonly IRepository<Language> _languageRepository;


		public ExportPromUaPlugin(
			ISettingService settingService,
			IRepository<Language> languageRepository)
		{
			_settingService = settingService;
			_languageRepository = languageRepository;
		}

		public override void Install()
		{
			//English language
			var languageEng = _languageRepository.Table.SingleOrDefault(l => l.Name == "English");

			if (languageEng != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Export.PromUa/App_Data/Translations"), "en_export-promua.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageEng, localesXml);
				}
			}

			//Russian language
			var languageRu = _languageRepository.Table.SingleOrDefault(l => l.Name == "Russian");

			if (languageRu != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Export.PromUa/App_Data/Translations"), "ru_export-promua.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageRu, localesXml);
				}
			}

			//Ukrainian language
			var languageUa = _languageRepository.Table.Single(l => l.Name == "Ukrainian");
			if (languageUa != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Export.PromUa/App_Data/Translations"), "ua_export-promua.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageUa, localesXml);
				}
			}

			base.Install();
		}

		/// <summary>
		/// Gets a route for provider configuration
		/// </summary>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "ExportPromUaXmlAll";
			controllerName = "ExportPromUa";
			routeValues = new RouteValueDictionary
			{
				{ "Namespaces", "Nop.Plugin.Export.PromUa.Controllers" },
				{ "area", null }
			};
		}

	}
}
