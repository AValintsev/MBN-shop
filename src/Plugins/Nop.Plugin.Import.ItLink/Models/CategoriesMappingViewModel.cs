using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.Import.ItLink.Models
{
	public class CategoriesMappingViewModel
	{
		public CategoriesMappingViewModel()
		{
			this.InternalCategories = new List<SelectListItem>();
		}

		public int Id { get; set; }
		
		public string ExternalCategoryId { get; set; }

		public string ExternalCategoryName { get; set; }

		public int InternalSelectedCategoryId { get; set; }

		public List<SelectListItem> InternalCategories { get; set; }
	}
}
