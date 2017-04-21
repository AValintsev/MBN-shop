using Nop.Core;
using Nop.Core.Data;
using Nop.Plugin.Misc.SMS.Domain;
using Nop.Plugin.Misc.SMS.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.SMS.Controllers
{
	[AdminAuthorize]
	public class MiscSMSController : BasePluginController
	{
		#region Fields

		private readonly IWorkContext _workContext;
		private readonly IStoreService _storeService;
		private readonly ISettingService _settingService;
		private readonly IStoreContext _storeContext;

		private readonly ILanguageService _languageService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;

		private readonly IRepository<Domain.SMS> _smsRepository;
		private readonly IRepository<SMSMessage> _smsMessageRepository;

		#endregion

		public MiscSMSController(
			IWorkContext workContext,
			IStoreService storeService,
			ISettingService settingService,
			IStoreContext storeContext,
			ILanguageService languageService,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService,
			IRepository<Domain.SMS> smsRepository,
			IRepository<SMSMessage> smsMessageRepostitory)
		{
			this._workContext = workContext;
			this._storeService = storeService;
			this._settingService = settingService;
			this._storeContext = storeContext;

			this._languageService = languageService;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;

			this._smsRepository = smsRepository;
			this._smsMessageRepository = smsMessageRepostitory;
		}

		[ChildActionOnly]
		public ActionResult Configure()
		{
			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var smsSettings = _settingService.LoadSetting<SMSSettings>(storeScope);

			var viewModel = new ConfigurationModel
			{
				AdminPhoneNumber = smsSettings.AdminPhoneNumber,
				AlfaName = smsSettings.AlfaName,
				ApiUrl = smsSettings.ApiUrl,
				EnableAlfaName = smsSettings.EnableAlfaName,
				Enabled = smsSettings.Enabled,
				LastConfigurationDate = smsSettings.LastConfigurationDate,
				Login = smsSettings.Login,
				Password = smsSettings.Password,
				XML = smsSettings.XML
			};

			return View("~/Plugins/Misc.SMS/Views/Configure.cshtml", viewModel);
		}

		[HttpPost]
		[ChildActionOnly]
		[FormValueRequired("save")]
		public ActionResult Configure(ConfigurationModel viewModel)
		{
			if (!ModelState.IsValid)
				return Configure();

			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var smsSettings = _settingService.LoadSetting<SMSSettings>(storeScope);

			//save settings
			smsSettings.AdminPhoneNumber = viewModel.AdminPhoneNumber;
			smsSettings.AlfaName = viewModel.AlfaName;
			smsSettings.ApiUrl = viewModel.ApiUrl;
			smsSettings.EnableAlfaName = viewModel.EnableAlfaName;
			smsSettings.Enabled = viewModel.Enabled;			
			smsSettings.LastConfigurationDate = DateTime.UtcNow;
			smsSettings.Login = viewModel.Login;
			smsSettings.Password = viewModel.Password;
			smsSettings.XML = viewModel.XML;

			_settingService.SaveSetting(smsSettings);

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

			return Configure();
		}

		[ChildActionOnly]
		public PartialViewResult MessagesPartial()
		{
			var viewModel = new List<SMSMessageViewModel>();

			var entites = _smsMessageRepository.Table.ToList();

			entites.ForEach(entity =>
			{
				var model = new SMSMessageViewModel
				{
					Id = entity.Id,
					Enabled = entity.Enabled,
					EventType = entity.EventType,
					IsforAdmin = entity.IsforAdmin,
					MessageText = entity.MessageText,
					Name = entity.Name
				};
				AddLocales(_languageService, model.Locales, (locale, languageId) =>
				{
					locale.MessageText = entity.GetLocalized(x => x.MessageText, languageId);
				});

				viewModel.Add(model);
			});

			return PartialView("~/Plugins/Misc.SMS/Views/MessagesPartial.cshtml", viewModel);
		}

		[HttpPost, ActionName("Configure")]
		[ChildActionOnly]
		[FormValueRequired("savemessages")]
		public ActionResult Messages(List<SMSMessageViewModel> viewModelList)
		{
			viewModelList.ForEach(vm =>
			{
				var existed = _smsMessageRepository.Table.FirstOrDefault(sms => sms.Id == vm.Id);
				if (existed != null)
				{
					existed.Enabled = vm.Enabled;
					existed.MessageText = vm.MessageText;

					_smsMessageRepository.Update(existed);

					foreach (var locale in vm.Locales)
					{
						_localizedEntityService.SaveLocalizedValue(existed, e => e.MessageText, locale.MessageText, locale.LanguageId);
					}
				}
			});

			return Configure();
		}
	}
}
