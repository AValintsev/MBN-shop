using System.Collections.Generic;
using System.Xml;

namespace Nop.Plugin.Import.ItLink.Services
{
	public interface IXmlImporter
	{
		 void ImportXml(XmlDocument recievedDocument);

	}
}
