using System;
using System.Collections.Generic;

namespace RP.Model
{
	public class Member : BaseEntity
	{
		public virtual string Email { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string MiddleName { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Mobile { get; set; }
		public virtual DateTime? DateOfBirth { get; set; }
		public virtual int? AddressId { get; set; }
		public virtual int? GenderId { get; set; }
		public virtual int? WebsiteId { get; set; }

		public virtual Address Address { get; set; }
		public virtual DomainValue Gender { get; set; }
		public virtual Website Website { get; set; }

		public virtual ICollection<OrganizationMemberType> OrganizationMemberTypes { get; set; }
		public virtual ICollection<Pigeon> Pigeons { get; set; }
		public virtual ICollection<RaceDistance> RaceDistances { get; set; }
		public virtual ICollection<RaceStatistic> RaceStatistics { get; set; }

		public static Member Empty()
		{
			return new Member
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Member.Id)
			};
		}
	}
}