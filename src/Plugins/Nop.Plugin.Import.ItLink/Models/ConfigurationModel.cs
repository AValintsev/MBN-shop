using Nop.Web.Framework;

namespace Nop.Plugin.Import.ItLink.Models
{
	public class ConfigurationModel
	{
		[NopResourceDisplayName("Plugins.Import.ItLink.XmlUrl")]
		public string XmlUrl { get; set; }

		[NopResourceDisplayName("Plugins.Import.ItLink.Username")]
		public string Username { get; set; }

		[NopResourceDisplayName("Plugins.Import.ItLink.Password")]
		public string Password { get; set; }

		[NopResourceDisplayName("Plugins.Import.ItLink.Percent")]
		public double Percent { get; set; }
	}
}
