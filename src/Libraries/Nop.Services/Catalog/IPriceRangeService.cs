using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
	public partial interface IPriceRangeService
	{
		PriceRange GetPriceRangeByCategory(int categoryId);
	}
}
