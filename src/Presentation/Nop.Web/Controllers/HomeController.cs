using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Factories;
using Nop.Web.Framework.Security;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
	public partial class HomeController : BasePublicController
	{
		string cacheKey = "HOME_PAGE_MANUFACTURERS";

		private readonly ICacheManager _cacheManager;
		private readonly IPictureService _pictureService;
		private readonly IManufacturerService _manufacturerService;
		private readonly ICatalogModelFactory _catalogModelFactory;


		public HomeController(
			ICatalogModelFactory catalogModelFactory,
			ICacheManager cacheManager,
			IPictureService pictureService,
			IManufacturerService manufacturerService)
		{
			this._cacheManager = cacheManager;
			this._pictureService = pictureService;
			this._manufacturerService = manufacturerService;
			this._catalogModelFactory = catalogModelFactory;
		}

		[NopHttpsRequirement(SslRequirement.No)]
		public virtual ActionResult Index()
		{
			return View();
		}

		public virtual ActionResult HomePageManufacturers()
		{
			var model = _cacheManager.Get(cacheKey, () =>
			{
				return _catalogModelFactory.PrepareManufacturerAllModels();
			});

			return PartialView(model);
		}
	}
}
