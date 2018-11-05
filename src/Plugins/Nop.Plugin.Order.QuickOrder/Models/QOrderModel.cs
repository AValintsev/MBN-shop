using FluentValidation.Attributes;
using Nop.Core;
using Nop.Web.Framework;
using System.Collections.Generic;
using static Nop.Web.Models.Checkout.CheckoutShippingMethodModel;

namespace Nop.Plugin.Order.QuickOrder.Models
{
	[Validator(typeof(Validator))]
	public class QOrderModel : BaseEntity
	{
		public QOrderModel()
		{
			ShippingMethodModel = new List<ShippingMethodModel>();
		}

		[NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerName")]
		public string CustomerName { get; set; }

		[NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerEmail")]
		public string CustomerEmail { get; set; }

		[NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerPhone")]
		public string CustomerPhone { get; set; }

		[NopResourceDisplayName("Plugins.Order.QuickOrder.City")]
		public string City { get; set; }

		[NopResourceDisplayName("Plugins.Order.QuickOrder.PostNumber")]
		public string PostNumber { get; set; }

		public string SelectedShippingMethod { get; set; }

		public List<ShippingMethodModel> ShippingMethodModel { get; set; }

		public bool NameEnable { get; set; }
		public bool PhoneEnable { get; set; }
		public bool EmailEnable { get; set; }
	}
}
