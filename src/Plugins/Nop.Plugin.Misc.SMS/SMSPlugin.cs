using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Plugin.Misc.SMS.Data;
using Nop.Plugin.Misc.SMS.Domain;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Events;
using Nop.Services.Localization;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Routing;
using System.Xml;

namespace Nop.Plugin.Misc.SMS
{
	public class SMSPlugin : BasePlugin, IMiscPlugin, IConsumer<OrderPlacedEvent>
	{
		/// Key pattern to clear cache
		/// </summary>
		private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Nop.lsr.";

		#region fields

		private readonly SMSObjectContext _context;
		private readonly SMSSettings _smsSettings;
		private readonly IRepository<Domain.SMS> _smsRepository;
		private readonly IRepository<SMSMessage> _smsMessageRepository;
		private readonly ISettingService _settingService;
		private readonly IRepository<Language> _languageRepository;
		//private readonly LanguageService _languageService;
		private readonly ICacheManager _cacheManager;

		#endregion

		public SMSPlugin(
			IRepository<Language> languageRepository,
			//LanguageService languageService,
			ICacheManager cacheManager,
			SMSSettings smsSettings,
			SMSObjectContext context,
			ISettingService settingsService,
			IRepository<Domain.SMS> smsRepository,
			IRepository<SMSMessage> smsMessageRepostitory
		)
		{
			_languageRepository = languageRepository;
			//_languageService = languageService;
			_cacheManager = cacheManager;
			_context = context;
			_smsSettings = smsSettings;
			_smsRepository = smsRepository;
			_settingService = settingsService;
			_smsMessageRepository = smsMessageRepostitory;
		}

		public bool Authenticate()
		{
			return true;
		}

		public override void Install()
		{
			_context.Install();

			InstallLocaleResources();

			var settings = new SMSSettings
			{
				LastConfigurationDate = DateTime.UtcNow,
				Enabled = true,
				EnableAlfaName = true,
			};
			_settingService.SaveSetting(settings);


			#region Add default messages

			var message1 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent",
				IsforAdmin = false,
				MessageText = "Your order #ORDERNUMBER# is placed. Total: #ORDERTOTAL#",
				Name = "Plugins.Misc.SMS.OrderPlacedEvent"
			};
			var message1Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent",
				IsforAdmin = true,
				MessageText = "Client has ordered #ORDERNUMBER#. Total: #ORDERTOTAL#",
				Name = "Plugins.Misc.SMS.OrderPlacedEvent.ForAdmin"
			};

			var message2 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPaidEvent",
				IsforAdmin = false,
				MessageText = "Order #ORDERNUMBER# has been successfully paid. Total: #ORDERTOTAL#",
				Name = "Plugins.Misc.SMS.OrderPaidEvent"
			};
			var message2Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPaidEvent",
				IsforAdmin = true,
				MessageText = "Client has paid #ORDERNUMBER#. Total: #ORDERTOTAL#",
				Name = "Plugins.Misc.SMS.OrderPaidEvent.ForAdmin"
			};

			var message3 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderCancelledEvent",
				IsforAdmin = false,
				MessageText = "Order #ORDERNUMBER# has been canceled",
				Name = "Plugins.Misc.SMS.OrderCancelledEvent"
			};
			var message3Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderCancelledEvent",
				IsforAdmin = true,
				MessageText = "Client has canceled #ORDERNUMBER#.",
				Name = "Plugins.Misc.SMS.OrderCancelledEvent.ForAdmin"
			};

			_context.Set<SMSMessage>().Add(message1);
			_context.Set<SMSMessage>().Add(message1Admin);
			_context.Set<SMSMessage>().Add(message2);
			_context.Set<SMSMessage>().Add(message2Admin);
			_context.Set<SMSMessage>().Add(message3);
			_context.Set<SMSMessage>().Add(message3Admin);

			_context.SaveChanges();

			#endregion

			base.Install();
		}

		protected virtual void InstallLocaleResources()
		{
			//English language
			var languageEng = _languageRepository.Table.SingleOrDefault(l => l.Name == "English");

			if (languageEng != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.SMS/App_Data/Translations"), "en_misc.sms.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageEng, localesXml);
				}
			}

			//Russian language
			var languageRu = _languageRepository.Table.SingleOrDefault(l => l.Name == "Russian");

			if (languageRu != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.SMS/App_Data/Translations"), "ru_misc.sms.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageRu, localesXml);
				}
			}
			
			//Ukrainian language
			var languageUa = _languageRepository.Table.Single(l => l.Name == "Ukrainian");
			if (languageUa != null)
			{
				//save resources
				foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.SMS/App_Data/Translations"), "ua_misc.sms.xml", SearchOption.TopDirectoryOnly))
				{
					var localesXml = File.ReadAllText(filePath);
					var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
					localizationService.ImportResourcesFromXml(languageUa, localesXml);
				}
			}
		}

		public override void Uninstall()
		{
			_settingService.DeleteSetting<SMSSettings>();

			_context.Uninstall();

			base.Uninstall();
		}

		#region IConsumer<OrderPlacedEvent>

		public void HandleEvent(OrderPlacedEvent eventMessage)
		{
			var eventType = eventMessage.GetType().ToString();
			HandleOrderEvent(eventMessage.Order, eventType);
		}

		public void HandleEvent(OrderCancelledEvent eventMessage)
		{
			var eventType = eventMessage.GetType().ToString();
			HandleOrderEvent(eventMessage.Order, eventType);
		}

		public void HandleEvent(OrderPaidEvent eventMessage)
		{
			var eventType = eventMessage.GetType().ToString();
			HandleOrderEvent(eventMessage.Order, eventType);
		}

		#endregion

		private void HandleOrderEvent(Order order, string eventType)
		{
			if (!_smsSettings.Enabled)
			{
				return;
			}

			#region Send message to client

			var sms = new Domain.SMS
			{
				Login = _smsSettings.Login,
				Password = _smsSettings.Password,
				ApiUrl = _smsSettings.ApiUrl,
				XML = _smsSettings.XML,
				AlfaName = _smsSettings.EnableAlfaName ? _smsSettings.AlfaName : null,
				Date = DateTime.UtcNow,
				EventType = eventType
			};

			var clientMessage = _smsMessageRepository.Table
				.FirstOrDefault(m => !m.IsforAdmin && m.Enabled && m.EventType == eventType);

			if (clientMessage != null)
			{
				var message = clientMessage.GetLocalized(x => x.MessageText);

				sms.PhoneNumber = order.Customer.ShippingAddress.PhoneNumber ??
						order.Customer.BillingAddress.PhoneNumber;
				sms.Message = message
					.Replace("#ORDERNUMBER#", order.Id.ToString())
					.Replace("#ORDERTOTAL#", order.OrderTotal.ToString());

				sms.SmsServerResponse = POSTRequest(sms);
				_smsRepository.Insert(sms);
			}

			#endregion

			#region Send message to admin

			var smsForAdmin = new Domain.SMS
			{
				Login = _smsSettings.Login,
				Password = _smsSettings.Password,
				ApiUrl = _smsSettings.ApiUrl,
				XML = _smsSettings.XML,
				AlfaName = _smsSettings.EnableAlfaName ? _smsSettings.AlfaName : null,
				Date = DateTime.UtcNow,
				EventType = eventType
			};

			var adminMessage = _smsMessageRepository.Table
				.FirstOrDefault(m => m.IsforAdmin && m.Enabled && m.EventType == eventType);

			if (adminMessage != null)
			{
				var message = adminMessage.GetLocalized(x => x.MessageText);
				smsForAdmin.PhoneNumber = _smsSettings.AdminPhoneNumber;
				smsForAdmin.Message = message
					.Replace("#ORDERNUMBER#", order.Id.ToString())
					.Replace("#ORDERTOTAL#", order.OrderTotal.ToString());
				smsForAdmin.SmsServerResponse = POSTRequest(smsForAdmin);
				_smsRepository.Insert(smsForAdmin);
			}

			#endregion
		}

		private string POSTRequest(Domain.SMS sms)
		{
			string apiUrl = sms.ApiUrl;

			if (sms.XML != null)
			{
				//TODO: check are all fields replaced
				sms.XML = sms.XML.Replace("#PHONENUMBER#", sms.PhoneNumber)
						 .Replace("#ALFANAME#", sms.AlfaName)
						 .Replace("#MESSAGE#", sms.Message);
			}

			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiUrl);

				string authInfo = sms.Login + ":" + sms.Password;
				authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
				webRequest.Headers["Authorization"] = "Basic " + authInfo;

				byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(sms.XML);
				webRequest.Method = "POST";
				webRequest.ContentType = "text/xml;charset=utf-8";
				webRequest.ContentLength = requestBytes.Length;
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(requestBytes, 0, requestBytes.Length);
				requestStream.Close();

				HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
				StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
				string responce = streamReader.ReadToEnd();

				streamReader.Close();
				webResponse.Close();

				return responce;
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		}

		#region Routes

		/// <summary>
		/// Gets a route for provider configuration
		/// </summary>
		/// <param name="actionName">Action name</param>
		/// <param name="controllerName">Controller name</param>
		/// <param name="routeValues">Route values</param>
		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "MiscSMS";
			routeValues = new RouteValueDictionary
			{
				{ "Namespaces", "Nop.Plugin.Misc.SMS.Controllers" },
				{ "area", null }
			};
		}

		#endregion
	}
}

