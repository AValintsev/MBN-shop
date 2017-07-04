using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.ExportImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using Nop.Services.Seo;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class XmlToXlsConverter : IXmlToXlsConverter
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		private readonly IWorkContext _workContext;

		private readonly IExportManager _exportManager;
		private readonly IImportManager _importManager;

		private readonly string PICTURES_FOLDER = System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Import.ItLink/pictures");

		public XmlToXlsConverter(
			ICategoryService categoryService,
			IProductService productService,
			IWorkContext workContext,
			IExportManager exportManager,
			IImportManager importManager)
		{
			_categoryService = categoryService;
			_productService = productService;
			_workContext = workContext;
			this._exportManager = exportManager;
			this._importManager = importManager;
		}

		private Dictionary<string, int> GetCategoriesMapping()
		{
			Dictionary<string, int> result = new Dictionary<string, int>();

			try
			{
				string path = CommonHelper.MapPath("~/Plugins/Import.ItLink/App_Data/CategoriesMapping.xml");

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(path);

				foreach (XmlNode category in xmlDoc.GetElementsByTagName("category"))
				{
					result.Add(category.Attributes["id"].Value, int.Parse(category.Attributes["MbnId"].Value));
				}

				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public Stream XmlToXlsx(XmlDocument recievedDocument)
		{
			XmlNodeList offers = recievedDocument.GetElementsByTagName("offer");

			Dictionary<string, int> mappedCategories = GetCategoriesMapping();

			List<Product> productsList = new List<Product>();

			foreach (XmlNode offer in offers)
			{
				#region Product

				var product = new Product
				{
					Name = offer["name"].FirstChild.Value,
					ShortDescription = offer["name"].FirstChild.Value,
					FullDescription = offer["name"].FirstChild.Value,
					Sku = offer["vendorCode"].FirstChild.Value,

					Price = decimal.Parse(offer["param"].FirstChild.Value),

					VisibleIndividually = true,
					ProductType = ProductType.SimpleProduct,
					ParentGroupedProductId = 0,
					ProductTemplateId = 1,
					ShowOnHomePage = false,
					VendorId = 0,
					AllowCustomerReviews = true,
					Published = true,
					IsGiftCard = false,
					GiftCardType = GiftCardType.Virtual,
					RequireOtherProducts = false,
					AutomaticallyAddRequiredProducts = false,
					IsDownload = false,
					DownloadId = 0,
					IsRecurring = false,
					IsRental = false,
					IsShipEnabled = true,
					IsFreeShipping = false,
					ShipSeparately = false,
					AdditionalShippingCharge = 0,
					DeliveryDateId = 0,
					IsTaxExempt = false,
					OrderMinimumQuantity = 1,
					DisplayStockAvailability = false,
					DisplayStockQuantity = false,

				};

				#endregion

				#region product picture

				//Setup product pictures
				if (offer["picture"] != null && !string.IsNullOrWhiteSpace(offer["picture"].FirstChild.Value))
				{
					using (WebClient client = new WebClient())
					{
						byte[] binaryData;
						try
						{
							binaryData = client.DownloadData(offer["picture"].FirstChild.Value);
							product.ProductPictures.Add(new ProductPicture
							{
								Picture = new Core.Domain.Media.Picture
								{
									MimeType = MimeTypes.ImagePJpeg,
									AltAttribute = offer["name"].FirstChild.Value,
									TitleAttribute = offer["name"].FirstChild.Value,
									SeoFilename = SeoExtensions.GetSeName(product.Name),
									PictureBinary = binaryData
								}
							});
						}
						catch
						{
						}
					}
				}

				#endregion

				#region Map category

				product.ProductCategories.Add(
					new ProductCategory
					{
						CategoryId = mappedCategories[offer["categoryId"].FirstChild.Value],
					});

				#endregion
				
				productsList.Add(product);
			}

			return new MemoryStream(_exportManager.ExportProductsToXlsx(productsList));
		}

	}
}

