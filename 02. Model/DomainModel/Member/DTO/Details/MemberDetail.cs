using System;

namespace RP.Model
{
	public class MemberDetail : BaseDetail
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string OrganizationName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public int? GenderId { get; set; }
		public int PigeonCount { get; set; }
	}
}