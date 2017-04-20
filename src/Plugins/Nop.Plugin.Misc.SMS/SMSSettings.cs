using Nop.Core.Configuration;
using System;

namespace Nop.Plugin.Misc.SMS
{
	public class SMSSettings : ISettings
	{
		public string Login { get; set; }

		public string Password { get; set; }

		public string ApiUrl { get; set; }

		public string XML { get; set; }

		public string AlfaName { get; set; }

		public string AdminPhoneNumber { get; set; }

		public DateTime LastConfigurationDate { get; set; }

		public bool EnableAlfaName { get; set; }

		public bool EnableXML { get; set; }

		public bool IsHttpBasic { get; set; }

		public bool Enabled { get; set; }
	}
}
