using System;

namespace RP.Model
{
	public class UserData : BaseModel
	{
		public Guid Guid { get; set; }
		public bool HasPhoto { get; set; }
		public int LanguageId { get; set; }
		public int OrganizationId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}