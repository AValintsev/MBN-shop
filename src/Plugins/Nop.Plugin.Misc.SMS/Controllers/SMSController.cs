using Nop.Core.Data;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.SMS.Domain;
using System.Web.Mvc;
using System.Net;
using System.IO;

namespace Nop.Plugin.Misc.SMS.Controllers
{
    public class SMSController : BasePluginController
    {
        private IRepository<Domain.SMS> _smsRepository;
        private IRepository<SMSProvider> _providerRepo;
        private IRepository<SMSMessage> _messagesRepo;

        public SMSController(IRepository<Domain.SMS> trialRepository, IRepository<SMSProvider> providerRepo, IRepository<SMSMessage> messagesRepo)
        {
            _smsRepository = trialRepository;
            _providerRepo = providerRepo;
            _messagesRepo = messagesRepo;
        }

        [AdminAuthorize]
        public ActionResult Manage()
        {
            if (_providerRepo.Table.Any())
            {
                SMSProvider model = _providerRepo.Table.SingleOrDefault();
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult Manage(SMSProvider provider)
        {
            provider.LastConfigurationDate = DateTime.Now;
                var old = _providerRepo.Table.SingleOrDefault();
            if(old != null)
            _providerRepo.Delete(old);

            _providerRepo.Insert(provider);
            return Manage();
        }

        [AdminAuthorize]
        public PartialViewResult MessagesPartial()
        {
           var messages  =  _messagesRepo.Table.Select(m => m).ToList();
            if (messages.Any())
            {
                return PartialView(messages);
            }
            SMSMessage message = new SMSMessage
            {
                Id = 100,
                MessageText = "TEST MESSAGE",
                Enabled = false,
                EventType = "Nop.Core.Domain.Orders.OrderPlacedEvent",
                Name = "Заказ Товара",
                IsforAdmin = false,
            };

            List<SMSMessage> model = new List<SMSMessage>();
            model.Add(message);

            return PartialView(model);
        }

        [HttpPost]
        [AdminAuthorize]
        public RedirectToRouteResult Messages(List<SMSMessage> messages)
        {
            foreach (SMSMessage newMesessage in messages)
            {
                var oldMessage = _messagesRepo.Table.FirstOrDefault(m => m.Id == newMesessage.Id);
                if(oldMessage != null)
                _messagesRepo.Delete(oldMessage);

                _messagesRepo.Insert(newMesessage);
            }
            return RedirectToAction("Manage");
        }

        [AdminAuthorize]
        public RedirectToRouteResult GetBalance()
        {
           
            SMSProvider _provider = _providerRepo.Table.SingleOrDefault();

            string xml = "<?xml version = \"1.0\" encoding = \"utf-8\" ?>" +
                 "<request>" +
                 "<operation>GETBALANCE</operation>" +
                 "</request>";

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_provider.Api);

                string authInfo = _provider.Login + ":" + _provider.Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                req.Headers["Authorization"] = "Basic " + authInfo;


                //string s = "id="+Server.UrlEncode(xml);
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(xml);
                req.Method = "POST";
                req.ContentType = "text/xml;charset=utf-8";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();


                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                string backstr = sr.ReadToEnd();

                sr.Close();
                res.Close();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return RedirectToAction("Manage");
        }
    }
}
