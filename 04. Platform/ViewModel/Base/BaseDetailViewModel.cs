using System;

namespace RP.Platform.ViewModel
{
	public class BaseDetailViewModel : BaseViewModel
	{
		public int AccessTypeId { get; set; }
		public int EntityInfoId { get; set; }
		public EntityStateChangeViewModel EntityStateChange { get; set; }
		public int EntityStateId { get; set; }
		public int? EntityStateNameCode { get; set; }
		public int EntityTypeId { get; set; }
		public Guid Guid { get; set; }
		public bool HasEntityStateChangeAccess { get; set; }
		public int OrganizationId { get; set; }
	}
}