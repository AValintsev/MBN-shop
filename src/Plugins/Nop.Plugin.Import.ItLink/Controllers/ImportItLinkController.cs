using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Import.ItLink.Models;
using Nop.Plugin.Import.ItLink.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Net;
using System.Web.Mvc;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Controllers
{
	[AdminAuthorize]
	public class ImportItLinkController : BasePluginController
	{
		#region Fields

		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;

		private readonly IStoreService _storeService;

		private readonly ISettingService _settingService;
		private readonly VendorSettings _vendorSettings;

		private readonly ILanguageService _languageService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;

		private readonly IPermissionService _permissionService;

		private readonly IXmlToXlsConverter _xmlToXlsxConverter;
		private readonly IImportManager _importManager;
		private readonly ICategoryService _categoryService;


		//private readonly IXmlToXlsConverter _xmlConverter;
		//private readonly IXmlImporter _xmlImporter;


		#endregion

		public ImportItLinkController(
			IWorkContext workContext,
			IStoreService storeService,
			VendorSettings vendorSettings,
			ISettingService settingService,
			IImportManager importManager,
			IStoreContext storeContext,
			ILanguageService languageService,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService,
			IPermissionService permissionService,
			ICategoryService categoryService,
			IXmlToXlsConverter xmlToXlsxConverter
			)
		{
			this._workContext = workContext;
			this._storeContext = storeContext;

			this._settingService = settingService;
			this._vendorSettings = vendorSettings;

			this._storeService = storeService;

			this._languageService = languageService;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;

			this._permissionService = permissionService;

			this._importManager = importManager;
			this._categoryService = categoryService;

			this._xmlToXlsxConverter = xmlToXlsxConverter;
		}

		[ChildActionOnly]
		public ActionResult Configure()
		{
			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

			var viewModel = new ConfigurationModel
			{
				XmlUrl = settings.XmlUrl,
				Username = settings.Username,
				Password = settings.Password
			};

			return View("~/Plugins/Import.ItLink/Views/Configure.cshtml", viewModel);
		}

		[HttpPost]
		[ChildActionOnly]
		[FormValueRequired("save")]
		public ActionResult Configure(ConfigurationModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return Configure();
			}

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

			//save settings
			settings.XmlUrl = viewModel.XmlUrl;
			settings.Username = viewModel.Username;
			settings.Password = viewModel.Password;

			_settingService.SaveSetting(settings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

		[HttpPost, ActionName("Configure")]
		[ChildActionOnly]
		[FormValueRequired("ImportFromItLink")]
		public virtual ActionResult ImportFromItLink()
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
			{
				ErrorNotification("Access Denied");
				return Configure();
			}

			//a vendor can not import products
			if (_workContext.CurrentVendor != null && !_vendorSettings.AllowVendorsToImportProducts)
			{
				ErrorNotification("Access Denied");
				return Configure();
			}

			try
			{
				var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
				var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

				//Create the credentials
				var credentials = new NetworkCredential(settings.Username, settings.Password);
				XmlUrlResolver xmlResolver = new XmlUrlResolver
				{
					Credentials = credentials
				};

				XmlReader xmlReader = XmlTextReader.Create(
					settings.XmlUrl,
					new XmlReaderSettings
					{
						XmlResolver = xmlResolver,
						DtdProcessing = DtdProcessing.Ignore
					});

				var xmlDoc = new XmlDocument();
				xmlDoc.Load(xmlReader);

				var file = _xmlToXlsxConverter.XmlToXlsx(xmlDoc);

				if (file != null && file.Length > 0)
				{
					_importManager.ImportProductsFromXlsx(file);
				}
				else
				{
					ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
					return Configure();

				}
				SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Imported"));
			}
			catch (Exception exc)
			{
				ErrorNotification(exc);
			}

			return Configure();

		}

		/// <summary>
		/// Access denied view
		/// </summary>
		/// <returns>Access denied view</returns>
		protected virtual ActionResult AccessDeniedView()
		{
			return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
		}
	}
}
