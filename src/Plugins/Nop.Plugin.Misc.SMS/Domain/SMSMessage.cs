using Nop.Core;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SMS.Domain
{
    public class SMSMessage : BaseEntity
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
