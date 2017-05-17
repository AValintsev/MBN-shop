using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.QuickOrder.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.Misc.QuickOrder.Enabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.EmailAddressRequired")]
        public bool EmailAddressRequired { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.EmailAddressEnabled")]
        public bool EmailAddressEnabled { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.NameRequired")]
        public bool NameRequired { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.NameEnabled")]
        public bool NameEnabled { get; set; }

        [NopResourceDisplayName("Admin.Misc.QuickOrder.WidgetZone")]
        public string WidgetZone { get; set; }

        public IList<SelectListItem> AvailableWidgets { get; set; }

        public ConfigurationModel()
        {
            AvailableWidgets = new List<SelectListItem>();
        }
    }
}
