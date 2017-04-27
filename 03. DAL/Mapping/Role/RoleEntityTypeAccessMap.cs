using RP.Model;

namespace RP.DAL.Mapping
{
	public class RoleEntityTypeAccessMap : BaseModelMap<RoleEntityTypeAccess>
	{
		public RoleEntityTypeAccessMap()
		{
			ToTable("RoleEntityTypeAccess", "acc");

			Property(t => t.RoleId).IsRequired();
			Property(t => t.EntityTypeId).IsRequired();
			Property(t => t.AccessTypeId).IsRequired();
		}
	}
}