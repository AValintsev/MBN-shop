﻿using Nop.Plugin.Misc.SMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
namespace Nop.Plugin.Misc.SMS.Data
{
    public class SMSProviderMap : EntityTypeConfiguration<Domain.SMSProvider>
    {
        public SMSProviderMap()
        {
            ToTable("SMSProvider");
            HasKey(m => m.Id);

            Property(m => m.Login);
            Property(m => m.Password);
            Property(m => m.Enabled);
            Property(m => m.Api);
            Property(m => m.AdminPhoneNumber);
        }
    }
}