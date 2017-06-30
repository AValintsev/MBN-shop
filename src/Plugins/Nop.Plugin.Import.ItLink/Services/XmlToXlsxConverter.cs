using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using OfficeOpenXml;
using System.IO;
using System.Net;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class XmlToXlsConverter : IXmlToXlsConverter
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		private readonly IWorkContext _workContext;

		public const int NUMBER_OF_COLS = 94;

		public XmlToXlsConverter(
			ICategoryService categoryService,
			IProductService productService,
			IWorkContext workContext)
		{
			_categoryService = categoryService;
			_productService = productService;
			_workContext = workContext;
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



			int endrow = offers.Count + 1;

			var path = System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Import.ItLink/products.xlsx");

			FileInfo file = new FileInfo(path);
			Product product = new Product();

			using (ExcelPackage xlPackage = new ExcelPackage(file))
			{
				ExcelWorksheet excellproducts = xlPackage.Workbook.Worksheets.FirstOrDefault();


				int row = 2;

				#region Product



				product.ProductType = ProductType.SimpleProduct;
				product.ParentGroupedProductId = 0;
				product.VisibleIndividually = true;

				product.ShortDescription = "ShortDescription";
				product.FullDescription = "FullDescription";
				product.VendorId = _workContext.CurrentVendor != null ? _workContext.CurrentVendor.Id : 0;
				product.ProductTemplateId = 0;
				product.ShowOnHomePage = false;
				//Metakeywords
				//MetaDescription
				//Metatitle
				//product.Name = "SeName";
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
				//product.ProductTags
				string productTags = "";
				//product.ProductPictures.Add(new ProductPicture { i})
				//How to save images
				bool hasPicture;
				string filename = "";
				var picturesFolder = System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Import.ItLink/pictures");

				#endregion

				foreach (XmlNode offer in offers)
				{

					product.Name = offer["name"].InnerText; //4
					product.Price = int.Parse(offer["price"].InnerText); //71
					product.OldPrice = int.Parse(offer["oldprice"].InnerText); //72
																			   //product.Categories
					hasPicture = offer["picture"] != null;
					if (hasPicture)
					{
						try
						{
							using (WebClient client = new WebClient())
							{
								filename = Path.GetFileName(offer["picture"].InnerText);

								client.DownloadFile(offer["picture"].InnerText, string.Format("{0}/{1}", picturesFolder, filename));
							}
						}
						catch (Exception ex)
						{
							hasPicture = false;
							filename = "";
						}
					}
					using (System.IO.StreamWriter Testfile = new System.IO.StreamWriter(@"C:\ROWS.txt"))
					{
						Testfile.WriteLine("Number : {0}, ID: {1}", row, offer["url"].InnerText);
					}
					int categoryId = mappedCategories[offer["categoryId"].InnerText];
					var category = _categoryService.GetCategoryById(categoryId);
					//product.ProductManufacturers ;
					string manufactor = "";/*offer["vendor"].InnerText;*/


					#region Writing to xlsx

					int cell = 1;


					excellproducts.Cells[row, cell++].Value = product.ProductType;
					excellproducts.Cells[row, cell++].Value = product.ParentGroupedProductId;
					excellproducts.Cells[row, cell++].Value = product.VisibleIndividually;
					excellproducts.Cells[row, cell++].Value = product.Name;
					excellproducts.Cells[row, cell++].Value = product.ShortDescription;
					excellproducts.Cells[row, cell++].Value = product.FullDescription;
					excellproducts.Cells[row, cell++].Value = product.VendorId;
					excellproducts.Cells[row, cell++].Value = product.ProductTemplateId;
					excellproducts.Cells[row, cell++].Value = product.ShowOnHomePage;
					excellproducts.Cells[row, cell++].Value = product.MetaKeywords;
					excellproducts.Cells[row, cell++].Value = product.MetaDescription;
					excellproducts.Cells[row, cell++].Value = product.MetaTitle;
					excellproducts.Cells[row, cell++].Value = product.Name;
					excellproducts.Cells[row, cell++].Value = product.AllowCustomerReviews;
					excellproducts.Cells[row, cell++].Value = product.Published;
					excellproducts.Cells[row, cell++].Value = product.Sku;
					excellproducts.Cells[row, cell++].Value = product.ManufacturerPartNumber;
					excellproducts.Cells[row, cell++].Value = product.Gtin;
					excellproducts.Cells[row, cell++].Value = product.IsGiftCard;
					excellproducts.Cells[row, cell++].Value = product.GiftCardType;
					excellproducts.Cells[row, cell++].Value = product.OverriddenGiftCardAmount;
					excellproducts.Cells[row, cell++].Value = product.RequireOtherProducts;
					excellproducts.Cells[row, cell++].Value = product.RequiredProductIds;
					excellproducts.Cells[row, cell++].Value = product.AutomaticallyAddRequiredProducts;
					excellproducts.Cells[row, cell++].Value = product.IsDownload;
					excellproducts.Cells[row, cell++].Value = product.DownloadId;
					excellproducts.Cells[row, cell++].Value = product.UnlimitedDownloads;
					excellproducts.Cells[row, cell++].Value = product.MaxNumberOfDownloads;
					excellproducts.Cells[row, cell++].Value = product.DownloadActivationType;
					excellproducts.Cells[row, cell++].Value = product.HasSampleDownload;
					excellproducts.Cells[row, cell++].Value = product.SampleDownloadId;
					excellproducts.Cells[row, cell++].Value = product.HasUserAgreement;
					excellproducts.Cells[row, cell++].Value = product.UserAgreementText;
					excellproducts.Cells[row, cell++].Value = product.IsRecurring;
					excellproducts.Cells[row, cell++].Value = product.RecurringCycleLength;
					excellproducts.Cells[row, cell++].Value = product.RecurringCyclePeriod;
					excellproducts.Cells[row, cell++].Value = product.RecurringTotalCycles;
					excellproducts.Cells[row, cell++].Value = product.IsRental;
					excellproducts.Cells[row, cell++].Value = product.RentalPriceLength;
					excellproducts.Cells[row, cell++].Value = product.RentalPricePeriod;
					excellproducts.Cells[row, cell++].Value = product.IsShipEnabled;
					excellproducts.Cells[row, cell++].Value = product.IsFreeShipping;
					excellproducts.Cells[row, cell++].Value = product.ShipSeparately;
					excellproducts.Cells[row, cell++].Value = product.AdditionalShippingCharge;
					excellproducts.Cells[row, cell++].Value = product.DeliveryDateId;
					excellproducts.Cells[row, cell++].Value = product.IsTaxExempt;
					excellproducts.Cells[row, cell++].Value = product.TaxCategoryId;
					excellproducts.Cells[row, cell++].Value = product.IsTelecommunicationsOrBroadcastingOrElectronicServices;
					excellproducts.Cells[row, cell++].Value = product.ManageInventoryMethod;
					excellproducts.Cells[row, cell++].Value = product.ProductAvailabilityRangeId;
					excellproducts.Cells[row, cell++].Value = product.UseMultipleWarehouses;
					excellproducts.Cells[row, cell++].Value = product.WarehouseId;
					excellproducts.Cells[row, cell++].Value = product.StockQuantity;
					excellproducts.Cells[row, cell++].Value = product.DisplayStockAvailability;
					excellproducts.Cells[row, cell++].Value = product.DisplayStockQuantity;
					excellproducts.Cells[row, cell++].Value = product.MinStockQuantity;
					excellproducts.Cells[row, cell++].Value = product.LowStockActivity;
					excellproducts.Cells[row, cell++].Value = product.NotifyAdminForQuantityBelow;
					excellproducts.Cells[row, cell++].Value = product.BackorderMode;
					excellproducts.Cells[row, cell++].Value = product.AllowBackInStockSubscriptions;
					excellproducts.Cells[row, cell++].Value = product.OrderMinimumQuantity;
					excellproducts.Cells[row, cell++].Value = product.OrderMaximumQuantity;
					excellproducts.Cells[row, cell++].Value = product.AllowedQuantities;
					excellproducts.Cells[row, cell++].Value = product.AllowAddingOnlyExistingAttributeCombinations;
					excellproducts.Cells[row, cell++].Value = product.NotReturnable;
					excellproducts.Cells[row, cell++].Value = product.DisableBuyButton;
					excellproducts.Cells[row, cell++].Value = product.DisableWishlistButton;
					excellproducts.Cells[row, cell++].Value = product.AvailableForPreOrder;
					excellproducts.Cells[row, cell++].Value = product.PreOrderAvailabilityStartDateTimeUtc;
					excellproducts.Cells[row, cell++].Value = product.CallForPrice;
					excellproducts.Cells[row, cell++].Value = product.Price;
					excellproducts.Cells[row, cell++].Value = product.OldPrice;
					excellproducts.Cells[row, cell++].Value = product.ProductCost;
					excellproducts.Cells[row, cell++].Value = product.CustomerEntersPrice;
					excellproducts.Cells[row, cell++].Value = product.MinimumCustomerEnteredPrice;
					excellproducts.Cells[row, cell++].Value = product.MaximumCustomerEnteredPrice;
					excellproducts.Cells[row, cell++].Value = product.BasepriceEnabled;
					excellproducts.Cells[row, cell++].Value = product.BasepriceAmount;
					excellproducts.Cells[row, cell++].Value = product.BasepriceUnitId;
					excellproducts.Cells[row, cell++].Value = product.BasepriceBaseAmount;
					excellproducts.Cells[row, cell++].Value = product.BasepriceBaseUnitId;
					excellproducts.Cells[row, cell++].Value = product.MarkAsNew;
					excellproducts.Cells[row, cell++].Value = product.MarkAsNewStartDateTimeUtc;
					excellproducts.Cells[row, cell++].Value = product.MarkAsNewEndDateTimeUtc;
					excellproducts.Cells[row, cell++].Value = product.Weight;
					excellproducts.Cells[row, cell++].Value = product.Length;
					excellproducts.Cells[row, cell++].Value = product.Width;
					excellproducts.Cells[row, cell++].Value = product.Height;
					excellproducts.Cells[row, cell++].Value = category != null ? category.Name : "Аксессуары";
					excellproducts.Cells[row, cell++].Value = manufactor;
					excellproducts.Cells[row, cell++].Value = productTags;
					excellproducts.Cells[row, cell++].Value = hasPicture ? string.Format("{0}/{1}", picturesFolder, filename) : "" ;
					//excellproducts.Cells[row, cell++].Value = product.Name;
					//excellproducts.Cells[row, cell++].Value = product.Name;




					row++;
				}

				#endregion

				var pathto = System.Web.HttpContext.Current.Server.MapPath("~/Plugins/Import.ItLink/productsNew.xlsx");

				Stream stream = File.Create(pathto);
				xlPackage.SaveAs(stream);
				stream.Close();

				Stream result = File.Open(pathto, FileMode.Open);

				return result;
			}


			throw new NotImplementedException();
		}

	}
}
