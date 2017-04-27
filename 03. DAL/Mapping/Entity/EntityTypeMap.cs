using RP.Model;

namespace RP.DAL.Mapping
{
	public class EntityTypeMap : BaseModelMap<EntityType>
	{
		public EntityTypeMap()
		{
			ToTable("EntityType", "plt");

			Property(t => t.DescriptionCode).IsOptional();
			Property(t => t.Name).HasMaxLength(64).IsRequired();

			HasMany(t => t.EntityStates).WithRequired().HasForeignKey(t => t.EntityTypeId);
		}
	}
}