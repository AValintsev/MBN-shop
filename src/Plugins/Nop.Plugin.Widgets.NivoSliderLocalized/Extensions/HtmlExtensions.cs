using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Extensions
{
	public static class HtmlExtensions
	{
		public static HelperResult LocalizedEditor<T, TLocalizedModelLocal>(this HtmlHelper<List<T>> helper,
			string name,
			Func<int, HelperResult> localizedTemplate,
			Func<T, HelperResult> standardTemplate,
			bool ignoreIfSeveralStores = false)
			where T : ILocalizedModel<TLocalizedModelLocal>
			where TLocalizedModelLocal : ILocalizedModelLocal
		{
			return new HelperResult(writer =>
			{
				var localizationSupported = helper.ViewData.Model.First().Locales.Count > 1;
				if (ignoreIfSeveralStores)
				{
					var storeService = EngineContext.Current.Resolve<IStoreService>();
					if (storeService.GetAllStores().Count >= 2)
					{
						localizationSupported = false;
					}
				}
				if (localizationSupported)
				{
					var tabStrip = new StringBuilder();
					tabStrip.AppendLine(string.Format("<div id=\"{0}\" class=\"nav-tabs-custom nav-tabs-localized-fields\">", name));
					tabStrip.AppendLine("<ul class=\"nav nav-tabs\">");

					//default tab
					tabStrip.AppendLine("<li class=\"active\">");
					tabStrip.AppendLine(string.Format("<a data-tab-name=\"{0}-{1}-tab\" href=\"#{0}-{1}-tab\" data-toggle=\"tab\">{2}</a>",
							name,
							"standard",
							EngineContext.Current.Resolve<ILocalizationService>().GetResource("Admin.Common.Standard")));
					tabStrip.AppendLine("</li>");

					var languageService = EngineContext.Current.Resolve<ILanguageService>();
					foreach (var locale in helper.ViewData.Model.First().Locales)
					{
						//languages
						var language = languageService.GetLanguageById(locale.LanguageId);
						if (language == null)
							throw new Exception("Language cannot be loaded");

						tabStrip.AppendLine("<li>");
						var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
						var iconUrl = urlHelper.Content("~/Content/images/flags/" + language.FlagImageFileName);
						tabStrip.AppendLine(string.Format("<a data-tab-name=\"{0}-{1}-tab\" href=\"#{0}-{1}-tab\" data-toggle=\"tab\"><img alt='' src='{2}'>{3}</a>",
								name,
								language.Id,
								iconUrl,
								HttpUtility.HtmlEncode(language.Name)));

						tabStrip.AppendLine("</li>");
					}
					tabStrip.AppendLine("</ul>");

					//default tab
					tabStrip.AppendLine("<div class=\"tab-content\">");
					tabStrip.AppendLine(string.Format("<div class=\"tab-pane active\" id=\"{0}-{1}-tab\">", name, "standard"));
					tabStrip.AppendLine(standardTemplate(helper.ViewData.Model.First()).ToHtmlString());
					tabStrip.AppendLine("</div>");

					for (int i = 0; i < helper.ViewData.Model.First().Locales.Count; i++)
					{
						//languages
						var language = languageService.GetLanguageById(helper.ViewData.Model.First().Locales[i].LanguageId);

						tabStrip.AppendLine(string.Format("<div class=\"tab-pane\" id=\"{0}-{1}-tab\">",
							name,
							language.Id));
						tabStrip.AppendLine(localizedTemplate(i).ToHtmlString());
						tabStrip.AppendLine("</div>");
					}
					tabStrip.AppendLine("</div>");
					tabStrip.AppendLine("</div>");
					writer.Write(new MvcHtmlString(tabStrip.ToString()));
				}
				else
				{
					standardTemplate(helper.ViewData.Model.First()).WriteTo(writer);
				}
			});
		}
	}
}
