using Nop.Core.Data;
using Nop.Core.Domain.Orders;
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

namespace Nop.Plugin.Misc.SMS
{
	public class SMSPlugin : BasePlugin, IMiscPlugin, IConsumer<OrderPlacedEvent>
	{
		#region fields

		private readonly SMSObjectContext _context;
		private readonly SMSSettings _smsSettings;
		private readonly IRepository<Domain.SMS> _smsRepository;
		private readonly IRepository<SMSMessage> _smsMessageRepository;
		private readonly ISettingService _settingService;

		#endregion

		public SMSPlugin(
			SMSSettings smsSettings,
			SMSObjectContext context,
			ISettingService settingsService,
			IRepository<Domain.SMS> smsRepository,
			IRepository<SMSMessage> smsMessageRepostitory
		)
		{
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

			var settings = new SMSSettings
			{
				LastConfigurationDate = DateTime.UtcNow,
				Enabled = true,
				EnableAlfaName = true,
			};
			_settingService.SaveSetting(settings);

			#region Localization Resources Add

			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ConfigurationPage", "SMS Sender Configuration");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderEnabled", "Enable sending sms");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderLogin", "Login");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderLoginRequired", "Login is Required");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderPassword", "Password");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderPasswordRequired", "Password is Required");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderApi", "Api");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderApi.Hint", "For <strong>HTTP API</strong> replace Login with #LOGIN#, Password  #PASSWORD#, recipient phone number #MOBILEPHONE# and message text with #MESSAGE# .");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderApiRequired", "Api is Required");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderAlfaName", "AlfaName");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderAlfaName.Hint", "Alfa Name will be displayed as sms sender for client.");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderEnableAlfaName", "Enable");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderAdminPhoneNumber", "Admin PhoneNumber");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.ProviderLastmodified", "Last modified");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageName", "Message");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageName.Hint", "Please use #ORDERNUMBER# to insert order number in message text");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageText", "Text of Message");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageEventType", "Event");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageEnabled", "Enabled");
			this.AddOrUpdatePluginLocaleResource("Plugins.Misc.SMS.MessageIsForAdmin", "Is for Admin");

			#endregion

			#region Add default messages

			var message1 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent",
				IsforAdmin = false,
				MessageText = "Your order #ORDERNUMBER# is placed. Total: #ORDERTOTAL#",
				Name = "Nop.Core.Domain.Orders.OrderPlacedEvent"
			};
			var message1Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent",
				IsforAdmin = true,
				MessageText = "Client has ordered #ORDERNUMBER#. Total: #ORDERTOTAL#",
				Name = "Nop.Core.Domain.Orders.OrderPlacedEvent"
			};

			var message2 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPaidEvent",
				IsforAdmin = false,
				MessageText = "Order #ORDERNUMBER# has been successfully paid. Total: #ORDERTOTAL#",
				Name = "Nop.Core.Domain.Orders.OrderPaidEvent"
			};
			var message2Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderPaidEvent",
				IsforAdmin = true,
				MessageText = "Client has paid #ORDERNUMBER#. Total: #ORDERTOTAL#",
				Name = "Nop.Core.Domain.Orders.OrderPaidEvent"
			};

			var message3 = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderCancelledEvent",
				IsforAdmin = false,
				MessageText = "Order #ORDERNUMBER# has been canceled",
				Name = "Nop.Core.Domain.Orders.OrderCancelledEvent"
			};
			var message3Admin = new SMSMessage
			{
				Enabled = true,
				EventType = "Nop.Core.Domain.Orders.OrderCancelledEvent",
				IsforAdmin = true,
				MessageText = "Client has canceled #ORDERNUMBER#.",
				Name = "Nop.Core.Domain.Orders.OrderCancelledEvent"
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

		public override void Uninstall()
		{
			#region Localization Resources Delete
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ConfigurationPage");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderEnabled");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderLogin");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderLoginRequired");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderPassword");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderPasswordRequired");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderApi");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderApi.Hint");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderApiRequired");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderAlfaName");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderAlfaName.Hint");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderEnableAlfaName");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderAdminPhoneNumber");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.ProviderLastmodified");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageName");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageName.Hint");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageText");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageEventType");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageEnabled");
			this.DeletePluginLocaleResource("Plugins.Misc.SMS.MessageIsForAdmin");
			#endregion

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

			#region Send message to client

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

			var adminMessage = _smsMessageRepository.Table
				.FirstOrDefault(m => m.IsforAdmin && m.Enabled && m.EventType == eventType);

			if (adminMessage != null)
			{
				var message = adminMessage.GetLocalized(x => x.MessageText);
				sms.PhoneNumber = _smsSettings.AdminPhoneNumber;
				sms.Message = message
					.Replace("#ORDERNUMBER#", order.Id.ToString())
					.Replace("#ORDERTOTAL#", order.OrderTotal.ToString());
				sms.SmsServerResponse = POSTRequest(sms);
				_smsRepository.Insert(sms);
			}

			#endregion
		}

		private string POSTRequest(Domain.SMS sms)
		{
			string apiUrl = sms.ApiUrl;
			string XML = _smsSettings.XML;

			if (XML != null)
			{
				//TODO: check are all fields replaced
				XML = XML.Replace("#PHONENUMBER#", sms.PhoneNumber)
						 .Replace("#ALFANAME#", sms.AlfaName)
						 .Replace("#MESSAGE#", sms.Message);
			}

			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiUrl);

				string authInfo = sms.Login + ":" + sms.Password;
				authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
				webRequest.Headers["Authorization"] = "Basic " + authInfo;

				byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(XML);
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

