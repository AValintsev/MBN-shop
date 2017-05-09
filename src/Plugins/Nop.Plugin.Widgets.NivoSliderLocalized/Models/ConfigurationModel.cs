using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Models
{
	public class ConfigurationModel : ILocalizedModel<ConfigurationModel.ConfigurationLocalizedModel>
	{
		public ConfigurationModel()
		{
			Locales = new List<ConfigurationLocalizedModel>();
		}

		public IList<ConfigurationLocalizedModel> Locales { get; set; }

		public class ConfigurationLocalizedModel : ILocalizedModelLocal
		{
			public int LanguageId { get; set; }

			[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Picture")]
			[UIHint("Picture")]
			public int PictureId { get; set; }

			[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Text")]
			[AllowHtml]
			public string Text { get; set; }

			[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Link")]
			[AllowHtml]
			public string Link { get; set; }
		}

		public int ActiveStoreScopeConfiguration { get; set; }

		public int Id { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Picture")]
		[UIHint("Picture")]
		public int PictureId { get; set; }
		public bool PictureId_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Text")]
		[AllowHtml]
		public string Text { get; set; }
		public bool Text_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.NivoSliderLocalized.Link")]
		[AllowHtml]
		public string Link { get; set; }
		public bool Link_OverrideForStore { get; set; }
	}
}
