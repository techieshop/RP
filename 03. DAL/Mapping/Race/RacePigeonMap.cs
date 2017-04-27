using RP.Model;

namespace RP.DAL.Mapping
{
	public class RacePigeonMap : BaseModelMap<RacePigeon>
	{
		public RacePigeonMap()
		{
			ToTable("RacePigeon", "race");

			Property(x => x.RaceId).IsRequired();
			Property(x => x.PigeonId).IsRequired();

			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
			HasRequired(t => t.Pigeon).WithMany().HasForeignKey(t => t.PigeonId);
		}
	}
}