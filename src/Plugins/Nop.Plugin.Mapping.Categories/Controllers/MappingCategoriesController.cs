using Newtonsoft.Json;
using Nop.Admin.Models.Catalog;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Mapping.Categories.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static Nop.Plugin.Mapping.Categories.MappingCategoriesSettings;

namespace Nop.Plugin.Mapping.Categories.Controllers
{
	[AdminAuthorize]
	public class MappingCategoriesController : BasePluginController
	{
		#region Fields

		private readonly IExportManager _exportManager;
		private readonly IProductService _productService;
		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly IStoreService _storeService;
		private readonly IVendorService _vendorService;
		private readonly ISettingService _settingService;
		private readonly VendorSettings _vendorSettings;
		private readonly ILanguageService _languageService;
		private readonly ICategoryService _categoryService;
		private readonly ICurrencyService _currencyService;
		private readonly IPermissionService _permissionService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;
		private readonly IPictureService _pictureService;
		private readonly IManufacturerService _manufacturerService;


		#endregion

		#region ctor

		public MappingCategoriesController(
			IWorkContext workContext,
			IStoreContext storeContext,
			IStoreService storeService,
			IExportManager exportManager,
			IVendorService vendorService,
			VendorSettings vendorSettings,
			IProductService productService,
			IPictureService pictureService,
			ISettingService settingService,
			ICategoryService categoryService,
			ICurrencyService currencyService,
			ILanguageService languageService,
			IPermissionService permissionService,
			ILocalizationService localizationService,
			IManufacturerService manufacturerService,
			ILocalizedEntityService localizedEntityService)
		{
			this._workContext = workContext;
			this._storeService = storeService;
			this._storeContext = storeContext;
			this._exportManager = exportManager;
			this._vendorService = vendorService;
			this._pictureService = pictureService;
			this._productService = productService;
			this._settingService = settingService;
			this._vendorSettings = vendorSettings;
			this._currencyService = currencyService;
			this._categoryService = categoryService;
			this._languageService = languageService;
			this._permissionService = permissionService;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;
			this._manufacturerService = manufacturerService;
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
		public ActionResult Configure(int originalCategoryId, string externalName, string externalId)
		{
			if (!ModelState.IsValid)
			{
				return Configure();
			}

			if (string.IsNullOrEmpty(externalId) || string.IsNullOrEmpty(externalName))
			{
				ModelState.AddModelError("ExternalNameOrId", "External name or external Id must not be emppty");
				return Configure();
			}

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(settings.MappedCategoriesJson);

			mappedCategories.Add(new MapCategoryRow
			{
				InternalCategoryId = originalCategoryId,
				ExternalCategoryId = externalId,
				GoupKey = externalName
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
		public ActionResult Configure(string externalId, string groupName)
		{
			if (!ModelState.IsValid)
			{
				return Configure();
			}

			if (string.IsNullOrEmpty(externalId) || string.IsNullOrEmpty(groupName))
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

			var mappedItem = mappedCategories.FirstOrDefault(mc => mc.GoupKey == groupName && mc.ExternalCategoryId == externalId);

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
				var xml = ExportProductsToXmlPromUa(products, Url);
				return new XmlDownloadResult(xml, "products.xml");
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

		public string ExportProductsToXmlPromUa(IList<Product> products, UrlHelper urlHelper)
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(settings.MappedCategoriesJson);

			var sb = new StringBuilder();
			var stringWriter = new StringWriter(sb);
			var xmlWriter = new XmlTextWriter(stringWriter);
			xmlWriter.WriteStartDocument();

			xmlWriter.WriteStartElement("price");
			xmlWriter.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
			xmlWriter.WriteString("name", _storeContext.CurrentStore.Name);

			xmlWriter.WriteStartElement("currency");

			var currency = _currencyService.GetCurrencyByCode("UAH");

			xmlWriter.WriteAttributeString("code", _workContext.WorkingCurrency.CurrencyCode);
			xmlWriter.WriteAttributeString("rate", string.Format("{0}",
				currency == null ? _workContext.WorkingCurrency.Rate : currency.Rate));
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("catalog");

			#region categories

			var categories = products
								.Select(p => p.ProductCategories.Select(pc => pc.Category).FirstOrDefault())
								.ToList()
								.Distinct();

			foreach (var category in categories)
			{
				if (category == null) continue;

				xmlWriter.WriteStartElement("category");

				xmlWriter.WriteAttributeString("id", category.Id.ToString());
				xmlWriter.WriteAttributeString("parentID", category.ParentCategoryId.ToString());
				xmlWriter.WriteValue(category.Name);

				xmlWriter.WriteEndElement();
			}

			#endregion

			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("items");

			#region product

			foreach (var product in products)
			{
				xmlWriter.WriteStartElement("item");
				xmlWriter.WriteAttributeString("id", product.Id.ToString());

				xmlWriter.WriteString("name", product.Name);
				xmlWriter.WriteString("available", "true");

				var pc = product.ProductCategories.FirstOrDefault();

				if (pc != null)
				{
					xmlWriter.WriteString("categoryId", pc.CategoryId);
					xmlWriter.WriteString("price", product.Price * currency.Rate);

					var promCategory = mappedCategories.FirstOrDefault(mc => mc.GoupKey == "PromUA" && mc.InternalCategoryId == pc.CategoryId);

					if (promCategory != null)
					{
						xmlWriter.WriteStartElement("portal_category_id");
						xmlWriter.WriteValue(promCategory.ExternalCategoryId);
						xmlWriter.WriteEndElement();
					}
				}

				var url = HttpUtility.UrlDecode(
					string.Format("{0}{1}", urlHelper.RequestContext.HttpContext.Request.Url.Host,
					urlHelper.RouteUrl("Product", new { SeName = product.GetSeName() }))
				);
				xmlWriter.WriteString("url", url);

				if (product.ProductPictures != null && product.ProductPictures.Count > 0)
				{
					var pictId = product.ProductPictures.First().PictureId;
					xmlWriter.WriteString("image", _pictureService.GetPictureUrl(pictId));
				}

				xmlWriter.WriteStartElement("description");
				xmlWriter.WriteCData(product.FullDescription);
				xmlWriter.WriteEndElement();

				//product quantity is 0 then set it 10. Because all product are exporting must be exist
				xmlWriter.WriteString("quantity", product.StockQuantity > 0 ? product.StockQuantity : 10);

				xmlWriter.WriteString("warranty", 12);

				var productManufacturers = _manufacturerService.GetProductManufacturersByProductId(product.Id);
				if (productManufacturers != null && productManufacturers.Count > 0)
				{
					xmlWriter.WriteString("vendor", productManufacturers.First().Manufacturer.Name);
					xmlWriter.WriteString("vendorCode", product.Sku);
				}

				xmlWriter.WriteEndElement();
			}

			#endregion

			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();

			return stringWriter.ToString();
		}
	}
}
