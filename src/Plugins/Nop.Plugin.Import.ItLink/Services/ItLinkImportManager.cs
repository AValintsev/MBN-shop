using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class ItLinkImportManager : IItLinkImportManager
	{
		private readonly IWorkContext _workContext;
		private readonly IDataProvider _dataProvider;
		private readonly MediaSettings _mediaSettings;
		private readonly IVendorService _vendorService;
		private readonly IProductService _productService;
		private readonly IPictureService _pictureService;
		private readonly ICurrencyService _currencyService;
		private readonly IUrlRecordService _urlRecordService;
		private readonly IManufacturerService _manufacturerService;
		private readonly ILocalizationService _localizationService;

		#region ctor

		public ItLinkImportManager(
			IWorkContext workContext,
			IDataProvider dataProvider,
			MediaSettings mediaSettings,
			IVendorService vendorService,
			IProductService productService,
			IPictureService pictureService,
			ICurrencyService _currencyService,
			IUrlRecordService urlRecordService,
			IManufacturerService manufacturerService,
			ILocalizationService localizationService)
		{
			this._workContext = workContext;
			this._dataProvider = dataProvider;
			this._mediaSettings = mediaSettings;
			this._vendorService = vendorService;
			this._productService = productService;
			this._pictureService = pictureService;
			this._currencyService = _currencyService;
			this._urlRecordService = urlRecordService;
			this._manufacturerService = manufacturerService;
			this._localizationService = localizationService;
		}

		#endregion

		/// <summary>
		/// Imports products from ItLinks xml.
		/// </summary>
		/// <param name="xmlDocument">xml document</param>
		/// <param name="categoriesMap">dictionary key = ItLink cateogry name, value = internal category Id</param>
		/// <param name="overrideExistedImanges">true if want to override existed images (could be clower).</param>
		/// <returns>Lists with errors.</returns>
		public List<string> Import(
			XmlDocument xmlDocument,
			Dictionary<string, int> categoriesMap,
			int vendorId,
			bool overrideExistedImanges = false)
		{
			if (categoriesMap == null)
			{
				throw new Exception(_localizationService.GetResource("Plugins.Imort.ItLink.CategoriesMapping.Alert.PleaseMapCategories"));
			}

			var result = new List<string>();

			try
			{
				XmlNodeList offers = xmlDocument.GetElementsByTagName("offer");
				var rateUsdToUah = _currencyService.GetCurrencyByCode("UAH").Rate;

				List<string> allSku = new List<string>(offers.Count);
				List<string> manufacturerNames = new List<string>();

				foreach (XmlNode offer in offers)
				{
					allSku.Add(offer["vendorCode"].InnerText);
					manufacturerNames.Add(offer["vendor"].InnerText);
				}

				manufacturerNames = manufacturerNames.Distinct().ToList();

				//performance optimization, the check for the existence of the manufacturers in one SQL request
				var notExistingManufacturers = _manufacturerService.GetNotExistingManufacturers(manufacturerNames.ToArray());
				if (notExistingManufacturers.Any())
				{
					notExistingManufacturers.ToList().ForEach(m =>
					{
						_manufacturerService.InsertManufacturer(
							new Manufacturer
							{
								CreatedOnUtc = DateTime.UtcNow,
								UpdatedOnUtc = DateTime.UtcNow,
								Name = m,
								Published = true
							});
					});
				}

				//performance optimization, load all products by SKU in one SQL request
				var allProductsBySku = _productService.GetProductsBySku(allSku.ToArray(), vendorId);

				//performance optimization, load all manufacturers IDs for products in one SQL request
				var allProductsManufacturerIds = _manufacturerService.GetProductManufacturerIds(allProductsBySku.Select(p => p.Id).ToArray());

				//performance optimization, load all manufacturers in one SQL request
				var allManufacturers = _manufacturerService.GetAllManufacturers(showHidden: true);

				foreach (XmlNode offer in offers)
				{
					var sku = offer["vendorCode"].InnerText;
					var productName = offer["name"].InnerText;
					var manufacturerName = offer["vendor"].InnerText;

					var product = allProductsBySku.FirstOrDefault(p => p.Sku == sku);

					try
					{
						var isNew = product == null;

						product = product ?? new Product();

						if (isNew)
						{
							product.CreatedOnUtc = DateTime.UtcNow;
							product.ProductType = ProductType.SimpleProduct;
							product.Name = productName;
							product.ShortDescription = productName;
							product.FullDescription = productName;
							product.MetaKeywords = string.Format("{0}; {1}", manufacturerName, sku);
							product.MetaDescription = productName;
							product.MetaTitle = productName;
							product.Sku = sku;
							product.ManufacturerPartNumber = sku;
							product.MarkAsNew = true;
							product.ShowOnHomePage = false;
							product.IsGiftCard = false;
							product.RequireOtherProducts = false;
							product.IsShipEnabled = true;
							product.DisplayStockQuantity = true;
							product.VisibleIndividually = true;
							product.VendorId = vendorId;
							product.AllowCustomerReviews = true;
							product.ProductTemplateId = 1;
						}
						product.UpdatedOnUtc = DateTime.UtcNow;

						#region Setup product

						product.Published = true;
						product.StockQuantity = 50;
						product.OrderMaximumQuantity = 11;
						product.AdminComment = offer.Attributes["id"].Value;

						product.OldPrice = isNew ? product.OldPrice : product.Price;
						var priceUsdStr = offer.SelectSingleNode("param[@name='priceUSD']").InnerText;
						product.Price = decimal.Parse(priceUsdStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);
						//We use store's internal rate
						//product.Price = decimal.Parse(offer["oldprice"].InnerText) / rateUsdToUah;

						try
						{
							product.ProductCategories.Add(
								new ProductCategory
								{
									CategoryId = categoriesMap[offer["categoryId"].InnerText]
								});
						}
						catch
						{
							throw new Exception(
								string.Format(_localizationService.GetResource("Plugins.Imort.ItLink.CategoriesMapping.Alert.CategoryWasNotMapped"), offer["categoryId"].InnerText));
						}

						#endregion

						if (isNew)
						{
							_productService.InsertProduct(product);
						}
						//Do not update product twice
						//else
						//{
						//	_productService.UpdateProduct(product);
						//}

						_urlRecordService.SaveSlug(product, product.ValidateSeName(product.GetSeName(), product.Name, true), _workContext.WorkingLanguage.Id);

						#region product Manufacturers

						var productManufacturerExists = false;
						if (!isNew)
						{
							var productManufacturers = product.ProductManufacturers
								.Where(pm => pm.Manufacturer.Name == manufacturerName).ToList();

							productManufacturerExists = productManufacturers.Count > 0;
						}
						if (isNew && !productManufacturerExists)
						{
							var manufacturer = allManufacturers.Where(m => m.Name == manufacturerName).FirstOrDefault();
							if (manufacturer != null)
							{
								var productManufacturer = new ProductManufacturer
								{
									ProductId = product.Id,
									ManufacturerId = manufacturer.Id,
									IsFeaturedProduct = false,
									DisplayOrder = 1
								};

								_manufacturerService.InsertProductManufacturer(productManufacturer);

								product.ProductManufacturers.Add(productManufacturer);
							}
						}

						#endregion

						#region product Pictures

						if (isNew || (!isNew && overrideExistedImanges))
						{
							product.ProductPictures.Add(DownloadNewPicture(offer["picture"], product.Name));
						}

						#endregion

						_productService.UpdateProduct(product);
					}
					catch (Exception e)
					{
						result.Add(string.Format("SKU: {0}.<br />Item: {1}.<br />Message: {2}", product.Sku, product.Name, e.Message));
					}
				}

				//Hide all products which were not updated. It means that products are not available any more.
				HideNotUpdatedProducts(allProductsBySku, vendorId);
			}
			catch (Exception e)
			{
				result.Add(e.Message);
			}

			return result;
		}

		private void HideNotUpdatedProducts(IList<Product> newellyImportedProducts, int vendorId)
		{
			var allProductsByVendor = _productService.SearchProducts(vendorId: vendorId);
			var productsWereNotUpdated = allProductsByVendor.Except(newellyImportedProducts).ToList();

			if (productsWereNotUpdated.Count > 0)
			{
				productsWereNotUpdated.ForEach(p =>
				{
					p.StockQuantity = 0;
					//p.VisibleIndividually = false;
					//p.Deleted = true;
					//p.Published = false;
				});

				_productService.UpdateProducts(productsWereNotUpdated);
			}
		}

		private ProductPicture DownloadNewPicture(XmlNode pictureNode, string name)
		{
			var result = new ProductPicture();

			if (pictureNode == null || string.IsNullOrWhiteSpace(pictureNode.FirstChild.Value))
			{
				throw new Exception(string.Format(_localizationService.GetResource("Plugins.Import.ItLink.NoImage"), name));
			}

			using (WebClient client = new WebClient())
			{
				byte[] binaryData;
				try
				{
					binaryData = client.DownloadData(pictureNode.FirstChild.Value);

					var picture = new Core.Domain.Media.Picture
					{
						MimeType = MimeTypes.ImagePJpeg,
						AltAttribute = name,
						TitleAttribute = name,
						SeoFilename = SeoExtensions.GetSeName(name),
						PictureBinary = binaryData
					};

					var newProductPicture = _pictureService.InsertPicture(
						picture.PictureBinary,
						picture.MimeType,
						picture.SeoFilename);

					result = new ProductPicture
					{
						//EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
						//pictures are duplicated
						//maybe because entity size is too large
						PictureId = newProductPicture.Id,
						DisplayOrder = 1,
					};
				}
				catch
				{
					throw;
				}

				return result;
			}
		}
	}
}

