using Nop.Plugin.Widgets.NivoSliderLocalized.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Data
{
	public class SliderItemMap : EntityTypeConfiguration<SliderItem>
	{
		public SliderItemMap()
		{
			ToTable("SliderItem");
			HasKey(m => m.Id);

			Property(m => m.PictureId);
			Property(m => m.Text);
			Property(m => m.Link);
		}
	}
}
