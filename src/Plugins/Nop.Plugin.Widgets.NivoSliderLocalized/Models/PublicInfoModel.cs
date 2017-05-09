using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Models
{
	public class PublicInfoModel : BaseNopModel
	{
		public string PictureUrl { get; set; }

		public string Text { get; set; }

		public string Link { get; set; }
	}
}
