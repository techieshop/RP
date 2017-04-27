using System;

namespace RP.Platform.ViewModel
{
	public class DateTimeSelectListItem
	{
		public DateTime? DateTime { get; set; }
		public bool Selected { get; set; }
		public string Text { get; set; }
		public string Value { get; set; }
	}
}