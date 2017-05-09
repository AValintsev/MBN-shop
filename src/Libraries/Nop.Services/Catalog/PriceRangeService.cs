using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Services.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
	public partial class PriceRangeService : IPriceRangeService
	{
		#region fields

		private const string PRICERANGE_BYCATEGORYID_KEY = "Nop.pricerange.bycategoryid-{0}-{1}";

		private readonly IRepository<ProductCategory> _productCategoryRepository;
		private readonly IRepository<Product> _productRepository;

		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly ICacheManager _cacheManager;
		private readonly ICurrencyService _currencyService;

		#endregion

		#region Ctor

		public PriceRangeService(
			IWorkContext _workContext,
			ICacheManager cacheManager,
			ICurrencyService currencyService,
			IRepository<ProductCategory> productCategoryRepository,
			IRepository<Product> productRepository,
			IStoreContext storeContext)
		{
			this._workContext = _workContext;
			this._cacheManager = cacheManager;
			this._storeContext = storeContext;
			this._currencyService = currencyService;
			this._productRepository = productRepository;
			this._productCategoryRepository = productCategoryRepository;
		}

		#endregion

		public PriceRange GetPriceRangeByCategory(int categoryId)
		{
			string key = string.Format(PRICERANGE_BYCATEGORYID_KEY, categoryId, _storeContext.CurrentStore.Id);

			return _cacheManager.Get(key, () =>
			{
				var query = from p in _productRepository.Table
							join pcm in _productCategoryRepository.Table on p.Id equals pcm.ProductId
							where pcm.CategoryId == categoryId
							group p by pcm.CategoryId into g
							select new PriceRange
							{
								From = g.Min(p => p.Price),
								To = g.Max(p => p.Price)
							};

				var result = query.ToList();

				var priceRange = result.SingleOrDefault();
				if (priceRange != null)
				{
					priceRange.From = _currencyService.ConvertFromPrimaryStoreCurrency(priceRange.From.Value, _workContext.WorkingCurrency);
					priceRange.To = _currencyService.ConvertFromPrimaryStoreCurrency(priceRange.To.Value, _workContext.WorkingCurrency);
				}
				return priceRange;
			});
		}
	}
}
