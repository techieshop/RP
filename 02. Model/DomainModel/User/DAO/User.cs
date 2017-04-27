using System;
using System.Collections.Generic;

namespace RP.Model
{
	public class User : BaseEntity
	{
		public virtual string Email { get; set; }
		public virtual string Password { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual DateTime? DateOfBirth { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Mobile { get; set; }
		public virtual string Salutation { get; set; }
		public virtual bool HasPhoto { get; set; }
		public virtual int? AddressId { get; set; }
		public virtual int? GenderId { get; set; }
		public virtual int LanguageId { get; set; }

		public virtual Address Address { get; set; }
		public virtual DomainValue Gender { get; set; }
		public virtual DomainValue Language { get; set; }

		public virtual ICollection<UserRole> UserRoles { get; set; }
	}
}