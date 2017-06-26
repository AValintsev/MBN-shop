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
		public Stream ConvertFromXml(XmlDocument recievedDocument)
		{

			XmlNodeList offers = recievedDocument.GetElementsByTagName("offers");

			//Шаблон из Nopcommerce
			XmlDocument doc = new XmlDocument();
			doc.Load("~/products.xml");

			XmlNode productPatern = doc.SelectSingleNode("//Prodct");


			//Создать новый XmlDocument
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode rootNode = xmlDoc.CreateElement("Products");
			xmlDoc.AppendChild(rootNode);



			foreach (XmlNode offer in offers)
			{
				XmlElement product = doc.CreateElement("Product");
				xmlDoc.AppendChild(product);

				productPatern.SelectSingleNode("/Name").InnerText = offer.SelectSingleNode("/name").InnerText;
				productPatern.SelectSingleNode("/Price").InnerText = offer.SelectSingleNode("/name").InnerText;
				productPatern.SelectSingleNode("/OldPrice").InnerText = offer.SelectSingleNode("/oldprice").InnerText;

			}

			//Потом конвертировать в нужную нам структуру

			//Потом получить xlsx из xml

			throw new NotImplementedException(@"//Создать новый XmlDocument

			//Потом конвертировать в нужную нам структуру

			//Потом получить xlsx из xml");






		}
	}
}
