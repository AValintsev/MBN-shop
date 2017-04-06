using Nop.Plugin.Misc.SMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
namespace Nop.Plugin.Misc.SMS.Data
{
    public class SMSMessageMap : EntityTypeConfiguration<SMSMessage>
    {
        public SMSMessageMap()
        {
            ToTable("SMSMessage");
            HasKey(m => m.Id);

            Property(m => m.Name);
            Property(m => m.MessageText);
            Property(m => m.EventType);
            Property(m => m.Enabled);
            Property(m => m.IsforAdmin);
        }
    }
}
