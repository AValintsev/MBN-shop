using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;

namespace Nop.Plugin.ExchangeRate.PrivatExchange
{
	public class PrivatExchangeRateProvider : BasePlugin, IExchangeRateProvider
	{
		#region Fields

		private readonly ILocalizationService _localizationService;
		private readonly ILogger _logger;

		#endregion

		#region Ctor

		public PrivatExchangeRateProvider(ILocalizationService localizationService,
			ILogger logger)
		{
			this._localizationService = localizationService;
			this._logger = logger;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets currency live rates
		/// </summary>
		/// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
		/// <returns>Exchange rates</returns>
		public IList<Core.Domain.Directory.ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
		{
			if (exchangeRateCurrencyCode == null)
				throw new ArgumentNullException("exchangeRateCurrencyCode");

			//add USD with rate 1
			var ratesToUsd = new List<Core.Domain.Directory.ExchangeRate> {
				new Core.Domain.Directory.ExchangeRate
				{
					CurrencyCode = exchangeRateCurrencyCode,
					Rate = 1,
					UpdatedOn = DateTime.UtcNow
				}
			};

			//get exchange rates to euro from Privat Bank
			var request = (HttpWebRequest)WebRequest.Create("https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=11");

			try
			{
				using (var response = request.GetResponse())
				{
					//load XML document
					var document = new XmlDocument();
					document.Load(response.GetResponseStream());

					var allRates = document.SelectNodes("//exchangerates/row/exchangerate");

					foreach (XmlNode currency in allRates)
					{
						//get rate only for UAH
						var currencyCode = currency.Attributes["ccy"].Value;
						if (currencyCode.Equals("usd", StringComparison.InvariantCultureIgnoreCase))
						{
							decimal currencyRate;
							if (!decimal.TryParse(currency.Attributes["sale"].Value, out currencyRate))
								continue;

							ratesToUsd.Add(new Core.Domain.Directory.ExchangeRate()
							{
								CurrencyCode = currency.Attributes["base_ccy"].Value,
								Rate = currencyRate,
								UpdatedOn = DateTime.UtcNow
							});
						}
					}
				}
			}
			catch (WebException ex)
			{
				_logger.Error("Privat exchange rate provider", ex);
			}
			
			//use only currencies that are supported by ECB
			var exchangeRateCurrency = ratesToUsd.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
			if (exchangeRateCurrency == null)
				throw new NopException(_localizationService.GetResource("Plugins.ExchangeRate.PrivatExchange.Error"));

			//return result for the selected (not euro) currency
			return ratesToUsd.Select(rate => new Core.Domain.Directory.ExchangeRate
			{
				CurrencyCode = rate.CurrencyCode,
				Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
				UpdatedOn = rate.UpdatedOn
			}).ToList();
		}

		/// <summary>
		/// Install the plugin
		/// </summary>
		public override void Install()
		{
			//locales
			this.AddOrUpdatePluginLocaleResource("Plugins.ExchangeRate.PrivatExchange.Error", "You can use PrivatBannk exchange rate provider only when the primary exchange rate currency is supported by PrivatBank");

			base.Install();
		}

		/// <summary>
		/// Uninstall the plugin
		/// </summary>
		public override void Uninstall()
		{
			//locales
			this.DeletePluginLocaleResource("Plugins.ExchangeRate.PrivatExchange.Error");

			base.Uninstall();
		}

		#endregion

	}
}
