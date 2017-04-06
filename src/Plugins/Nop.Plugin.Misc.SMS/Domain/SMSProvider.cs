using Nop.Core;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SMS.Domain
{
    public class SMSProvider : BaseEntity
    {
        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderEnabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderLogin")]
        public string Login { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderPassword")]
        public string Password { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderApi")]
        public string Api { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderAlfaName")]
        public string AlfaName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderEnableAlfaName")]
        public bool EnableAlfaName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderAdminPhoneNumber")]
        public string AdminPhoneNumber { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.ProviderLastmodified")]
        public DateTime LastConfigurationDate { get; set; }
    }
}
