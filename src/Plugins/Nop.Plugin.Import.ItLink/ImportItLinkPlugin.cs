using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.IO;
using System.Web.Routing;
using System.Linq;
using Nop.Plugin.Import.ItLink.Data;

namespace Nop.Plugin.Import.ItLink
{
	public class ImportItLinkPlugin : BasePlugin, IMiscPlugin
	{
		private readonly ISettingService _settingService;
		private readonly IRepository<Language> _languageRepository;
		private readonly ImportObjectContext _context;

		public ImportItLinkPlugin(
			ISettingService settingService, 
			IRepository<Language> languageRepository,
			ImportObjectContext context)
		{
			_settingService = settingService;
			_languageRepository = languageRepository;
			_context = context;
		}

		#region Routes

		/// <summary>
		/// Gets a route for provider configuration
		/// </summary>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "ImportItLink";
			routeValues = new RouteValueDictionary
			{
				{ "Namespaces", "Nop.Plugin.Import.ItLink.Controllers" },
				{ "area", null }
			};
		}

		#endregion

		public override void Install()
		{
			_context.Install();

			InstallLocaleResources();

			var settings = new ImportItLinkSettings
			{
				XmlUrl = "http://price.it-link.com.ua/price/xml",
				Username = "Martyn12345",
				Password = "Martyn12345+"
			};

			_settingService.SaveSetting(settings);


			base.Install();
		}

		public override void Uninstall()
		{
			_settingService.DeleteSetting<ImportItLinkSettings>();

			_context.Uninstall();

			base.Uninstall();
		}

		protected virtual void InstallLocaleResources()
		{
			//English language
			var languageEng = _languageRepository.Table.SingleOrDefault(l => l.Name == "English");

			if (languageEng != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Import.ItLink/App_Data/Translations"), "en_import-ItLink.xml", SearchOption.TopDirectoryOnly))
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
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Import.ItLink/App_Data/Translations"), "ru_import-ItLink.xml", SearchOption.TopDirectoryOnly))
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
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Import.ItLink/App_Data/Translations"), "ua_import-ItLink.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageUa, localesXml);
				}
			}
		}
	}
}
