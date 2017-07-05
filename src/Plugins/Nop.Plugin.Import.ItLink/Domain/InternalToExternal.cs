using Nop.Core;

namespace Nop.Plugin.Import.ItLink.Domain
{
	public class InternalToExternal : BaseEntity
	{
		public int InternalId { get; set;}

		public string ExternalId { get; set; }

		public string ExternalName { get; set;}
	}
}
