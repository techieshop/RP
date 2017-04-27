using RP.Model;

namespace RP.DAL.Mapping
{
	public class EntityInfoMap : BaseModelMap<EntityInfo>
	{
		public EntityInfoMap()
		{
			ToTable("EntityInfo", "plt");

			Property(t => t.Guid).IsRequired();
			Property(t => t.CreatedDate).IsRequired();
			Property(t => t.EntityTypeId).IsRequired();

			HasMany(t => t.EntityProgress).WithRequired().HasForeignKey(t => t.EntityInfoId);
			HasMany(t => t.EntityOrganizations).WithRequired().HasForeignKey(t => t.EntityInfoId);
		}
	}
}