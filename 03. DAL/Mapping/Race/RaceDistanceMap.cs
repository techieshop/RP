using RP.Model;

namespace RP.DAL.Mapping
{
	public class RaceDistanceMap : BaseModelMap<RaceDistance>
	{
		public RaceDistanceMap()
		{
			ToTable("RaceDistance", "race");

			Property(x => x.RaceId).IsRequired();
			Property(x => x.MemberId).IsRequired();
			Property(x => x.Distance).IsRequired();

			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
			HasRequired(t => t.Member).WithMany().HasForeignKey(t => t.MemberId);
		}
	}
}