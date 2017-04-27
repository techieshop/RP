using System;
using System.Collections.Generic;

namespace RP.Model
{
	public class Organization : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual DateTime? CreateDate { get; set; }
		public virtual int? AddressId { get; set; }
		public virtual int? OrganizationTypeId { get; set; }
		public virtual int? WebsiteId { get; set; }

		public virtual Address Address { get; set; }
		public virtual Website Website { get; set; }

		public virtual ICollection<OrganizationMemberType> OrganizationMemberTypes { get; set; }
		public virtual ICollection<OrganizationRelation> OrganizationRelations { get; set; }

		public static Organization Empty(string name = "", int organizationTypeId = Dom.OrganizationType.Club)
		{
			return new Organization
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Organization.Id),
				OrganizationRelations = new List<OrganizationRelation>(),
				Name = name,
				OrganizationTypeId = organizationTypeId
			};
		}
	}
}