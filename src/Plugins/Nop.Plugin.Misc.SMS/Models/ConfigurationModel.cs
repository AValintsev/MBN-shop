using FluentValidation.Attributes;
using Nop.Web.Framework;
using System;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.SMS.Models
{
    [Validator(typeof(Validator))]
    public class ConfigurationModel
	{
        [NopResourceDisplayName("Plugins.Misc.SMS.SettingsLogin")]
		public string Login { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.SettingsPassword")]
		public string Password { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.SettingsApiUrl")]
		public string ApiUrl { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Plugins.Misc.SMS.SettingsXML")]
		public string XML { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.SettingsAlfaName")]
		public string AlfaName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.SettingsAdminPhoneNumber")]
		public string AdminPhoneNumber { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.SettingsLastmodified")]
		public DateTime LastConfigurationDate { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.SettingsEnableAlfaName")]
		public bool EnableAlfaName { get; set; }
		
		[NopResourceDisplayName("Plugins.Misc.SMS.SettingsEnabled")]
		public bool Enabled { get; set; }
	}
}
