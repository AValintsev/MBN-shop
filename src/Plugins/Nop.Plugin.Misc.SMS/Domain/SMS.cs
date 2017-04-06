using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Misc.SMS.Domain
{
    public class SMS : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Api { get; set; }
        public string AlfaName { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string EventType { get; set; }
        public DateTime Date { get; set; }
    }
}
