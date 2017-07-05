using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using System;
using System.Collections.Generic;
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
			this._urlRecordService = urlRecordService;
			this._manufacturerService = manufacturerService;
			this._localizationService = localizationService;
		}

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

		/// <summary>
		/// Imports products from ItLinks xml.
		/// </summary>
		/// <param name="xmlDocument">xml document</param>
		/// <param name="categoriesMap">dictionary key = ItLink cateogry name, value = internal category Id</param>
		/// <param name="overrideExistedImanges">true if want to override existed images (could be clower).</param>
		/// <returns>Lists with errors.</returns>
		public List<string> Import(XmlDocument xmlDocument, Dictionary<string, int> categoriesMap, bool overrideExistedImanges = false)
		{
			var result = new List<string>();

			try
			{
				XmlNodeList offers = xmlDocument.GetElementsByTagName("offer");

				List<string> allSku = new List<string>(offers.Count);
				List<string> manufacturerNames = new List<string>();

				foreach (XmlNode offer in offers)
				{
					allSku.Add(offer["vendorCode"].InnerText);
					manufacturerNames.Add(offer["vendor"].InnerText);
				}

				manufacturerNames = manufacturerNames.Distinct().ToList();

				//performance optimization, load all products by SKU in one SQL request
				var allProductsBySku = _productService.GetProductsBySku(allSku.ToArray(), VendorId);

				//performance optimization, load all manufacturers IDs for products in one SQL request
				var allProductsManufacturerIds = _manufacturerService.GetProductManufacturerIds(allProductsBySku.Select(p => p.Id).ToArray());

				//performance optimization, load all manufacturers in one SQL request
				var allManufacturers = _manufacturerService.GetAllManufacturers(showHidden: true);

				foreach (XmlNode offer in offers)
				{
					try
					{
						var sku = offer["vendorCode"].InnerText;
						var product = allProductsBySku.FirstOrDefault(p => p.Sku == sku);

						var isNew = product == null;

						product = product ?? new Product();

						if (isNew)
						{
							product.CreatedOnUtc = DateTime.UtcNow;
						}

						#region Setup product

						var productName = offer["name"].InnerText;
						product.ProductType = ProductType.SimpleProduct;
						product.VisibleIndividually = true;
						product.Name = productName;
						product.ShortDescription = productName;
						product.FullDescription = productName;
						product.VendorId = VendorId;
						product.ProductTemplateId = 1;
						product.ShowOnHomePage = false;
						product.MetaKeywords = string.Format("{0}; {1}", offer["vendor"].InnerText, offer["vendorCode"].InnerText);
						product.MetaDescription = productName;
						product.MetaTitle = productName;
						product.AllowCustomerReviews = true;
						product.Published = true;
						product.Sku = sku;
						product.ManufacturerPartNumber = offer["vendorCode"].InnerText;
						product.IsGiftCard = false;
						product.RequireOtherProducts = false;
						product.IsShipEnabled = true;
						product.StockQuantity = 1;
						product.DisplayStockQuantity = true; ;
						product.OldPrice = isNew ? product.OldPrice : product.Price;
						product.Price = decimal.Parse(offer["param"].FirstChild.Value);
						product.MarkAsNew = isNew;

						product.ProductCategories.Add(
							new ProductCategory
							{
								CategoryId = categoriesMap[offer["categoryId"].InnerText]
							});

						#endregion

						if (isNew)
						{
							_productService.InsertProduct(product);
						}
						else
						{
							_productService.UpdateProduct(product);
						}

						_urlRecordService.SaveSlug(product, product.ValidateSeName(product.GetSeName(), product.Name, true), _workContext.WorkingLanguage.Id);

						#region product Manufacturers

						var manufacturers = isNew || !allProductsManufacturerIds.ContainsKey(product.Id)
							? new int[0]
							: allProductsManufacturerIds[product.Id];
						var importedManufacturers = manufacturerNames.Select(x => allManufacturers.First(m => m.Name == x.Trim()).Id).ToList();
						foreach (var manufacturerId in importedManufacturers)
						{
							if (manufacturers.Any(c => c == manufacturerId))
								continue;

							var productManufacturer = new ProductManufacturer
							{
								ProductId = product.Id,
								ManufacturerId = manufacturerId,
								IsFeaturedProduct = false,
								DisplayOrder = 1
							};
							_manufacturerService.InsertProductManufacturer(productManufacturer);
						}

						//delete product manufacturers
						var deletedProductsManufacturers = manufacturers
								.Where(manufacturerId => !importedManufacturers.Contains(manufacturerId))
								.Select(manufacturerId => product.ProductManufacturers.First(pc => pc.ManufacturerId == manufacturerId));
						foreach (var deletedProductManufacturer in deletedProductsManufacturers)
						{
							_manufacturerService.DeleteProductManufacturer(deletedProductManufacturer);
						}

						#endregion

						#region product Pictures

						if (isNew || (!isNew && overrideExistedImanges))
						{
							product.ProductPictures.Add(DownloadNewPicture(offer["picture"], product.Name));
						}

						#endregion

						product.UpdatedOnUtc = DateTime.UtcNow;
						_productService.UpdateProduct(product);
					}
					catch (Exception e)
					{
						result.Add(e.Message);
					}
				}
			}
			catch (Exception e)
			{
				result.Add(e.Message);
			}

			return result;
		}

		private ProductPicture DownloadNewPicture(XmlNode pictureNode, string name)
		{
			var result = new ProductPicture();

			if (pictureNode == null && !string.IsNullOrWhiteSpace(pictureNode.FirstChild.Value))
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

