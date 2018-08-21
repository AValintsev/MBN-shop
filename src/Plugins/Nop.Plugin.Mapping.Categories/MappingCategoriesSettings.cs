using Nop.Core.Configuration;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Mapping.Categories
{
	public class MappingCategoriesSettings : ISettings
	{
		public string MappedCategoriesJson { get; set; }

		[Serializable]
		public class MapCategoryRow
		{
			public string GoupKey { get; set; }

			public int InternalCategoryId { get; set; }

			public string ExternalCategoryId { get; set; }
		}
	}
}
