using Nop.Core.Data;
using Nop.Core.Plugins;
using Nop.Plugin.Misc.SMS.Data;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.SMS.Domain;
using System.Web.Routing;
using Nop.Services.Events;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Core.Domain.Customers;
using Nop.Services.Configuration;
using Nop.Core.Configuration;
using Nop.Core.Domain.Configuration;
using System.Linq.Expressions;
using Nop.Services.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Nop.Services.Localization;
using System.IO;

namespace Nop.Plugin.Misc.SMS
{
    public class SMSPlugin : BasePlugin, IAdminMenuPlugin, IConsumer<OrderPlacedEvent>
    {
        #region fields
        private SMSObjectContext _context;
        private IRepository<Domain.SMS> _trialRepo;
        private IRepository<SMSMessage> _messagesRepo;
        private IRepository<SMSProvider> _providerRepo;
        private SMSProvider _provider;
        #endregion

        public SMSPlugin(SMSObjectContext context, IRepository<Domain.SMS> trialRepo,
            IRepository<SMSMessage> messagesRepo, IRepository<SMSProvider> providerRepo)
        {
            _context = context;
            _trialRepo = trialRepo;
            _messagesRepo = messagesRepo;
            _providerRepo = providerRepo;
        }
        public bool Authenticate()
        {
            return true;
        }

        public override void Install()
        {
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
            _context.Install();
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

            _context.Uninstall();
            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Misc.SMS",
                Title = "SMS",
                ControllerName = "SMS",
                ActionName = "Manage",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            _provider = _providerRepo.Table.SingleOrDefault();

            if (!_provider.Enabled)
                return;

            var eventType = eventMessage.GetType().ToString();

            var message = _messagesRepo.Table.Where(m =>
            m.Enabled == true && m.EventType == eventType).FirstOrDefault() ??
            new SMSMessage { MessageText = "TEST MESSAGE",
                Enabled = true, EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent", Id = 1 };

            
            string messageText = message.MessageText.Replace("#ORDERNUMBER#", eventMessage.Order.Id.ToString());



            Domain.SMS sms = new Domain.SMS
            {
                Message = messageText,
                Login = _provider.Login,
                Password = _provider.Password,
                Api = _provider.Api,
                AlfaName = _provider.EnableAlfaName ? _provider.AlfaName : null,
                EventType = eventType,
                PhoneNumber = eventMessage.Order.Customer.ShippingAddress.PhoneNumber ??
                eventMessage.Order.Customer.BillingAddress.PhoneNumber,
                Date = DateTime.Now,
            };

            _trialRepo.Insert(sms);


            GetRequest(sms);

        }


        public void GetRequest(Domain.SMS sms)
        {
            string api = sms.Api;

            api = api.Replace("#MESSAGE#", sms.Message);
            api = api.Replace("#MOBILEPHONE#", sms.PhoneNumber);
            api = api.Replace("#LOGIN#", sms.Login);
            api = api.Replace("#PASSWORD#", sms.Password);

            string xmlwithAlfa = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<request>" +
                "<operation>SENDSMS</operation>" +
                "<message start_time = \" AUTO \" end_time = \" AUTO \" lifetime = \"4\" rate = \"120\" desc = \"My first campaign \"  source=\"" + sms.AlfaName + "\" >" +
                "<body>" + sms.Message + "</body>" +
                "<recipient>" + sms.PhoneNumber + "</recipient>" +
                "</message>" +
                "</request>";

            string xmlwithOutAlfa = "<?xml version = \"1.0\" encoding = \"utf-8\" ?>" +
                "<request>" +
                "<operation>SENDSMS</operation>" +
                "<message start_time = \"AUTO\" end_time = \"AUTO\" lifetime = \"4\" rate = \"60\" desc = \"description\" type = \"single\">" +
                "<recipient>"+sms.PhoneNumber+"</recipient>" +
                "<body>"+sms.Message+"</body>" +
                "</message>" +
                "</request>";

                string xml = sms.AlfaName != null ? xmlwithAlfa : xmlwithOutAlfa;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(api);

                string authInfo = sms.Login + ":" + sms.Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                req.Headers["Authorization"] = "Basic " + authInfo;


                //string s = "id="+Server.UrlEncode(xml);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(xml);
                req.Method = "POST";
                req.ContentType = "text/xml;charset=utf-8";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();


                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8);
                string backstr = sr.ReadToEnd();


                sr.Close();
                res.Close();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
    }
    }

