using Microsoft.Office.Interop.Excel;
using Nop.Admin.Models.Catalog;
using Nop.Core.Domain.Catalog;
using System;
using System.IO;
using System.Xml;
using Exel =  Microsoft.Office.Interop.Excel;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class XmlToXlsConverter : IXmlToXlsConverter
	{
		/// <summary>
		/// Converts input xml to excel xls document and returns a stream.
		/// </summary>
		/// <param name="xmlDocument">Xml document to convert.</param>
		/// <returns>Excel (xls) stream.</returns>
		public Stream ConvertFromXml(XmlDocument xmlDocument)
		{
			const int NUMBER_OF_COLS = 94;
			#region Product
			Product product = new Product();
			product.ProductType = ProductType.SimpleProduct;
			product.ParentGroupedProductId = 0;
			product.VisibleIndividually = true;
			product.Name = "Name";
			product.ShortDescription = "ShortDescription";
			product.FullDescription = "FullDescription";
			// Vendor - null
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
			product.MarkAsNew = false;
			product.MarkAsNewStartDateTimeUtc = null;
			product.MarkAsNewEndDateTimeUtc = null;
			product.Weight = 0;
			product.Length = 0;
			product.Width = 0;
			product.Height = 0;
			//product.ProductCategories.Add();
			//product.ProductManufacturers = null;
			//product.ProductTags
			//product.ProductPictures.Add(new ProductPicture { i})

			#endregion


			Exel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
			excelapp.Visible = true;

			excelapp.Workbooks.Open("~/products.xslx");

			Exel.Workbook excelappworkbook = excelapp.Workbooks[1];

			Exel.Sheets excelsheets = excelappworkbook.Worksheets;

			Worksheet excelworksheet = (Exel.Worksheet)excelsheets.get_Item(1);

			Exel.Range excelcell = excelworksheet.get_Range("A2", "CP2");

			int cell = 1;


			excelcell[1, cell++].Value2 = product.ProductType;
			excelcell[1, cell++].Value2 = product.ParentGroupedProductId;
			excelcell[1, cell++].Value2 = product.VisibleIndividually;
			excelcell[1, cell++].Value2 = product.Name;
			excelcell[1, cell++].Value2 = product.ShortDescription;
			excelcell[1, cell++].Value2 = product.FullDescription;
			excelcell[1, cell++].Value2 = product.VendorId;
			excelcell[1, cell++].Value2 = product.ProductTemplateId;
			excelcell[1, cell++].Value2 = product.ShowOnHomePage;
			excelcell[1, cell++].Value2 = product.MetaKeywords;
			excelcell[1, cell++].Value2 = product.MetaDescription;
			excelcell[1, cell++].Value2 = product.MetaTitle;
			excelcell[1, cell++].Value2 = product.Name;
			excelcell[1, cell++].Value2 = product.AllowCustomerReviews;
			excelcell[1, cell++].Value2 = product.Published;
			excelcell[1, cell++].Value2 = product.Sku;
			excelcell[1, cell++].Value2 = product.ManufacturerPartNumber;
			excelcell[1, cell++].Value2 = product.Gtin;
			excelcell[1, cell++].Value2 = product.IsGiftCard;
			excelcell[1, cell++].Value2 = product.GiftCardType;
			excelcell[1, cell++].Value2 = product.OverriddenGiftCardAmount;
			excelcell[1, cell++].Value2 = product.RequireOtherProducts;
			excelcell[1, cell++].Value2 = product.RequiredProductIds;
			excelcell[1, cell++].Value2 = product.AutomaticallyAddRequiredProducts;
			excelcell[1, cell++].Value2 = product.IsDownload;
			excelcell[1, cell++].Value2 = product.DownloadId;
			excelcell[1, cell++].Value2 = product.UnlimitedDownloads;
			excelcell[1, cell++].Value2 = product.MaxNumberOfDownloads;
			excelcell[1, cell++].Value2 = product.DownloadActivationType;
			excelcell[1, cell++].Value2 = product.HasSampleDownload;
			excelcell[1, cell++].Value2 = product.SampleDownloadId;
			excelcell[1, cell++].Value2 = product.HasUserAgreement;
			excelcell[1, cell++].Value2 = product.UserAgreementText;
			excelcell[1, cell++].Value2 = product.IsRecurring;
			excelcell[1, cell++].Value2 = product.RecurringCycleLength;
			excelcell[1, cell++].Value2 = product.RecurringCyclePeriod;
			excelcell[1, cell++].Value2 = product.RecurringTotalCycles;
			excelcell[1, cell++].Value2 = product.IsRental;
			excelcell[1, cell++].Value2 = product.RentalPriceLength;
			excelcell[1, cell++].Value2 = product.RentalPricePeriod;
			excelcell[1, cell++].Value2 = product.IsShipEnabled;
			excelcell[1, cell++].Value2 = product.IsFreeShipping;
			excelcell[1, cell++].Value2 = product.ShipSeparately;
			excelcell[1, cell++].Value2 = product.AdditionalShippingCharge;
			excelcell[1, cell++].Value2 = product.DeliveryDateId;
			excelcell[1, cell++].Value2 = product.IsTaxExempt;
			excelcell[1, cell++].Value2 = product.TaxCategoryId;
			excelcell[1, cell++].Value2 = product.IsTelecommunicationsOrBroadcastingOrElectronicServices;
			excelcell[1, cell++].Value2 = product.ManageInventoryMethod;
			excelcell[1, cell++].Value2 = product.ProductAvailabilityRangeId;
			excelcell[1, cell++].Value2 = product.UseMultipleWarehouses;
			excelcell[1, cell++].Value2 = product.WarehouseId;
			excelcell[1, cell++].Value2 = product.StockQuantity;
			excelcell[1, cell++].Value2 = product.DisplayStockAvailability;
			excelcell[1, cell++].Value2 = product.DisplayStockQuantity;
			excelcell[1, cell++].Value2 = product.MinStockQuantity;
			excelcell[1, cell++].Value2 = product.LowStockActivity;
			excelcell[1, cell++].Value2 = product.NotifyAdminForQuantityBelow;
			excelcell[1, cell++].Value2 = product.BackorderMode;
			excelcell[1, cell++].Value2 = product.AllowBackInStockSubscriptions;
			excelcell[1, cell++].Value2 = product.OrderMinimumQuantity;
			excelcell[1, cell++].Value2 = product.OrderMaximumQuantity;
			excelcell[1, cell++].Value2 = product.AllowedQuantities;
			excelcell[1, cell++].Value2 = product.AllowAddingOnlyExistingAttributeCombinations;
			excelcell[1, cell++].Value2 = product.NotReturnable;
			excelcell[1, cell++].Value2 = product.DisableBuyButton;
			excelcell[1, cell++].Value2 = product.DisableWishlistButton;
			excelcell[1, cell++].Value2 = product.AvailableForPreOrder;
			excelcell[1, cell++].Value2 = product.PreOrderAvailabilityStartDateTimeUtc;
			excelcell[1, cell++].Value2 = product.CallForPrice;
			excelcell[1, cell++].Value2 = product.Price;
			excelcell[1, cell++].Value2 = product.OldPrice;
			excelcell[1, cell++].Value2 = product.ProductCost;
			excelcell[1, cell++].Value2 = product.CustomerEntersPrice;
			excelcell[1, cell++].Value2 = product.MinimumCustomerEnteredPrice;
			excelcell[1, cell++].Value2 = product.MaximumCustomerEnteredPrice;
			excelcell[1, cell++].Value2 = product.BasepriceEnabled;
			excelcell[1, cell++].Value2 = product.BasepriceAmount;
			excelcell[1, cell++].Value2 = product.BasepriceUnitId;
			excelcell[1, cell++].Value2 = product.BasepriceBaseAmount;
			excelcell[1, cell++].Value2 = product.BasepriceBaseUnitId;
			excelcell[1, cell++].Value2 = product.MarkAsNew;
			excelcell[1, cell++].Value2 = product.MarkAsNewStartDateTimeUtc;
			excelcell[1, cell++].Value2 = product.MarkAsNewEndDateTimeUtc;
			excelcell[1, cell++].Value2 = product.Weight;
			excelcell[1, cell++].Value2 = product.Length;
			excelcell[1, cell++].Value2 = product.Width;
			excelcell[1, cell++].Value2 = product.Height;
			foreach (var category in product.ProductCategories) {
				excelcell[1, cell].Value2 = excelcell[1, cell].Value2 + category.Category.Name + ";";
			}
			cell++;
			foreach (var manufacturer in product.ProductManufacturers)
			{
				excelcell[1, cell].Value2 = excelcell[1, cell].Value2 +  manufacturer.Manufacturer.Name + ";";
			}
			cell++;
			foreach (var tag in product.ProductTags)
			{
				excelcell[1, cell].Value2 = excelcell[1, cell].Value2 + tag.Name + ";";
			}
			cell++;
			//excelcell[1, cell++].Value2 = product.Name;
			//excelcell[1, cell++].Value2 = product.Name;
			//excelcell[1, cell++].Value2 = product.Name;


			excelappworkbook.Save();
			excelapp.Quit();

			throw new NotImplementedException();
		}
	}
}
