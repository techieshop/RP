using RP.Model;

namespace RP.DAL.Mapping
{
	public class CommonRaceMap : BaseModelMap<CommonRace>
	{
		public CommonRaceMap()
		{
			ToTable("CommonRace", "race");

			Property(x => x.CommonRaceId).IsRequired();
			Property(x => x.RaceId).IsRequired();

			HasRequired(t => t.CommonnRace).WithMany().HasForeignKey(t => t.CommonRaceId);
			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
		}
	}
}