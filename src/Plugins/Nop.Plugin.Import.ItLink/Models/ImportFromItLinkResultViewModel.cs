using System.Collections.Generic;

namespace Nop.Plugin.Import.ItLink.Models
{
	public class ImportFromItLinkResultViewModel
	{
		public IList<string> Errors { get; set; }

		public IList<string> Messages { get; set; }

		public ImportFromItLinkResultViewModel()
		{
			Errors = new List<string>();
			Messages = new List<string>();
		}
	}
}
