using RP.Common.Attribute;

namespace RP.Model
{
	public class BaseDetails<TDetail> where TDetail : BaseDetail
	{
		[Metadata(Order = 1)]
		public TDetail Detail { get; set; }
	}
}