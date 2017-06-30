using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public interface IXmlToXlsConverter
	{
		Stream XmlToXlsx(XmlDocument recievedDocument);
	}
}
