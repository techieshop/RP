using RP.Model;

namespace RP.DAL.Mapping
{
	public class EntityStateTransitionMap : BaseModelMap<EntityStateTransition>
	{
		public EntityStateTransitionMap()
		{
			ToTable("EntityStateTransition", "plt");

			Property(t => t.ActionBeforeCode).IsOptional();
			Property(t => t.ActionAfterCode).IsRequired();
			Property(t => t.Order).IsRequired();
			Property(t => t.EntityStateFromId).IsOptional();
			Property(t => t.EntityStateToId).IsRequired();

			HasOptional(t => t.EntityStateFrom).WithMany().HasForeignKey(t => t.EntityStateFromId);
			HasRequired(t => t.EntityStateTo).WithMany().HasForeignKey(t => t.EntityStateToId);
		}
	}
}