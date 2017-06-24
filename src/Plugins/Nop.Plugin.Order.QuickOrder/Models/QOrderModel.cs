using FluentValidation.Attributes;
using Nop.Core;
using Nop.Web.Framework;

namespace Nop.Plugin.Order.QuickOrder.Models
{
    [Validator(typeof(Validator))]
    public class QOrderModel : BaseEntity
    {
        [NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerName")]
        public string CustomerName { get; set; }

        [NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerEmail")]
        public string CustomerEmail { get; set; }

        [NopResourceDisplayName("Plugins.Order.QuickOrder.CustomerPhone")]
        public string CustomerPhone { get; set; }

        public bool NameEnable { get; set; }
        public bool PhoneEnable { get; set; }
        public bool EmailEnable { get; set; }
    }
}
