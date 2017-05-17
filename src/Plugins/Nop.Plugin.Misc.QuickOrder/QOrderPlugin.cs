using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.QuickOrder
{
    public class QOrderPlugin : BasePlugin, IMiscPlugin , IWidgetPlugin
    {
        #region fields

        private readonly ISettingService _settingService;
        private readonly IRepository<Language> _languageRepository;
        private readonly QOrderSettings _qOrderSettings;

        #endregion

        public QOrderPlugin(
            IRepository<Language> languageRepository,
            ISettingService settingsService,
            QOrderSettings qrderSettings
        )
        {
            _languageRepository = languageRepository;
            _settingService = settingsService;
            _qOrderSettings = qrderSettings;
        }

        public bool Authenticate()
        {
            return true;
        }

        public override void Install()
        {
            InstallLocaleResources();

            var settings = new QOrderSettings()
            {
               Enabled = false,
               NameEnabled = true,
               NameRequired = true,
               EmailAddressEnabled = true,
               EmailAddressRequired = true,
               PhoneEnabled = false,
               PhoneRequired = false,
               WidgetZone = "order_summary_content_after"
            };
            _settingService.SaveSetting(settings);

            base.Install();
        }

        protected virtual void InstallLocaleResources()
        {
            ////English language
            //var languageEng = _languageRepository.Table.Single(l => l.Name == "English");

            //if (languageEng != null)
            //{
            //    //save resources
            //    foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.QuickOrder/App_Data/Translations"), "en_misc.quickOrder.xml", SearchOption.TopDirectoryOnly))
            //    {
            //        var localesXml = File.ReadAllText(filePath);
            //        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            //        localizationService.ImportResourcesFromXml(languageEng, localesXml);
            //    }
            //}

            //Russian language
            var languageRu = _languageRepository.Table.SingleOrDefault(l => l.Name == "Russian");

            if (languageRu != null)
            {
                //save resources
                foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.QuickOrder/App_Data/Translations"), "ru_misc.quickOrder.xml", SearchOption.TopDirectoryOnly))
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
                foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/Plugins/Misc.QuickOrder/App_Data/Translations"), "ua_misc.quickOrder.xml", SearchOption.TopDirectoryOnly))
                {
                    var localesXml = File.ReadAllText(filePath);
                    var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                    localizationService.ImportResourcesFromXml(languageUa, localesXml);
                }
            }
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<QOrderSettings>();


            base.Uninstall();
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
            controllerName = "QuickOrder";
            routeValues = new RouteValueDictionary
            {
                { "Namespaces", "Nop.Plugin.Misc.QuickOrder.Controllers" },
                { "area", null }
            };
        }

        public IList<string> GetWidgetZones()
        {
            List<string> stringList;
            if (!string.IsNullOrWhiteSpace(_qOrderSettings.WidgetZone))
                stringList = new List<string>()
        {
          _qOrderSettings.WidgetZone
        };
            else
                stringList = new List<string>()
        {
          "order_summary_content_after"
        };
            return (IList<string>) stringList;
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Form";
            controllerName = "QuickOrder";
            routeValues = new RouteValueDictionary
                  {
                      {"Namespaces", "Nop.Plugin.Misc.QuickOrder.Controllers"},
                      {"area", null},
                      {"widgetZone", widgetZone}
                  };
        }

        #endregion
    }
}