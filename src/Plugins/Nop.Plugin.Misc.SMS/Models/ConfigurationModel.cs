using Nop.Web.Framework;
using System;

namespace Nop.Plugin.Misc.SMS.Models
{
	public class ConfigurationModel
	{
		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderLogin")]
		public string Login { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderPassword")]
		public string Password { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderApi")]
		public string ApiUrl { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderXML")]
		public string XML { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderAlfaName")]
		public string AlfaName { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderAdminPhoneNumber")]
		public string AdminPhoneNumber { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderLastmodified")]
		public DateTime LastConfigurationDate { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderEnableAlfaName")]
		public bool EnableAlfaName { get; set; }
		
		[NopResourceDisplayName("Plugins.Misc.SMS.ProviderEnabled")]
		public bool Enabled { get; set; }
	}
}
