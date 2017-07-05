using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Import.ItLink.Domain;

namespace Nop.Plugin.Import.ItLink.Data
{
	public class CategoriesMap : EntityTypeConfiguration<InternalToExternal>
	{
		public CategoriesMap()
		{
			ToTable("InternalToExternal");
			HasKey(m => m.Id);
		}
	}
}
