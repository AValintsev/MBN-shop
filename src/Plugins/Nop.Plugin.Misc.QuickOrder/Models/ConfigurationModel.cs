using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.Order.QuickOrder.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.Order.QuickOrder.Enabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.EmailAddressRequired")]
        public bool EmailAddressRequired { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.EmailAddressEnabled")]
        public bool EmailAddressEnabled { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.NameRequired")]
        public bool NameRequired { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.NameEnabled")]
        public bool NameEnabled { get; set; }

        [NopResourceDisplayName("Admin.Order.QuickOrder.WidgetZone")]
        public string WidgetZone { get; set; }

        public IList<SelectListItem> AvailableWidgets { get; set; }

        public ConfigurationModel()
        {
            AvailableWidgets = new List<SelectListItem>();
        }
    }
}
