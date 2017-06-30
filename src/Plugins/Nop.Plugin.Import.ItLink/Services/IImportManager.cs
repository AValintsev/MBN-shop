using Nop.Core.Domain.Catalog;
using System.Collections.Generic;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public interface IImportManager
	{
		Dictionary<string, int> GetCategoriesMapping();

		void ImportProducts(XmlDocument xmlProducts);
	}
}
