using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using System.Linq;
using Nop.Services.ExportImport.Help;
using Nop.Services.Seo;
using System.Collections.Generic;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class ItLinkExportManager : ExportManager
	{
		private readonly ICategoryService _categoryService;
		private readonly IPictureService _pictureService;

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
			this._pictureService = pictureService;
		}

		protected override string GetCategories(Product product)
		{
			string categoryNames = null;
			var pc = _categoryService.GetCategoryById(product.ProductCategories.First().CategoryId);

			categoryNames += pc.Name;
			categoryNames += ";";

			return categoryNames;
		}

		protected override string[] GetPictures(Product product)
		{
			//pictures (up to 3 pictures)
			string picture1 = null;
			string picture2 = null;
			string picture3 = null;

			var pictures = product.ProductPictures.Select(p => p.Picture).ToList();

			for (var i = 0; i < pictures.Count; i++)
			{
				var pictureLocalPath = _pictureService.GetThumbLocalPath(pictures[i]);
				switch (i)
				{
					case 0:
						picture1 = pictureLocalPath;
						break;
					case 1:
						picture2 = pictureLocalPath;
						break;
					case 2:
						picture3 = pictureLocalPath;
						break;
				}
			}
			return new[] { picture1, picture2, picture3 };
		}
	}
}
