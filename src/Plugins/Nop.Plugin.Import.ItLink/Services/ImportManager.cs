using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class ImportManager : IImportManager
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;

		public ImportManager(
			ICategoryService categoryService,
			IProductService productService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		public Dictionary<string, int> GetCategoriesMapping()
		{
			Dictionary<string, int> result = new Dictionary<string, int>();

			try
			{
				string path = CommonHelper.MapPath("~/Plugins/Import.ItLink/App_Data/CategoriesMapping.xml");

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(path);

				foreach (XmlNode category in xmlDoc.GetElementsByTagName("category"))
				{
					result.Add(category.Attributes["id"].Value, int.Parse(category.Attributes["MbnId"].Value));
				}

				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void ImportProducts(XmlDocument products)
		{
			//How to save images
			//using (var client = new WebClient())
			//{
			//	var imgUrl = "http://it-link.ua/Content/images/prods/00143524.jpg";
			//	client.DownloadFile(imgUrl, string.Format("{0}/{1}.jpg", "ImageSavingPath", "imageName"));
			//}
			
			throw new NotImplementedException();
		}
	}
}
