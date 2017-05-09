using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.SMS.Models
{
	public class SMSMessageViewModel : ILocalizedModel<SMSMessageViewModel.SMSMessageLocalizedModel>
	{
		#region Nested class

		public class SMSMessageLocalizedModel : ILocalizedModelLocal
		{
			public int LanguageId { get; set; }

			[NopResourceDisplayName("Plugins.Misc.SMS.MessageText")]
			public string MessageText { get; set; }
		}

		#endregion

		public IList<SMSMessageLocalizedModel> Locales { get; set; }

		public SMSMessageViewModel()
		{
			Locales = new List<SMSMessageLocalizedModel>();
		}

		public int Id { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.MessageName")]
		public string Name { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.MessageText")]
		public string MessageText { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.MessageEventType")]
		public string EventType { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.MessageEnabled")]
		public bool Enabled { get; set; }

		[NopResourceDisplayName("Plugins.Misc.SMS.MessageIsForAdmin")]
		public bool IsforAdmin { get; set; }
	}
}