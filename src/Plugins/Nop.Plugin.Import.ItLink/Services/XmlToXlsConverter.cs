using Microsoft.Office.Interop.Excel;
using Nop.Admin.Models.Catalog;
using Nop.Core.Domain.Catalog;
using System;
using System.IO;
using System.Xml;
using Exel = Microsoft.Office.Interop.Excel;

namespace Nop.Plugin.Import.ItLink.Services
{
	public class XmlToXlsConverter : IXmlToXlsConverter
	{
		/// <summary>
		/// Converts input xml to excel xls document and returns a stream.
		/// </summary>
		/// <param name="xmlDocument">Xml document to convert.</param>
		/// <returns>Excel (xls) stream.</returns>
		public Stream ConvertFromXml(XmlDocument xmlDocument)
		{
			//Создать новый XmlDocument

			//Потом конвертировать в нужную нам структуру

			//Потом получить xlsx из xml

			throw new NotImplementedException(@"//Создать новый XmlDocument

			//Потом конвертировать в нужную нам структуру

			//Потом получить xlsx из xml");

		}
	}
}
