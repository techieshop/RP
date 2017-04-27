using System;

namespace RP.Model
{
	public class BaseDetail : BaseModel
	{
		public int AccessTypeId { get; set; }
		public int EntityInfoId { get; set; }
		public int EntityStateId { get; set; }
		public int EntityTypeId { get; set; }
		public Guid Guid { get; set; }
		public int OrganizationId { get; set; }
	}
}