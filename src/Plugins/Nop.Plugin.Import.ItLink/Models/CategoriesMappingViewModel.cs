using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.Import.ItLink.Models
{
	public class CategoriesMappingViewModel
	{
		public int Id { get; set; }
		
		public int InternalId{ get; set; }

		public string InternalName { get; set; }

		public string ExternalId { get; set; }

		public string ExternalName { get; set; }

		public List<SelectListItem> InternalCategoriesSelectList { get; set; }

		public CategoriesMappingViewModel()
		{
			InternalCategoriesSelectList = new List<SelectListItem>();
		}
	}
}
