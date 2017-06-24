using System;
using System.IO;
using System.Xml;

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
			throw new NotImplementedException();
		}
	}
}
