using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Plugins;
using Nop.Plugin.Widgets.NivoSliderLocalized.Data;
using Nop.Plugin.Widgets.NivoSliderLocalized.Domain;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Routing;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.Widgets.NivoSliderLocalized
{
	public class NivoSliderPluginLocalizedPlugin : BasePlugin, IWidgetPlugin
	{
		private readonly SliderObjectContext _context;
		private readonly IRepository<Language> _languageRepository;
		private readonly IRepository<SliderItem> _sliderItemRepository;
		private readonly IPictureService _pictureService;
		private readonly ISettingService _settingService;
		private readonly IWebHelper _webHelper;

		public NivoSliderPluginLocalizedPlugin(
			SliderObjectContext context,
			IRepository<SliderItem> sliderItemRepository,
			IRepository<Language> languageRepository,
			IPictureService pictureService,
			ISettingService settingService,
			IWebHelper webHelper)
		{
			this._context = context;
			this._languageRepository = languageRepository;
			this._sliderItemRepository = sliderItemRepository;
			this._pictureService = pictureService;
			this._settingService = settingService;
			this._webHelper = webHelper;
		}

		/// <summary>
		/// Gets widget zones where this widget should be rendered
		/// </summary>
		/// <returns>Widget zones</returns>
		public IList<string> GetWidgetZones()
		{
			return new List<string> { "home_page_top" };
		}

		/// <summary>
		/// Gets a route for provider configuration
		/// </summary>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "WidgetsNivoSliderLocalized";
			routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.NivoSliderLocalized.Controllers" }, { "area", null } };
		}

		/// <summary>
		/// Gets a route for displaying widget
		/// </summary>
		/// <param name="widgetZone">Widget zone where it's displayed</param>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "PublicInfo";
			controllerName = "WidgetsNivoSliderLocalized";
			routeValues = new RouteValueDictionary
			{
				{"Namespaces", "Nop.Plugin.Widgets.NivoSliderLocalized.Controllers"},
				{"area", null},
				{"widgetZone", widgetZone}
			};
		}

		/// <summary>
		/// Install plugin
		/// </summary>
		public override void Install()
		{
			_context.Install();

			//pictures
			var sampleImagesPath = CommonHelper.MapPath("~/Plugins/Widgets.NivoSliderLocalized/Content/nivoslider/sample-images/");

			var sliderItem1 = new SliderItem
			{
				PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), MimeTypes.ImagePJpeg, "banner_1")
					.Id.ToString(),
				Text = "",
				Link = _webHelper.GetStoreLocation(false),
			};

			var sliderItem2 = new SliderItem
			{
				PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), MimeTypes.ImagePJpeg, "banner_1")
					.Id.ToString(),
				Text = "",
				Link = _webHelper.GetStoreLocation(false),
			};

			var sliderItem3 = new SliderItem
			{
				PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), MimeTypes.ImagePJpeg, "banner_1")
					.Id.ToString(),
				Text = "",
				Link = _webHelper.GetStoreLocation(false),
			};

			_context.Set<SliderItem>().Add(sliderItem1);
			_context.Set<SliderItem>().Add(sliderItem2);
			_context.Set<SliderItem>().Add(sliderItem3);

			#region Plugin Locale resoureces

			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Picture", "Slide");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.RemoveSlide", "Remove slide");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.AddNewSlide", "Add new slide");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Picture.Hint", "Upload picture.");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Text", "Comment");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Text.Hint", "Enter comment for picture. Leave empty if you don't want to display any text.");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Link", "URL");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Link.Hint", "Enter URL. Leave empty if you don't want this picture to be clickable.");
			
			//Russian language
			var languageRu = _languageRepository.Table.Single(l => l.Name == "Russian");

			//save resources
			foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Widgets.NivoSliderLocalized/App_Data/Translations/"), "ru_language_pack.xml", SearchOption.TopDirectoryOnly))
			{
				var localesXml = File.ReadAllText(filePath);
				var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
				localizationService.ImportResourcesFromXml(languageRu, localesXml);
			}

			//Ukrainian language
			var languageUa = _languageRepository.Table.Single(l => l.Name == "Ukrainian");

			//save resources
			foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Widgets.NivoSliderLocalized/App_Data/Translations/"), "ua_language_pack.xml", SearchOption.TopDirectoryOnly))
			{
				var localesXml = File.ReadAllText(filePath);
				var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
				localizationService.ImportResourcesFromXml(languageUa, localesXml);
			}

			#endregion

			_context.SaveChanges();

			base.Install();
		}

		/// <summary>
		/// Uninstall plugin
		/// </summary>
		public override void Uninstall()
		{
			var entities = _sliderItemRepository.Table.ToList();

			entities.ForEach(entity =>
			{
				if (entity != null)
				{
					var picutre = _pictureService.GetPictureById(int.Parse(entity.PictureId));
					if (picutre != null)
					{
						_pictureService.DeletePicture(picutre);
					}

					_sliderItemRepository.Delete(entity);
				}
			});

			//locales
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Picture");
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Picture.Hint");
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Text");
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Text.Hint");
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Link");
			this.DeletePluginLocaleResource("Plugins.Widgets.NivoSliderLocalized.Link.Hint");

			_context.Uninstall();

			base.Uninstall();
		}
	}
}
