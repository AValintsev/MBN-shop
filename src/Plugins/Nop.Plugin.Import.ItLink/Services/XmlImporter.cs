using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Date;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Nop.Services.ExportImport;
using System.IO;
using System.Web;
using System.Web.WebPages;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class XmlImporter : IXmlImporter
	{
		#region Fields

		private readonly IProductService _productService;
		private readonly IProductAttributeService _productAttributeService;
		private readonly ICategoryService _categoryService;
		private readonly IManufacturerService _manufacturerService;
		private readonly IPictureService _pictureService;
		private readonly IUrlRecordService _urlRecordService;
		private readonly IStoreContext _storeContext;
		private readonly ICountryService _countryService;
		private readonly IStateProvinceService _stateProvinceService;
		private readonly IEncryptionService _encryptionService;
		private readonly IDataProvider _dataProvider;
		private readonly MediaSettings _mediaSettings;
		private readonly IVendorService _vendorService;
		private readonly IProductTemplateService _productTemplateService;
		private readonly IShippingService _shippingService;
		private readonly IDateRangeService _dateRangeService;
		private readonly ITaxCategoryService _taxCategoryService;
		private readonly IMeasureService _measureService;
		private readonly CatalogSettings _catalogSettings;
		private readonly IProductTagService _productTagService;
		private readonly IWorkContext _workContext;
		private readonly ILocalizationService _localizationService;
		private readonly VendorSettings _vendorSettings;

		#endregion

		#region Ctor

		public XmlImporter(IProductService productService,
			ICategoryService categoryService,
			IManufacturerService manufacturerService,
			IPictureService pictureService,
			IUrlRecordService urlRecordService,
			IStoreContext storeContext,
			ICountryService countryService,
			IStateProvinceService stateProvinceService,
			IEncryptionService encryptionService,
			IDataProvider dataProvider,
			MediaSettings mediaSettings,
			IVendorService vendorService,
			IProductTemplateService productTemplateService,
			IShippingService shippingService,
			IDateRangeService dateRangeService,
			ITaxCategoryService taxCategoryService,
			IMeasureService measureService,
			IProductAttributeService productAttributeService,
			CatalogSettings catalogSettings,
			IProductTagService productTagService,
			IWorkContext workContext,
			ILocalizationService localizationService,
			VendorSettings vendorSettings)
		{
			this._productService = productService;
			this._categoryService = categoryService;
			this._manufacturerService = manufacturerService;
			this._pictureService = pictureService;
			this._urlRecordService = urlRecordService;
			this._storeContext = storeContext;
			this._countryService = countryService;
			this._stateProvinceService = stateProvinceService;
			this._encryptionService = encryptionService;
			this._dataProvider = dataProvider;
			this._mediaSettings = mediaSettings;
			this._vendorService = vendorService;
			this._productTemplateService = productTemplateService;
			this._shippingService = shippingService;
			this._dateRangeService = dateRangeService;
			this._taxCategoryService = taxCategoryService;
			this._measureService = measureService;
			this._productAttributeService = productAttributeService;
			this._catalogSettings = catalogSettings;
			this._productTagService = productTagService;
			this._workContext = workContext;
			this._localizationService = localizationService;
			this._vendorSettings = vendorSettings;
		}

		#endregion


		public void ImportXml(XmlDocument recievedDocument)
		{
			XmlNodeList categs = recievedDocument.GetElementsByTagName("category");

			//MATCHING CATEGORIES
			foreach (XmlNode cat in categs)
			{
				switch (cat.InnerText)
				{
					case "Подставки для ноутбуков":
						cat.InnerText = "Аксессуары";
						break;
					case "Кресла":
						cat.InnerText = "Аксессуары";
						break;
					case "Вентиляторы":
						cat.InnerText = "Аксессуары";
						break;
					case "Источники питания":
						cat.InnerText = "Аксессуары";
						break;
					case "Кабели, шлейфы":
						cat.InnerText = "Кабели";
						break;
					case "Клавиатуры":
						cat.InnerText = "Клавиатуры и мыши";
						break;
					case "Игровые поверхности":
						cat.InnerText = "Коврики";
						break;
					case "Корпуса для ПК":
						cat.InnerText = "Корпуса";
						break;
					case "Кулеры для видеокарт, чипсетов и жестких дисков":
						cat.InnerText = "Охлаждение";
						break;
					case "Кулеры для процессоров":
						cat.InnerText = "Охлаждение";
						break;
					case "Модули памяти":
						cat.InnerText = "Оперативная память";
						break;
					case "Мыши":
						cat.InnerText = "Клавиатуры и мыши";
						break;
					case "Жесткие диски":
						cat.InnerText = "HDD диски";
						break;
					case "Адаптеры, переходники":
						cat.InnerText = "Переходники";
						break;
					case "Звуковые карты":
						cat.InnerText = "Звуковые карты";
						break;
					case "Геймпады":
						cat.InnerText = "Аксессуары";
						break;
					case "Карманы и Rack устройства":
						cat.InnerText = "Аксессуары";
						break;
					case "Контроллеры, интерфейсные платы PCI/PCIE": //????????
						cat.InnerText = "Аксессуары";
						break;
					case "Хабы USB и кард-ридеры":
						cat.InnerText = "Аксессуары";
						break;
					case "Батареи мобильные":               //?????
						cat.InnerText = "Аксессуары";
						break;
					case "Акустика":                        //??
						cat.InnerText = "Аксессуары";
						break;
					case "Наушники":
						cat.InnerText = "Аксессуары";
						break;
					case "Зарядные устройства":
						cat.InnerText = "Аксессуары";
						break;
					case "Сетевые фильтры":                 //??
						cat.InnerText = "Аксессуары";
						break;
					case "Коннекторы силовые, изолента":        //??
						cat.InnerText = "Аксессуары";
						break;
					case "Выключатели силовые и розетки":       //???
						cat.InnerText = "Аксессуары";
						break;
				}
			}





			XmlNodeList offers = recievedDocument.GetElementsByTagName("offer");

			const int NUMBER_OF_COLS = 94;



			#region Product

			Product product = new Product();
			product.ProductType = ProductType.SimpleProduct;
			product.ParentGroupedProductId = 0;
			product.VisibleIndividually = true;
			product.Name = "Name";
			product.ShortDescription = "ShortDescription";
			product.FullDescription = "FullDescription";
			product.VendorId = _workContext.CurrentVendor != null ? _workContext.CurrentVendor.Id : 0;
			product.ProductTemplateId = 0;
			product.ShowOnHomePage = false;
			//Metakeywords
			//MetaDescription
			//Metatitle
			product.Name = "SeName";
			product.AllowCustomerReviews = true;
			product.Published = true;
			product.Sku = "SKU";
			//product.ManufacturerPartNumber = 0;
			//product.Gtin
			product.IsGiftCard = false;
			product.GiftCardType = GiftCardType.Virtual;
			//product.OverriddenGiftCardAmount
			product.RequireOtherProducts = false;
			//product.RequiredProductIds
			product.AutomaticallyAddRequiredProducts = false;
			product.IsDownload = false;
			product.DownloadId = 0;
			product.UnlimitedDownloads = true;
			product.MaxNumberOfDownloads = 10;
			product.DownloadActivationType = DownloadActivationType.WhenOrderIsPaid;
			product.HasSampleDownload = false;
			product.SampleDownloadId = 0;
			product.HasUserAgreement = false;
			//product.UserAgreementText
			product.IsRecurring = false;
			product.RecurringCycleLength = 100;
			product.RecurringCyclePeriod = RecurringProductCyclePeriod.Days;
			product.RecurringTotalCycles = 10;
			product.IsRental = false;
			product.RentalPriceLength = 1;
			product.RentalPricePeriod = RentalPricePeriod.Days;
			product.IsShipEnabled = true;
			product.IsFreeShipping = false;
			product.ShipSeparately = false;
			product.AdditionalShippingCharge = 0;
			product.DeliveryDateId = 0;
			product.IsTaxExempt = false; //or true
										 //product.TaxCategoryId =;
			product.IsTelecommunicationsOrBroadcastingOrElectronicServices = false;
			product.ManageInventoryMethod = ManageInventoryMethod.DontManageStock; //or other
			product.ProductAvailabilityRangeId = 0;
			product.UseMultipleWarehouses = false;
			product.WarehouseId = 0;
			product.StockQuantity = 10; // value
			product.DisplayStockAvailability = false;
			product.DisplayStockQuantity = false;
			product.MinStockQuantity = 0;
			product.LowStockActivity = LowStockActivity.Nothing;
			product.NotifyAdminForQuantityBelow = 1;
			product.BackorderMode = BackorderMode.NoBackorders;
			product.AllowBackInStockSubscriptions = false;
			product.OrderMinimumQuantity = 1;
			product.OrderMaximumQuantity = 10000;
			//product.AllowedQuantities
			product.AllowAddingOnlyExistingAttributeCombinations = false;
			product.NotReturnable = false;
			product.DisableBuyButton = false;
			product.DisableWishlistButton = false;
			product.AvailableForPreOrder = false;
			//product.PreOrderAvailabilityStartDateTimeUtc
			product.CallForPrice = false;
			product.Price = 100; //value
			product.OldPrice = 0;
			product.ProductCost = 0;
			product.CustomerEntersPrice = false;
			product.MinimumCustomerEnteredPrice = 0;
			product.MaximumCustomerEnteredPrice = 1000;
			product.BasepriceEnabled = false;
			product.BasepriceAmount = 0;
			product.BasepriceUnitId = 0;
			product.BasepriceBaseAmount = 0;
			product.BasepriceBaseUnitId = 0;
			product.MarkAsNew = true;
			//product.MarkAsNewStartDateTimeUtc;
			//product.MarkAsNewEndDateTimeUtc;
			product.Weight = 0;
			product.Length = 0;
			product.Width = 0;
			product.Height = 0;
			//product.ProductCategories.Add();
			//product.ProductManufacturers = null;
			//product.ProductTags
			//product.ProductPictures.Add(new ProductPicture { i})

			#endregion


			var allCategoriesNames = new List<string>();
			var allSku = new List<string>();

			foreach (XmlNode categ in categs)
			{
				allCategoriesNames.Add(categ.InnerText);
			}

			foreach (XmlNode offer in offers)
			{
				var sku = offer["categoryId"].InnerText + offer.Attributes["id"].Value;

				if (!sku.IsEmpty())
					allSku.Add(sku);
			}
			var StoreCategories = _categoryService.GetAllCategories();



			var countProductsInFile = 0;


			//performance optimization, the check for the existence of the categories in one SQL request
			var notExistingCategories = _categoryService.GetNotExistingCategories(allCategoriesNames.ToArray());
			if (notExistingCategories.Any())
			{
				throw new ArgumentException(string.Format("The following category name(s) don't exist - {0}", string.Join(", ", notExistingCategories)));
			}


			//performance optimization, load all products by SKU in one SQL request
			var allProductsBySku = _productService.GetProductsBySku(allSku.ToArray(), _workContext.CurrentVendor.Return(v => v.Id, 0));

			//validate maximum number of products per vendor
			if (_vendorSettings.MaximumProductNumber > 0 &&
				_workContext.CurrentVendor != null)
			{
				var newProductsCount = countProductsInFile - allProductsBySku.Count;
				if (_productService.GetNumberOfProductsByVendorId(_workContext.CurrentVendor.Id) + newProductsCount > _vendorSettings.MaximumProductNumber)
					throw new ArgumentException(string.Format(_localizationService.GetResource("Admin.Catalog.Products.ExceededMaximumNumber"), _vendorSettings.MaximumProductNumber));
			}


			//performance optimization, load all categories IDs for products in one SQL request
			var allProductsCategoryIds = _categoryService.GetProductCategoryIds(allProductsBySku.Select(p => p.Id).ToArray());

			//performance optimization, load all categories in one SQL request
			var allCategories = _categoryService.GetAllCategories(showHidden: true);

			//performance optimization, load all manufacturers IDs for products in one SQL request
			var allProductsManufacturerIds = _manufacturerService.GetProductManufacturerIds(allProductsBySku.Select(p => p.Id).ToArray());

			//performance optimization, load all manufacturers in one SQL request
			var allManufacturers = _manufacturerService.GetAllManufacturers(showHidden: true);

			//product to import images
			var productPictureMetadata = new List<ProductPictureMetadata>();

			//some of previous values
			var previousStockQuantity = product.StockQuantity;
			var previousWarehouseId = product.WarehouseId;

			var manufacturer = _manufacturerService.GetManufacturerById(0);

			var isNew = manufacturer == null;

			manufacturer = manufacturer ?? new Manufacturer();

			if (isNew)
			{
				manufacturer.CreatedOnUtc = DateTime.UtcNow;

				//default values
				manufacturer.PageSize = _catalogSettings.DefaultManufacturerPageSize;
				manufacturer.PageSizeOptions = _catalogSettings.DefaultManufacturerPageSizeOptions;
				manufacturer.Published = true;
				manufacturer.AllowCustomersToSelectPageSize = true;
			}

			var seName = string.Empty;


			foreach (XmlNode offer in offers)
			{
				product.Name = offer["name"].InnerText;
				product.OldPrice = int.Parse(offer["oldprice"].InnerText);
				product.Price = int.Parse(offer["price"].InnerText);






				product.UpdatedOnUtc = DateTime.UtcNow;
				product.CreatedOnUtc = DateTime.UtcNow;

				if (isNew)
				{
					_productService.InsertProduct(product);
				}
				else
				{
					_productService.UpdateProduct(product);
				}

				//quantity change history
				if (isNew || previousWarehouseId == product.WarehouseId)
				{
					_productService.AddStockQuantityHistoryEntry(product, product.StockQuantity - previousStockQuantity, product.StockQuantity,
						product.WarehouseId, _localizationService.GetResource("Admin.StockQuantityHistory.Messages.ImportProduct.Edit"));
				}
				//warehouse is changed 
				else
				{
					//compose a message
					var oldWarehouseMessage = string.Empty;
					if (previousWarehouseId > 0)
					{
						var oldWarehouse = _shippingService.GetWarehouseById(previousWarehouseId);
						if (oldWarehouse != null)
							oldWarehouseMessage = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.EditWarehouse.Old"), oldWarehouse.Name);
					}
					var newWarehouseMessage = string.Empty;
					if (product.WarehouseId > 0)
					{
						var newWarehouse = _shippingService.GetWarehouseById(product.WarehouseId);
						if (newWarehouse != null)
							newWarehouseMessage = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.EditWarehouse.New"), newWarehouse.Name);
					}
					var message = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.ImportProduct.EditWarehouse"), oldWarehouseMessage, newWarehouseMessage);

					//record history
					_productService.AddStockQuantityHistoryEntry(product, -previousStockQuantity, 0, previousWarehouseId, message);
					_productService.AddStockQuantityHistoryEntry(product, product.StockQuantity, product.StockQuantity, product.WarehouseId, message);
				}

				var tempProperty = product.Name;
				if (tempProperty != null)
				{
					seName = tempProperty;
					//search engine name
					_urlRecordService.SaveSlug(product, product.ValidateSeName(seName, product.Name, true), 0);
				}

				XmlNode categoryxml = recievedDocument.SelectSingleNode("//categories/category[@id='" + offer["categoryId"].InnerText + "']");

				tempProperty = categoryxml.InnerText;

				//foreach (XmlNode categ in categs)
				//{
				//	if (categ.Attributes["id"].Value == offer["categoryId"].InnerText)
				//	{
				//		tempProperty = categ.InnerText;
				//		break;
				//	}
				//}


				if (tempProperty != null)
				{
					var categoryNames = tempProperty;

					//category mappings
					var categories = isNew || !allProductsCategoryIds.ContainsKey(product.Id) ? new int[0] : allProductsCategoryIds[product.Id];
					var importedCategories = categoryNames.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => allCategories.First(c => c.Name == x.Trim()).Id).ToList();
					foreach (var categoryId in importedCategories)
					{
						if (categories.Any(c => c == categoryId))
							continue;

						var productCategory = new ProductCategory
						{
							ProductId = product.Id,
							CategoryId = categoryId,
							IsFeaturedProduct = false,
							DisplayOrder = 1
						};
						_categoryService.InsertProductCategory(productCategory);
					}

					//delete product categories
					var deletedProductCategories = categories.Where(categoryId => !importedCategories.Contains(categoryId))
							.Select(categoryId => product.ProductCategories.First(pc => pc.CategoryId == categoryId));
					foreach (var deletedProductCategory in deletedProductCategories)
					{
						_categoryService.DeleteProductCategory(deletedProductCategory);
					}
					//}

					//tempProperty = manager.GetProperty("Manufacturers");
					//if (tempProperty != null)
					//{
					//	var manufacturerNames = tempProperty.StringValue;

					//	//manufacturer mappings
					//	var manufacturers = isNew || !allProductsManufacturerIds.ContainsKey(product.Id) ? new int[0] : allProductsManufacturerIds[product.Id];
					//	var importedManufacturers = manufacturerNames.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => allManufacturers.First(m => m.Name == x.Trim()).Id).ToList();
					//	foreach (var manufacturerId in importedManufacturers)
					//	{
					//		if (manufacturers.Any(c => c == manufacturerId))
					//			continue;

					//		var productManufacturer = new ProductManufacturer
					//		{
					//			ProductId = product.Id,
					//			ManufacturerId = manufacturerId,
					//			IsFeaturedProduct = false,
					//			DisplayOrder = 1
					//		};
					//		_manufacturerService.InsertProductManufacturer(productManufacturer);
					//	}

					//	//delete product manufacturers
					//	var deletedProductsManufacturers = manufacturers.Where(manufacturerId => !importedManufacturers.Contains(manufacturerId))
					//			.Select(manufacturerId => product.ProductManufacturers.First(pc => pc.ManufacturerId == manufacturerId));
					//	foreach (var deletedProductManufacturer in deletedProductsManufacturers)
					//	{
					//		_manufacturerService.DeleteProductManufacturer(deletedProductManufacturer);
					//	}
					//}

					//tempProperty = manager.GetProperty("ProductTags");
					//if (tempProperty != null)
					//{
					//	var productTags = tempProperty.StringValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

					//	//product tag mappings
					//	_productTagService.UpdateProductTags(product, productTags);
					//}

					//var picture1 = offer["picture"].InnerText;

					productPictureMetadata.Add(new ProductPictureMetadata
					{
						ProductItem = product,
						//Picture1Path = /*picture1*/,
						IsNew = isNew
					});



				}

				if (_mediaSettings.ImportProductImagesUsingHash && _pictureService.StoreInDb && _dataProvider.SupportedLengthOfBinaryHash() > 0)
					ImportProductImagesUsingHash(productPictureMetadata, allProductsBySku);
				else
					ImportProductImagesUsingServices(productPictureMetadata);

			}
		}

		#region Nested classes

		protected class ProductPictureMetadata
		{
			public Product ProductItem { get; set; }
			public string Picture1Path { get; set; }
			public string Picture2Path { get; set; }
			public string Picture3Path { get; set; }
			public bool IsNew { get; set; }
		}

		#endregion


		protected virtual void ImportProductImagesUsingServices(IList<ProductPictureMetadata> productPictureMetadata)
		{
			foreach (var product in productPictureMetadata)
			{
				foreach (var picturePath in new[] { product.Picture1Path, product.Picture2Path, product.Picture3Path })
				{
					if (String.IsNullOrEmpty(picturePath))
						continue;

					var mimeType = GetMimeTypeFromFilePath(picturePath);
					var newPictureBinary = File.ReadAllBytes(picturePath);
					var pictureAlreadyExists = false;
					if (!product.IsNew)
					{
						//compare with existing product pictures
						var existingPictures = _pictureService.GetPicturesByProductId(product.ProductItem.Id);
						foreach (var existingPicture in existingPictures)
						{
							var existingBinary = _pictureService.LoadPictureBinary(existingPicture);
							//picture binary after validation (like in database)
							var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
							if (!existingBinary.SequenceEqual(validatedPictureBinary) &&
								!existingBinary.SequenceEqual(newPictureBinary))
								continue;
							//the same picture content
							pictureAlreadyExists = true;
							break;
						}
					}

					if (pictureAlreadyExists)
						continue;
					var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(product.ProductItem.Name));
					product.ProductItem.ProductPictures.Add(new ProductPicture
					{
						//EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
						//pictures are duplicated
						//maybe because entity size is too large
						PictureId = newPicture.Id,
						DisplayOrder = 1,
					});
					_productService.UpdateProduct(product.ProductItem);
				}
			}
		}

		protected virtual string GetMimeTypeFromFilePath(string filePath)
		{
			var mimeType = MimeMapping.GetMimeMapping(filePath);

			//little hack here because MimeMapping does not contain all mappings (e.g. PNG)
			if (mimeType == MimeTypes.ApplicationOctetStream)
				mimeType = MimeTypes.ImageJpeg;

			return mimeType;
		}

		protected virtual void ImportProductImagesUsingHash(IList<ProductPictureMetadata> productPictureMetadata, IList<Product> allProductsBySku)
		{
			//performance optimization, load all pictures hashes
			//it will only be used if the images are stored in the SQL Server database (not compact)
			var takeCount = _dataProvider.SupportedLengthOfBinaryHash() - 1;
			var productsImagesIds = _productService.GetProductsImagesIds(allProductsBySku.Select(p => p.Id).ToArray());
			var allPicturesHashes = _pictureService.GetPicturesHash(productsImagesIds.SelectMany(p => p.Value).ToArray());

			foreach (var product in productPictureMetadata)
			{
				foreach (var picturePath in new[] { product.Picture1Path, product.Picture2Path, product.Picture3Path })
				{
					if (String.IsNullOrEmpty(picturePath))
						continue;

					var mimeType = GetMimeTypeFromFilePath(picturePath);
					var newPictureBinary = File.ReadAllBytes(picturePath);
					var pictureAlreadyExists = false;
					if (!product.IsNew)
					{
						var newImageHash = _encryptionService.CreateHash(newPictureBinary.Take(takeCount).ToArray());
						var newValidatedImageHash = _encryptionService.CreateHash(_pictureService.ValidatePicture(newPictureBinary, mimeType).Take(takeCount).ToArray());

						var imagesIds = productsImagesIds.ContainsKey(product.ProductItem.Id)
							? productsImagesIds[product.ProductItem.Id]
							: new int[0];

						pictureAlreadyExists = allPicturesHashes.Where(p => imagesIds.Contains(p.Key)).Select(p => p.Value).Any(p => p == newImageHash || p == newValidatedImageHash);
					}

					if (pictureAlreadyExists)
						continue;
					var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(product.ProductItem.Name));
					product.ProductItem.ProductPictures.Add(new ProductPicture
					{
						//EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
						//pictures are duplicated
						//maybe because entity size is too large
						PictureId = newPicture.Id,
						DisplayOrder = 1,
					});
					_productService.UpdateProduct(product.ProductItem);
				}
			}
		}

	}
}
