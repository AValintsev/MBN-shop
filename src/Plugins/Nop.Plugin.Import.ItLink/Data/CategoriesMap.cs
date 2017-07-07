using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Import.ItLink.Domain;

namespace Nop.Plugin.Import.ItLink.Data
{
	public class CategoriesMap : EntityTypeConfiguration<CategoryInternalToExternalMap>
	{
		public CategoriesMap()
		{
			ToTable("CategoryInternalToExternalMap");
			HasKey(m => m.Id);
		}
	}
}
