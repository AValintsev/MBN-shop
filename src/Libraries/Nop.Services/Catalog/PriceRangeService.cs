using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
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

		private readonly IStoreContext _storeContext;
		private readonly ICacheManager _cacheManager;

		#endregion

		#region Ctor

		public PriceRangeService(
			ICacheManager cacheManager,
			IRepository<ProductCategory> productCategoryRepository,
			IRepository<Product> productRepository,
			IStoreContext storeContext)
		{
			this._cacheManager = cacheManager;
			this._storeContext = storeContext;
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
				return result.SingleOrDefault();
			});
		}
	}
}
