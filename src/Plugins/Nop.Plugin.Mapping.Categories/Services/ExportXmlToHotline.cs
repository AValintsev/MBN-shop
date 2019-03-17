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
	public class ExportXmlToHotline : IExportXml<ExportXmlToHotline>
	{
		public string Key => "hotline";

		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly ISettingService _settingService;
		private readonly ICategoryService _categoryService;
		private readonly ICurrencyService _currencyService;
		private readonly IPictureService _pictureService;
		private readonly IManufacturerService _manufacturerService;

		private List<Category> categoriesAreDone = new List<Category>();
		private List<MapCategoryRow> mappedCategories = new List<MapCategoryRow>();

		public ExportXmlToHotline(
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

			mappedCategories = string.IsNullOrEmpty(settings.MappedCategoriesJson)
				? new List<MapCategoryRow>()
				: JsonConvert.DeserializeObject<List<MappingCategoriesSettings.MapCategoryRow>>(
					settings.MappedCategoriesJson);
			mappedCategories = mappedCategories
				.Where(mc => mc.GoupKey == "hotline")
				.ToList();

			var mappedCategoriesIds = mappedCategories.Select(mc => mc.InternalCategoryId);
			products = products
				.Where(p => p.ProductCategories
							.Any(pc => mappedCategoriesIds.Contains(pc.CategoryId))
				)
				.ToList();

			var sb = new StringBuilder();
			var stringWriter = new StringWriter(sb);
			var xmlWriter = new XmlTextWriter(stringWriter);
			xmlWriter.WriteStartDocument();

			xmlWriter.WriteStartElement("price");
			xmlWriter.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
			xmlWriter.WriteString("firmName", _storeContext.CurrentStore.Name);

			var currency = _currencyService.GetCurrencyByCode("UAH");

			xmlWriter.WriteString("rate", string.Format("{0}",
				currency == null ? _workContext.WorkingCurrency.Rate : currency.Rate));

			xmlWriter.WriteStartElement("categories");

			#region categories

			var categories = products
								.Select(p => p.ProductCategories
									.FirstOrDefault(pc => !pc.Category.Deleted && pc.Category.Published)
									.Category)
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
				xmlWriter.WriteString("id", product.Id.ToString());
				xmlWriter.WriteString("code", product.Sku);
				xmlWriter.WriteString("name", product.Name);
				xmlWriter.WriteString("description", product.ShortDescription);

				var pc = product.ProductCategories
					.FirstOrDefault(pcat => !pcat.Category.Deleted && pcat.Category.Published);

				if (pc != null)
				{
					//xmlWriter.WriteString("categoryId", pc.CategoryId);
					//xmlWriter.WriteString("price", product.Price * currency.Rate);

					var hotlineCategory = mappedCategories
						.FirstOrDefault(mc => mc.InternalCategoryId == pc.CategoryId);

					if (hotlineCategory != null)
					{
						//xmlWriter.WriteStartElement("portal_category_id");
						xmlWriter.WriteString("categoryId", hotlineCategory.ExternalCategoryId);
						//xmlWriter.WriteEndElement();
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

				xmlWriter.WriteString("priceRUAH", product.Price * currency.Rate);
				xmlWriter.WriteString("priceRUSD", product.Price);
				//xmlWriter.WriteString("stock", "Под заказ")
				//xmlWriter.WriteString("delivery", "<delivery id=\"1\" cost=\"70\" freeFrom=\"5000\" time=\"3\"/>");
				//xmlWriter.WriteString("guarantee", "<guarantee type=\"manufacturer\">12</guarantee>");
				//xmlWriter.WriteString("<param name=\"Страна изготовления\">Китай</param>");
				//xmlWriter.WriteString("<param name=\"Оригинальность\">Оригинал</param>");
				//xmlWriter.WriteString("condition", 0);
				//xmlWriter.WriteString("custom", 1);

				var productManufacturers = _manufacturerService.GetProductManufacturersByProductId(product.Id);
				if (productManufacturers != null && productManufacturers.Count > 0)
				{
					xmlWriter.WriteString("vendor", productManufacturers.First().Manufacturer.Name);
					//xmlWriter.WriteString("vendorCode", product.Sku);
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

			var hotlineCategory = mappedCategories
				.FirstOrDefault(mc => mc.InternalCategoryId == category.Id);

			if (hotlineCategory == null)
				throw new Exception($"Category {category.Id} - {category.Name} is not mapped to hotline category");

			xmlTextWriter.WriteString("id", hotlineCategory.ExternalCategoryId);

			if (category.ParentCategoryId > 0)
			{
				var hotlineParent = mappedCategories.FirstOrDefault(mc => mc.InternalCategoryId == category.ParentCategoryId);
				if (hotlineParent == null) throw new Exception($"Category {category.ParentCategoryId} is not mapped to hotline category");
				xmlTextWriter.WriteString("parentID", hotlineParent.ExternalCategoryId);
			}

			xmlTextWriter.WriteString("name", category.Name);

			xmlTextWriter.WriteEndElement();
		}
	}
}
