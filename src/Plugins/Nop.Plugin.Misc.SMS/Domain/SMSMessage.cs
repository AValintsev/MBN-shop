using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Web.Framework;

namespace Nop.Plugin.Misc.SMS.Domain
{
	public class SMSMessage : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }

        public string MessageText { get; set; }

        public string EventType { get; set;}

        public bool Enabled { get; set; }

        public bool IsforAdmin { get; set; }
    }
}
