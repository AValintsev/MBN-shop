using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Web.Framework;

namespace Nop.Plugin.Misc.SMS.Domain
{
	public class SMSMessage : BaseEntity, ILocalizedEntity
    {
        [NopResourceDisplayName("Plugins.Misc.SMS.MessageName")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.MessageText")]
        public string MessageText { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.MessageEventType")]
        public string EventType { get; set;}

        [NopResourceDisplayName("Plugins.Misc.SMS.MessageEnabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SMS.MessageIsForAdmin")]
        public bool IsforAdmin { get; set; }
    }
}
