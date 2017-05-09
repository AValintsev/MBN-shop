using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Misc.SMS.Data
{
	public class SMSMap : EntityTypeConfiguration<Domain.SMS>
	{
		public SMSMap()
		{
			ToTable("SMS");
			HasKey(m => m.Id);

			Property(m => m.Login);
			Property(m => m.Password);
			Property(m => m.ApiUrl);
			Property(m => m.XML);
			Property(m => m.AlfaName);
			Property(m => m.PhoneNumber);
			Property(m => m.Message);
			Property(m => m.SmsServerResponse);
			Property(m => m.EventType);
			Property(m => m.Date);
		}
	}
}
