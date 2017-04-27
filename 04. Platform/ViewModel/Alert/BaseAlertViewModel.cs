namespace RP.Platform.ViewModel
{
	public enum AlertType
	{
		Success,
		Info,
		Warning,
		Danger
	}

	public class BaseAlertViewModel
	{
		public AlertType AlertType { get; set; }
		public bool CanClose { get; set; }
		public string Message { get; set; }
		public int? MessageCode { get; set; }
	}
}