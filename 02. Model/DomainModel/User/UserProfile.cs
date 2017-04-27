using System;

namespace RP.Model
{
	public class ProfileModel : BaseModel
	{
		public DateTime? DateOfBirth { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public int? GenderId { get; set; }
		public int LanguageId { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Mobile { get; set; }
		public string Password { get; set; }
		public string Phone { get; set; }
		public string Salutation { get; set; }
	}

	public class UserProfile
	{
		public ProfileModel Profile { get; set; }
		public Address Address { get; set; }
	}
}