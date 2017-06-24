using Nop.Core.Configuration;

namespace Nop.Plugin.Import.ItLink
{
	public class ImportItLinkSettings : ISettings
	{
		public string XmlUrl { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}


