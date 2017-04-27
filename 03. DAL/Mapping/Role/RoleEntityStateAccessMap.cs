using RP.Model;

namespace RP.DAL.Mapping
{
	public class RoleEntityStateAccessMap : BaseModelMap<RoleEntityStateAccess>
	{
		public RoleEntityStateAccessMap()
		{
			this.ToTable("RoleEntityStateAccess", "acc");

			this.Property(t => t.RoleId).IsRequired();
			this.Property(t => t.EntityTypeId).IsRequired();
			this.Property(t => t.EntityStateId).IsRequired();
			this.Property(t => t.AccessTypeId).IsRequired();
		}
	}
}