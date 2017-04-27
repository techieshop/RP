using System;

namespace RP.Model
{
	public class PublicMemberDetail : BaseDetail
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Height { get; set; }
		public int? AddressId { get; set; }
		public int? WebsiteId { get; set; }
		public int? GenderId { get; set; }
		public int PigeonCount { get; set; }
	}
}