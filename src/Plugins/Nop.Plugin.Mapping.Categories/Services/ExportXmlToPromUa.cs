using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Media;
using Nop.Services.Seo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static Nop.Plugin.Mapping.Categories.MappingCategoriesSettings;

namespace Nop.Plugin.Mapping.Categories.Services
{
	public class ExportXmlToPromUa : IExportXml<ExportXmlToPromUa>
	{
		public string Key => "promua";

		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly ISettingService _settingService;
		private readonly ICategoryService _categoryService;
		private readonly ICurrencyService _currencyService;
		private readonly IPictureService _pictureService;
		private readonly IManufacturerService _manufacturerService;

		private List<Category> categoriesAreDone = new List<Category>();

		public ExportXmlToPromUa(
					IWorkContext workContext,
					IStoreContext storeContext,
					ISettingService settingService,
					IPictureService pictureService,
					ICategoryService categoryService,
					ICurrencyService currencyService,
					IManufacturerService manufacturerService)
		{
			this._workContext = workContext;
			this._storeContext = storeContext;
			this._settingService = settingService;
			this._pictureService = pictureService;
			this._categoryService = categoryService;
			this._currencyService = currencyService;
			this._manufacturerService = manufacturerService;
		}

		public string ExportProductsToXml(IList<Product> products, UrlHelper urlHelper, int storeConfiguration)
		{
			var storeScope = storeConfiguration;
			var settings = _settingService.LoadSetting<MappingCategoriesSettings>(storeScope);

			var mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(
					settings.MappedCategoriesJson);

			mappedCategories = mappedCategories
				.Where(mc => mc.GoupKey.ToLower() == this.Key)
				.ToList();

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
								.Select(p => p.ProductCategories
									.Where(pc => !pc.Category.Deleted && pc.Category.Published)
									.Select(pc => pc.Category)
									.FirstOrDefault())
								.ToList()
								.Distinct();

			foreach (var category in categories)
			{
				WriteCategoriesRecurcieve(xmlWriter, category);
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

				var pc = product.ProductCategories
					.Where(pcat => !pcat.Category.Deleted && pcat.Category.Published)
					.FirstOrDefault();

				if (pc != null)
				{
					xmlWriter.WriteString("categoryId", pc.CategoryId);
					xmlWriter.WriteString("price", product.Price * currency.Rate);

					var promCategory = mappedCategories
						.FirstOrDefault(mc => mc.InternalCategoryId == pc.CategoryId);

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

		protected virtual void WriteCategoriesRecurcieve(XmlTextWriter xmlTextWriter, Category category)
		{
			if (category == null
				|| categoriesAreDone.Contains(category)) return;

			categoriesAreDone.Add(category);

			if (category.ParentCategoryId > 0)
			{
				WriteCategoriesRecurcieve(xmlTextWriter, _categoryService.GetCategoryById(category.ParentCategoryId));
			}

			xmlTextWriter.WriteStartElement("category");

			xmlTextWriter.WriteAttributeString("id", category.Id.ToString());

			if (category.ParentCategoryId > 0)
				xmlTextWriter.WriteAttributeString("parentID", category.ParentCategoryId.ToString());

			xmlTextWriter.WriteValue(category.Name);

			xmlTextWriter.WriteEndElement();
		}
	}
}
