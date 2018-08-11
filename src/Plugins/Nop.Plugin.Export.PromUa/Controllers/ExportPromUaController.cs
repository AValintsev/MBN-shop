using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Controllers;
using Nop.Admin.Extensions;
using Nop.Admin.Helpers;
using Nop.Admin.Infrastructure.Cache;
using Nop.Admin.Models.Catalog;
using Nop.Admin.Models.Orders;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Export.PromUa
{
	public class ExportPromUaController : BaseAdminController
	{
		#region Fields

		private readonly IProductService _productService;
		private readonly IProductTemplateService _productTemplateService;
		private readonly ICategoryService _categoryService;
		private readonly IManufacturerService _manufacturerService;
		private readonly ICustomerService _customerService;
		private readonly IUrlRecordService _urlRecordService;
		private readonly IWorkContext _workContext;
		private readonly ILanguageService _languageService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;
		private readonly ISpecificationAttributeService _specificationAttributeService;
		private readonly IPictureService _pictureService;
		private readonly ITaxCategoryService _taxCategoryService;
		private readonly IProductTagService _productTagService;
		private readonly ICopyProductService _copyProductService;
		private readonly IPdfService _pdfService;
		private readonly IExportManager _exportManager;
		private readonly IImportManager _importManager;
		private readonly ICustomerActivityService _customerActivityService;
		private readonly IPermissionService _permissionService;
		private readonly IAclService _aclService;
		private readonly IStoreService _storeService;
		private readonly IOrderService _orderService;
		private readonly IStoreMappingService _storeMappingService;
		private readonly IVendorService _vendorService;
		private readonly IDateRangeService _dateRangeService;
		private readonly IShippingService _shippingService;
		private readonly IShipmentService _shipmentService;
		private readonly ICurrencyService _currencyService;
		private readonly CurrencySettings _currencySettings;
		private readonly IMeasureService _measureService;
		private readonly MeasureSettings _measureSettings;
		private readonly ICacheManager _cacheManager;
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly IDiscountService _discountService;
		private readonly IProductAttributeService _productAttributeService;
		private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IProductAttributeFormatter _productAttributeFormatter;
		private readonly IProductAttributeParser _productAttributeParser;
		private readonly IDownloadService _downloadService;
		private readonly ISettingService _settingService;
		private readonly TaxSettings _taxSettings;
		private readonly VendorSettings _vendorSettings;

		#endregion

		#region Constructors

		public ExportPromUaController(IProductService productService,
			IProductTemplateService productTemplateService,
			ICategoryService categoryService,
			IManufacturerService manufacturerService,
			ICustomerService customerService,
			IUrlRecordService urlRecordService,
			IWorkContext workContext,
			ILanguageService languageService,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService,
			ISpecificationAttributeService specificationAttributeService,
			IPictureService pictureService,
			ITaxCategoryService taxCategoryService,
			IProductTagService productTagService,
			ICopyProductService copyProductService,
			IPdfService pdfService,
			IExportManager exportManager,
			IImportManager importManager,
			ICustomerActivityService customerActivityService,
			IPermissionService permissionService,
			IAclService aclService,
			IStoreService storeService,
			IOrderService orderService,
			IStoreMappingService storeMappingService,
			IVendorService vendorService,
			IDateRangeService dateRangeService,
			IShippingService shippingService,
			IShipmentService shipmentService,
			ICurrencyService currencyService,
			CurrencySettings currencySettings,
			IMeasureService measureService,
			MeasureSettings measureSettings,
			ICacheManager cacheManager,
			IDateTimeHelper dateTimeHelper,
			IDiscountService discountService,
			IProductAttributeService productAttributeService,
			IBackInStockSubscriptionService backInStockSubscriptionService,
			IShoppingCartService shoppingCartService,
			IProductAttributeFormatter productAttributeFormatter,
			IProductAttributeParser productAttributeParser,
			IDownloadService downloadService,
			ISettingService settingService,
			TaxSettings taxSettings,
			VendorSettings vendorSettings)
		{
			this._productService = productService;
			this._productTemplateService = productTemplateService;
			this._categoryService = categoryService;
			this._manufacturerService = manufacturerService;
			this._customerService = customerService;
			this._urlRecordService = urlRecordService;
			this._workContext = workContext;
			this._languageService = languageService;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;
			this._specificationAttributeService = specificationAttributeService;
			this._pictureService = pictureService;
			this._taxCategoryService = taxCategoryService;
			this._productTagService = productTagService;
			this._copyProductService = copyProductService;
			this._pdfService = pdfService;
			this._exportManager = exportManager;
			this._importManager = importManager;
			this._customerActivityService = customerActivityService;
			this._permissionService = permissionService;
			this._aclService = aclService;
			this._storeService = storeService;
			this._orderService = orderService;
			this._storeMappingService = storeMappingService;
			this._vendorService = vendorService;
			this._dateRangeService = dateRangeService;
			this._shippingService = shippingService;
			this._shipmentService = shipmentService;
			this._currencyService = currencyService;
			this._currencySettings = currencySettings;
			this._measureService = measureService;
			this._measureSettings = measureSettings;
			this._cacheManager = cacheManager;
			this._dateTimeHelper = dateTimeHelper;
			this._discountService = discountService;
			this._productAttributeService = productAttributeService;
			this._backInStockSubscriptionService = backInStockSubscriptionService;
			this._shoppingCartService = shoppingCartService;
			this._productAttributeFormatter = productAttributeFormatter;
			this._productAttributeParser = productAttributeParser;
			this._downloadService = downloadService;
			this._settingService = settingService;
			this._taxSettings = taxSettings;
			this._vendorSettings = vendorSettings;
		}

		#endregion

		#region Export / Import

		

		#endregion

		#region Methods

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

		#endregion
	}
}
