using RP.Model;

namespace RP.DAL.Mapping
{
	public class RoleEntityStateTransitionAccessMap : BaseModelMap<RoleEntityStateTransitionAccess>
	{
		public RoleEntityStateTransitionAccessMap()
		{
			ToTable("RoleEntityStateTransitionAccess", "acc");

			Property(t => t.RoleId).IsRequired();
			Property(t => t.EntityTypeId).IsRequired();
			Property(t => t.EntityStateTransitionId).IsRequired();
		}
	}
}