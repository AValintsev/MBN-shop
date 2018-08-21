using Nop.Core.Domain.Catalog;
using System.Collections.Generic;
using static Nop.Plugin.Mapping.Categories.MappingCategoriesSettings;

namespace Nop.Plugin.Mapping.Categories.Models
{
	public class ConfigurationModel
	{
		public ConfigurationModel()
		{
			MappedCategories = new List<MappingModel>();
		}

		public List<MappingModel> MappedCategories { get; set; }


		public class MappingModel
		{
			public Category OriginalCategory { get; set; }

			public List<MapCategoryRow> MappedCategoriesRow { get; set; }

		}
	}
}
