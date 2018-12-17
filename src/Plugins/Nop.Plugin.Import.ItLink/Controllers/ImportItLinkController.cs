using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Import.ItLink.Data;
using Nop.Plugin.Import.ItLink.Domain;
using Nop.Plugin.Import.ItLink.Models;
using Nop.Plugin.Import.ItLink.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly IVendorService _vendorService;
		private readonly ISettingService _settingService;
		private readonly VendorSettings _vendorSettings;
		private readonly ILanguageService _languageService;
		private readonly ICategoryService _categoryService;
		private readonly IPermissionService _permissionService;
		private readonly IItLinkImportManager _itLinkImportManager;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;
		private readonly IRepository<CategoryInternalToExternalMap> _categoriesMappingRepository;

		#endregion

		private int _vendorId = 0;
		private int VendorId
		{
			get
			{
				if (this._vendorId == 0)
				{
					var vendor = _vendorService.GetAllVendors("ItLink", showHidden: true).FirstOrDefault();
					if (vendor == null)
					{
						vendor = new Vendor
						{
							Name = "ItLink",
							Active = true,
							AdminComment = "Auto added vendor to map to ItLink provider"
						};
						_vendorService.InsertVendor(vendor);
					}
					this._vendorId = vendor.Id;
				}

				return this._vendorId;
			}
		}

		#region ctor

		public ImportItLinkController(
			IWorkContext workContext,
			IStoreContext storeContext,
			IStoreService storeService,
			IVendorService vendorService,
			VendorSettings vendorSettings,
			ISettingService settingService,
			ICategoryService categoryService,
			ILanguageService languageService,
			IPermissionService permissionService,
			IItLinkImportManager itLinkImportManager,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService,
			IRepository<CategoryInternalToExternalMap> categoriesMappingRepository)
		{
			this._workContext = workContext;
			this._storeService = storeService;
			this._storeContext = storeContext;
			this._vendorService = vendorService;
			this._settingService = settingService;
			this._vendorSettings = vendorSettings;
			this._categoryService = categoryService;
			this._languageService = languageService;
			this._permissionService = permissionService;
			this._itLinkImportManager = itLinkImportManager;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;
			this._categoriesMappingRepository = categoriesMappingRepository;
		}

		#endregion

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
				Password = settings.Password,
				Percent = settings.Percent
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
			settings.Percent = viewModel.Percent;

			_settingService.SaveSetting(settings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

		public virtual PartialViewResult ImportFromItLink()
		{
			var viewPath = "~/Plugins/Import.ItLink/Views/ImportFromItLinkPartialResult.cshtml";
			var viewModel = new ImportFromItLinkResultViewModel();

			if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
			{
				viewModel.Errors.Add("Access Denied");
				return PartialView(viewPath, viewModel);
			}

			//a vendor can not import products
			if (_workContext.CurrentVendor != null && !_vendorSettings.AllowVendorsToImportProducts)
			{
				viewModel.Errors.Add("Access Denied");
				return PartialView(viewPath, viewModel);
			}
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

			try
			{
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

				var itLinkXmlDocument = new XmlDocument();
				itLinkXmlDocument.Load(xmlReader);

				var mappedCategories = this._categoriesMappingRepository.Table
					.Where(map => map.VendorId == VendorId)
					.ToDictionary(x => x.ExternalId, s => s.InternalId);

				var errors = _itLinkImportManager.Import(itLinkXmlDocument, mappedCategories, VendorId, settings.Percent);

				if (errors.Count > 0)
				{
					viewModel.Errors = errors;
					return PartialView(viewPath, viewModel);
				}
			}
			catch (Exception exc)
			{
				viewModel.Errors.Add(exc.Message);
			}

			viewModel.Messages.Add(_localizationService.GetResource("Admin.Catalog.Products.Imported"));
			return PartialView(viewPath, viewModel);
		}

		public PartialViewResult CategoriesMapping()
		{
			List<CategoriesMappingViewModel> viewModel = new List<CategoriesMappingViewModel>();

			//Load all categories from It-Link xml
			var externalCategories = LoadExternalCategories();

			//Load already mapped categories from internal database
			var mappedCategories = this._categoriesMappingRepository.Table
				.Where(map => map.VendorId == VendorId)
				.ToList();

			//Load all internal categories
			var internalCategories = _categoryService.GetAllCategories().ToList();

			//Prepare view model for each external category and map with internal category if exists
			externalCategories.ForEach(ec =>
			{
				var mappedCategory = mappedCategories.FirstOrDefault(mc => mc.ExternalId == ec.ExternalId);
				viewModel.Add(new CategoriesMappingViewModel
				{
					ExternalCategoryId = ec.ExternalId,
					ExternalCategoryName = ec.ExternalName,
					Id = mappedCategory == null ? 0 : mappedCategory.Id,
					InternalSelectedCategoryId = mappedCategory == null ? 0 : mappedCategory.InternalId
				});
			});

			//Load lists for each view model
			viewModel.ForEach(vm =>
			{
				vm.InternalCategories = internalCategories
						.Select(c => new SelectListItem
						{
							Text = c.Name,
							Value = c.Id.ToString(),
							Selected = c.Id == vm.InternalSelectedCategoryId
						})
						.ToList();
				vm.InternalCategories.Insert(0, new SelectListItem { Text = "", Value = "-1" });
			});

			return PartialView("~/Plugins/Import.ItLink/Views/CategoiresMapping.cshtml", viewModel);
		}

		[HttpPost, ActionName("Configure")]
		[ChildActionOnly]
		[FormValueRequired("mapcategories")]
		public ActionResult MapCategories(List<CategoriesMappingViewModel> viewModel)
		{
			var newMappings = new List<CategoryInternalToExternalMap>();
			var existedMappings = new List<CategoryInternalToExternalMap>();

			viewModel.ForEach(item =>
			{
				if (item.Id == 0)
				{
					//Insert new
					newMappings.Add(new CategoryInternalToExternalMap
					{
						ExternalId = item.ExternalCategoryId,
						ExternalName = item.ExternalCategoryName,
						InternalId = item.InternalSelectedCategoryId,
						VendorId = VendorId
					});
				}
				else
				{
					var map = _categoriesMappingRepository.GetById(item.Id);
					if (map != null)
					{
						map.InternalId = item.InternalSelectedCategoryId;
						map.VendorId = VendorId;
						existedMappings.Add(map);
					}
				}
			});

			if (newMappings.Count > 0)
			{
				this._categoriesMappingRepository.Insert(newMappings);
			}
			if (existedMappings.Count > 0)
			{
				this._categoriesMappingRepository.Update(existedMappings);
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

		private List<CategoryInternalToExternalMap> LoadExternalCategories()
		{
			var result = new List<CategoryInternalToExternalMap>();

			try
			{
				var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
				var settings = _settingService.LoadSetting<ImportItLinkSettings>(storeScope);

				var credentials = new NetworkCredential(settings.Username, settings.Password);
				XmlUrlResolver xmlResolver = new XmlUrlResolver
				{
					Credentials = credentials
				};

				using (XmlReader reader = XmlReader.Create(settings.XmlUrl,
					new XmlReaderSettings
					{
						XmlResolver = xmlResolver,
						DtdProcessing = DtdProcessing.Ignore
					}))
				{
					//Read only category sections from XML
					while (reader.ReadToFollowing("category"))
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							var map = new CategoryInternalToExternalMap
							{
								ExternalId = reader.GetAttribute("id"),
								ExternalName = reader.ReadInnerXml()
							};

							result.Add(map);
						}
					}
				}

				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
