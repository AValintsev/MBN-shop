using Nop.Services.ExportImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class ItLinkExportManager : ExportManager
	{
		private readonly ICategoryService _categoryService;

		public ItLinkExportManager(
			ICategoryService categoryService,
			IManufacturerService manufacturerService,
			ICustomerService customerService,
			IProductAttributeService productAttributeService,
			IPictureService pictureService,
			INewsLetterSubscriptionService newsLetterSubscriptionService,
			IStoreService storeService,
			IWorkContext workContext,
			ProductEditorSettings productEditorSettings,
			IVendorService vendorService,
			IProductTemplateService productTemplateService,
			IDateRangeService dateRangeService,
			ITaxCategoryService taxCategoryService,
			IMeasureService measureService,
			CatalogSettings catalogSettings,
			IGenericAttributeService genericAttributeService,
			ICustomerAttributeFormatter customerAttributeFormatter,
			OrderSettings orderSettings) :
			base(categoryService, manufacturerService, customerService, productAttributeService, pictureService, newsLetterSubscriptionService, storeService, workContext, productEditorSettings, vendorService, productTemplateService, dateRangeService, taxCategoryService, measureService, catalogSettings, genericAttributeService, customerAttributeFormatter, orderSettings)
		{
			this._categoryService = categoryService;
		}

		protected override string GetCategories(Product product)
		{
			string categoryNames = null;
			var pc = _categoryService.GetCategoryById(product.ProductCategories.First().CategoryId);

			categoryNames += pc.Name;
			categoryNames += ";";

			return categoryNames;
		}
	}
}
