using Nop.Core;
using System;

namespace Nop.Plugin.Misc.SMS.Domain
{
	/// <summary>
	/// Already sent sms which is saved to database
	/// </summary>
	public class SMS : BaseEntity
	{
		public string Login { get; set; }

		public string Password { get; set; }

		public string ApiUrl { get; set; }

		public string AlfaName { get; set; }

		public string PhoneNumber { get; set; }

		public string Message { get; set; }

		public string SmsServerResponse { get; set; }

		public string EventType { get; set; }

		public DateTime Date { get; set; }
	}
}
