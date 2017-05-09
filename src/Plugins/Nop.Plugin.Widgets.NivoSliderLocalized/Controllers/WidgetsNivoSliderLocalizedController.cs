using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.Widgets.NivoSliderLocalized.Domain;
using Nop.Plugin.Widgets.NivoSliderLocalized.Infrastructure.Cache;
using Nop.Plugin.Widgets.NivoSliderLocalized.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Controllers
{
	public class WidgetsNivoSliderLocalizedController : BasePluginController
	{
		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly IStoreService _storeService;
		private readonly IPictureService _pictureService;
		private readonly ISettingService _settingService;
		private readonly ICacheManager _cacheManager;

		private readonly ILanguageService _languageService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;

		private readonly IRepository<SliderItem> _sliderItemRepository;

		public WidgetsNivoSliderLocalizedController(
			IRepository<SliderItem> sliderItemRepository,
			IWorkContext workContext,
			IStoreContext storeContext,
			IStoreService storeService,
			IPictureService pictureService,
			ISettingService settingService,
			ICacheManager cacheManager,
			ILanguageService languageService,
			ILocalizationService localizationService,
			ILocalizedEntityService localizedEntityService)
		{
			this._sliderItemRepository = sliderItemRepository;
			this._workContext = workContext;
			this._storeContext = storeContext;
			this._storeService = storeService;
			this._pictureService = pictureService;
			this._settingService = settingService;
			this._cacheManager = cacheManager;
			this._languageService = languageService;
			this._localizedEntityService = localizedEntityService;
			this._localizationService = localizationService;
		}

		protected string GetPictureUrl(string pictureId)
		{
			string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
			return _cacheManager.Get(cacheKey, () =>
			{
				var url = _pictureService.GetPictureUrl(int.Parse(pictureId), showDefaultPicture: false);
				//little hack here. nulls aren't cacheable so set it to ""
				if (url == null)
					url = "";

				return url;
			});
		}

		[AdminAuthorize]
		[ChildActionOnly]
		public ActionResult Configure(bool? addNew)
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

			var viewModel = new List<ConfigurationModel>();

			var entities = _sliderItemRepository.Table.ToList();

			if (addNew.HasValue && addNew.Value == true)
			{
				entities.Add(new SliderItem { PictureId = "0" });
			}

			entities.ForEach(entity =>
			{
				var model = new ConfigurationModel
				{
					Id = entity.Id,
					PictureId = int.Parse(entity.PictureId),
					Text = entity.Text,
					Link = entity.Link,
					ActiveStoreScopeConfiguration = storeScope
				};

				AddLocales(_languageService, model.Locales, (locale, languageId) =>
				{
					locale.PictureId = int.Parse(entity.GetLocalized(x => x.PictureId, languageId));
					locale.Link = entity.GetLocalized(x => x.Link, languageId);
					locale.Text = entity.GetLocalized(x => x.Text, languageId);
				});

				viewModel.Add(model);
			});

			return View("~/Plugins/Widgets.NivoSliderLocalized/Views/Configure.cshtml", viewModel);
		}

		[HttpPost, ActionName("Configure")]
		[AdminAuthorize]
		[ChildActionOnly]
		[FormValueRequired("addNew")]
		public ActionResult Configure()
		{
			return Configure(true);
		}

		[HttpPost, ActionName("Configure")]
		[AdminAuthorize]
		[ChildActionOnly]
		[FormValueRequired("save")]
		public ActionResult Configure(List<ConfigurationModel> viewModelList)
		{
			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

			if (viewModelList != null && viewModelList.Count > 0)
			{
				viewModelList.ForEach(vm =>
				{
					if (vm.PictureId > 0)
					{
						var entity = _sliderItemRepository.Table.FirstOrDefault(si => si.Id == vm.Id);

						if (entity != null)
						{
							entity.Link = vm.Link;
							entity.PictureId = vm.PictureId.ToString();
							entity.Text = vm.Text;

							_sliderItemRepository.Update(entity);
						}
						else
						{
							entity = new SliderItem
							{
								Link = vm.Link,
								PictureId = vm.PictureId.ToString(),
								Text = vm.Text
							};

							_sliderItemRepository.Insert(entity);
						}

						foreach (var locale in vm.Locales)
						{
							_localizedEntityService.SaveLocalizedValue(entity, e => e.Text, locale.Text, locale.LanguageId);
							_localizedEntityService.SaveLocalizedValue(entity, e => e.Link, locale.Link, locale.LanguageId);
							_localizedEntityService.SaveLocalizedValue(entity, e => e.PictureId, locale.PictureId.ToString(), locale.LanguageId);
						}
					}
				});

				SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
			}

			return Configure();
		}

		[HttpPost]
		[AdminAuthorize]
		public ActionResult DeleteSlide(int? id)
		{
			if (id.HasValue)
			{
				var entity = _sliderItemRepository.GetById(id.Value);

				if (entity != null)
				{
					foreach (var lang in _languageService.GetAllLanguages())
					{
						var pictureId = entity.GetLocalized(x => x.PictureId, lang.Id);
						var picutre = _pictureService.GetPictureById(int.Parse(pictureId));
						if (picutre != null)
						{
							_pictureService.DeletePicture(picutre);
						}

						_localizedEntityService.SaveLocalizedValue(entity, e => e.Text, "", lang.Id);
						_localizedEntityService.SaveLocalizedValue(entity, e => e.Link, "", lang.Id);
						_localizedEntityService.SaveLocalizedValue(entity, e => e.PictureId, "", lang.Id);
					}

					_sliderItemRepository.Delete(entity);
				}
			}

			return new JsonResult
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = "success"
			};
		}

		[ChildActionOnly]
		public ActionResult PublicInfo(string widgetZone, object additionalData = null)
		{
			var viewModel = new List<PublicInfoModel>();

			var entities = _sliderItemRepository.Table.ToList();

			entities.ForEach(entity =>
			{
				var model = new PublicInfoModel
				{
					PictureUrl = GetPictureUrl(entity.GetLocalized(x => x.PictureId)),
					Text = entity.Text,
					Link = entity.Link
				};

				viewModel.Add(model);
			});

			return View("~/Plugins/Widgets.NivoSliderLocalized/Views/PublicInfo.cshtml", viewModel);
		}
	}
}