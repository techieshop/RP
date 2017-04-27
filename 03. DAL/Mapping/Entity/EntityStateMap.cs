using RP.Model;

namespace RP.DAL.Mapping
{
	public class EntityStateMap : BaseModelMap<EntityState>
	{
		public EntityStateMap()
		{
			ToTable("EntityState", "plt");

			Property(t => t.NameCode).IsRequired();
			Property(t => t.DescriptionCode).IsOptional();
			Property(t => t.Order).IsRequired();
			Property(t => t.IsStart).IsRequired();
			Property(t => t.IsFinish).IsRequired();
			Property(t => t.IsActive).IsRequired();
			Property(t => t.EntityTypeId).IsRequired();

			HasMany(t => t.TransitionsFrom).WithOptional().HasForeignKey(t => t.EntityStateFromId);
			HasMany(t => t.TransitionsTo).WithRequired().HasForeignKey(t => t.EntityStateToId);
		}
	}
}