using Nop.Core.Domain.Catalog;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Plugin.Mapping.Categories.Services
{
	public interface IExportXml<T> where T : class
	{
		string ExportProductsToXml(IList<Product> products, UrlHelper urlHelper, int storeConfiguration);
		string Key { get; }
	}
}
