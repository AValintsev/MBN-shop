using Nop.Core.Configuration;

namespace Nop.Plugin.Order.QuickOrder
{
    public class QOrderSettings : ISettings
    {

            public bool Enabled { get; set; }

            public bool EmailAddressRequired { get; set; }

            public bool EmailAddressEnabled { get; set; }

            public bool PhoneRequired { get; set; }

            public bool PhoneEnabled { get; set; }

            public bool NameRequired { get; set; }

            public bool NameEnabled { get; set; }

            public string WidgetZone { get; set; }
        }
    }

