using System.Collections.Generic;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public interface IItLinkImportManager
	{
		/// <summary>
		/// Imports products from ItLinks xml.
		/// </summary>
		/// <param name="xmlDocument">xml document</param>
		/// <param name="categoriesMap">dictionary key = ItLink cateogry name, value = internal category Id</param>
		/// <param name="overrideExistedImanges">true if want to override existed images (could be clower).</param>
		/// <returns>Lists with errors.</returns>
		List<string> Import(XmlDocument xmlDocument, Dictionary<string, int> categoriesMap, bool overrideExistedImanges = false);
	}
}
