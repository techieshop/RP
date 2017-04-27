using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class EntityStateChangedViewModel
	{
		public int EntityStateId { get; set; }
		public string EntityStateName { get; set; }
	}

	public class EntityStateChangeViewModel
	{
		public int EntityInfoId { get; set; }
		public EntityStateChangedViewModel EntityStateChanged { get; set; }
		public IDictionary<int, string> EntityTransitions { get; set; }
		public int OrganizationId { get; set; }
	}

	public class EntityTransitionViewModel
	{
		public int EntityInfoId { get; set; }
		public int EntityTransitionId { get; set; }
		public int OrganizationId { get; set; }
	}
}