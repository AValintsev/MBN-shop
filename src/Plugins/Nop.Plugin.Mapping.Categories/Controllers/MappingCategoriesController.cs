using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Mapping.Categories.Models;
using Nop.Plugin.Mapping.Categories.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Nop.Plugin.Mapping.Categories.MappingCategoriesSettings;

namespace Nop.Plugin.Mapping.Categories.Controllers
{
	[AdminAuthorize]
	public class MappingCategoriesController : BasePluginController
	{
		#region Fields

		private readonly IExportXml<ExportXmlToPromUa> _exportManagerPromUa;
		private readonly IExportXml<ExportXmlToHotline> _exportManagerHotline;

		private readonly IWorkContext _workContext;
		private readonly IStoreService _storeService;
		private readonly IProductService _productService;
		private readonly ISettingService _settingService;
		private readonly ICategoryService _categoryService;
		private readonly IPermissionService _permissionService;
		private readonly ILocalizationService _localizationService;

		#endregion

		#region ctor

		public MappingCategoriesController(
			IWorkContext workContext,
			IStoreService storeService,
			IExportXml<ExportXmlToPromUa> exportManagerPromUa,
			IExportXml<ExportXmlToHotline> exportManagerHotline,
			IProductService productService,
			ISettingService settingService,
			ICategoryService categoryService,
			IPermissionService permissionService,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService)
		{
			this._workContext = workContext;
			this._storeService = storeService;
			this._exportManagerPromUa = exportManagerPromUa;
			this._exportManagerHotline = exportManagerHotline;
			this._productService = productService;
			this._settingService = settingService;
			this._categoryService = categoryService;
			this._permissionService = permissionService;
			this._localizationService = localizationService;
		}

		#endregion

		[ChildActionOnly]
		public ActionResult Configure()
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(settings.MappedCategoriesJson);

			var viewModel = new ConfigurationModel();

			var categories = _categoryService.GetAllCategories().ToList();
			foreach (var c in categories)
			{
				var currentMap = mappedCategories.Where(m => m.InternalCategoryId == c.Id);

				viewModel.MappedCategories.Add(new ConfigurationModel.MappingModel
				{
					OriginalCategory = c,
					MappedCategoriesRow = currentMap.ToList()
				});
			}

			return View("~/Plugins/Mapping.Categories/Views/Configure.cshtml", viewModel);
		}

		[HttpPost]
		[ChildActionOnly]
		[FormValueRequired("save")]
		public ActionResult Configure(int originalCategoryId, string goupKey, string externalCategoryId)
		{
			if (!ModelState.IsValid)
			{
				return Configure();
			}

			if (string.IsNullOrEmpty(externalCategoryId) || string.IsNullOrEmpty(goupKey))
			{
				ModelState.AddModelError("ExternalNameOrId", "External name or external Id must not be emppty");
				return Configure();
			}

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(
					settings.MappedCategoriesJson);

			mappedCategories.Add(new MapCategoryRow
			{
				InternalCategoryId = originalCategoryId,
				ExternalCategoryId = externalCategoryId,
				GoupKey = goupKey.ToLower()
			});

			var json = JsonConvert.SerializeObject(mappedCategories);

			settings.MappedCategoriesJson = json;

			_settingService.SaveSetting(settings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

		[HttpPost]
		[ChildActionOnly]
		[FormValueRequired("remove")]
		public ActionResult Configure(string externalCategoryId, string goupKey)
		{
			if (!ModelState.IsValid)
			{
				return Configure();
			}

			if (string.IsNullOrEmpty(externalCategoryId) || string.IsNullOrEmpty(goupKey))
			{
				ModelState.AddModelError("ExternalId", "External name or external Id must not be emppty");
				return Configure();
			}

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(settings.MappedCategoriesJson);

			var mappedItem = mappedCategories.FirstOrDefault(mc => mc.GoupKey.ToLower() == goupKey.ToLower() && mc.ExternalCategoryId == externalCategoryId);

			if (mappedItem != null)
			{
				mappedCategories.Remove(mappedItem);
			}


			var json = JsonConvert.SerializeObject(mappedCategories);

			settings.MappedCategoriesJson = json;

			_settingService.SaveSetting(settings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

		[HttpPost]
		public virtual ActionResult ExportPromUaXmlSelected(string selectedIds)
		{
			return Export(selectedIds, "promua");
		}

		[HttpPost]
		public virtual ActionResult ExportHotlineXmlSelected(string selectedIds)
		{
			return Export(selectedIds, "hotline");
		}

		private ActionResult Export(string selectedIds, string exportName)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
				return AccessDeniedView();

			var products = new List<Product>();
			if (selectedIds != null)
			{
				//means export all
				if (selectedIds == "-10")
				{
					products.AddRange(_productService.SearchProducts());
				}
				else
				{
					var ids = selectedIds
						.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					products.AddRange(_productService.GetProductsByIds(ids));
				}
			}
			//a vendor should have access only to his products
			if (_workContext.CurrentVendor != null)
			{
				products = products.Where(p => p.VendorId == _workContext.CurrentVendor.Id).ToList();
			}

			products = products.Where(p => p.StockQuantity > 0).ToList();

			try
			{
				switch (exportName.ToLower())
				{
					case "promua":
						{
							var xml = this._exportManagerPromUa.ExportProductsToXml(
								products,
								Url,
								this.GetActiveStoreScopeConfiguration(_storeService, _workContext));

							return new XmlDownloadResult(xml, "products-for-promUa.xml");
						}
					case "hotline":
					default:
						{
							var xml = this._exportManagerHotline.ExportProductsToXml(
								products,
								Url,
								this.GetActiveStoreScopeConfiguration(_storeService, _workContext));

							return new XmlDownloadResult(xml, "products-for-hotline.xml");
						}
				}
			}
			catch (Exception exc)
			{
				ErrorNotification(exc);
				return RedirectToAction("List");
			}
		}

		[NonAction]
		protected virtual List<int> GetChildCategoryIds(int parentCategoryId)
		{
			var categoriesIds = new List<int>();
			var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
			foreach (var category in categories)
			{
				categoriesIds.Add(category.Id);
				categoriesIds.AddRange(GetChildCategoryIds(category.Id));
			}
			return categoriesIds;
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
