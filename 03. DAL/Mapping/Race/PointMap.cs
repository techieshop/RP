using RP.Model;

namespace RP.DAL.Mapping
{
	public class PointMap : BaseEntityMap<Point>
	{
		public PointMap()
		{
			ToTable("Point", "race");

			Property(x => x.Name).HasMaxLength(128).IsRequired();
			Property(x => x.AddressId).IsRequired();

			HasRequired(t => t.Address).WithMany().HasForeignKey(t => t.AddressId);
			HasMany(t => t.Races).WithRequired().HasForeignKey(t => t.PointId);
		}
	}
}