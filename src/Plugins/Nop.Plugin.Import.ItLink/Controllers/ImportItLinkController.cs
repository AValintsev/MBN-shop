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
using Nop.Plugin.Import.ItLink.Domain;
using System.Collections.Generic;
using Nop.Core.Data;
using System.Linq;
using Nop.Plugin.Import.ItLink.Data;

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

		private readonly IRepository<InternalToExternal> _categoriesMappingrepository;

		private readonly ImportObjectContext _context;



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
			IRepository<InternalToExternal> repos,
			IXmlToXlsConverter xmlToXlsxConverter,
			ImportObjectContext context
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

			this._categoriesMappingrepository = repos;

			this._context = context;

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
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

			if (!settings.AreCategoriesMapped)
			{
				ErrorNotification(_localizationService.GetResource("Plugins.Imort.ItLink.CategoriesMapping.Alert.PleaseMapCategories"));
				return Configure();
			}
			try
			{
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

		private Dictionary<string, string> DownloadExternalCategories()
		{
			Dictionary<string, string> result = new Dictionary<string, string>();

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

				//В этом варианте считывается xml документ
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(xmlReader);

				foreach (XmlNode category in xmlDoc.GetElementsByTagName("category"))
				{
					result.Add(category.Attributes["id"].Value, category.InnerText);

				}

				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public PartialViewResult CategoriesMapping()
		{
			List<CategoriesMappingViewModel> viewModel = new List<CategoriesMappingViewModel>();

			Dictionary<string, string> externalCategories = DownloadExternalCategories();

			var Internalcategories = _categoryService.GetAllCategories().ToList();

			foreach (var extCat in externalCategories)
			{
				viewModel.Add(new CategoriesMappingViewModel
				{
					ExternalId = extCat.Key,
					ExternalName = extCat.Value,
				});


			}
				viewModel.ForEach(item =>
				{
					item.InternalCategoriesSelectList.Add(new SelectListItem
					{
						Text = "",
						Value = null,
						Selected = true

					});
					Internalcategories.ForEach(cat => item.InternalCategoriesSelectList.Add(new SelectListItem
					{
						Text = cat.Name,
						Value = cat.Id.ToString()
					}));
				});


			return PartialView("~/Plugins/Import.ItLink/Views/CategoiresMapping.cshtml", viewModel);
		}

		[ChildActionOnly]
		public PartialViewResult EditCategoriesMapped()
		{
			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

			List<CategoriesMappingViewModel> viewModel = new List<CategoriesMappingViewModel>();

			if (settings.AreCategoriesMapped)
			{

				List<InternalToExternal> entities = _categoriesMappingrepository.Table.ToList();


				var Internalcategories = _categoryService.GetAllCategories().ToList();


				entities.ForEach(e =>
				{
					var internalCategory = _categoryService.GetCategoryById(e.InternalId);

					viewModel.Add(new CategoriesMappingViewModel
					{
						Id = e.Id,
						ExternalId = e.ExternalId,
						ExternalName = e.ExternalName,
						InternalId = e.InternalId,
						InternalName = internalCategory.Name
					});


				});

				viewModel.ForEach(item =>
				{
					Internalcategories.ForEach(cat =>
				   {
					   item.InternalCategoriesSelectList.Add(new SelectListItem
					   {
						   Text = cat.Name,
						   Value = cat.Id.ToString(),
						   Selected = cat.Name == item.InternalName
					   });
				   });
				});
			}

			return PartialView("~/Plugins/Import.ItLink/Views/EditCategoriesMapped.cshtml", viewModel);
		}

		[HttpPost, ActionName("Configure")]
		[ChildActionOnly]
		[FormValueRequired("MapCategories")]
		public ActionResult CategoriesMapping(List<CategoriesMappingViewModel> viewModel, bool isNew)
		{

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);


			// Check IF all categories were mapped
			bool error = false;
			string NotMappedCategs = "";

			viewModel.ForEach(item =>
			{
				if (item.InternalId == 0)
				{
					error = true;
					NotMappedCategs = item.ExternalName + "; ";
				}

			});

			if (error)
			{
				ErrorNotification(string.Format(_localizationService.GetResource("Plugins.Imort.ItLink.CategoriesMapping.Alert.CategoryWasNotMapped"), NotMappedCategs));
				return Configure();
			}

			//New Categories were downloaded from External Resource and mapped
			if (isNew)
			{
				_context.ExecuteSqlCommand("TRUNCATE TABLE InternalToExternal");
			}


			foreach (var item in viewModel)
			{
				var entity = _categoriesMappingrepository.GetById(item.Id);
				if (entity != null)
				{
					entity.InternalId = item.InternalId;
					entity.ExternalId = item.ExternalId;
					entity.ExternalName = item.ExternalName;
					_categoriesMappingrepository.Update(entity);
				}
				else
				{
					InternalToExternal newEntity = new InternalToExternal()
					{
						InternalId = item.InternalId,
						ExternalId = item.ExternalId,
						ExternalName = item.ExternalName
					};
					_categoriesMappingrepository.Insert(newEntity);
				}
			}


			settings.AreCategoriesMapped = true;
			_settingService.SaveSetting(settings);

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
