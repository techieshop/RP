namespace RP.Platform.ViewModel
{
	public class BaseDetailsViewModel<TDetailViewModel> where TDetailViewModel : BaseDetailViewModel
	{
		public TDetailViewModel Detail { get; set; }
	}
}