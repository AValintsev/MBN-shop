using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Domain
{
	public class SliderItem : BaseEntity, ILocalizedEntity
	{
		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Picture")]
		public string PictureId { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Text")]
		public string Text { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Link")]
		public string Link { get; set; }
	}
}
