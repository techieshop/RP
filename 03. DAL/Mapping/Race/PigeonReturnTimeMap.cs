using RP.Model;

namespace RP.DAL.Mapping
{
	public class PigeonReturnTimeMap : BaseModelMap<PigeonReturnTime>
	{
		public PigeonReturnTimeMap()
		{
			ToTable("PigeonReturnTime", "race");

			Property(x => x.RaceId).IsRequired();
			Property(x => x.PigeonId).IsRequired();
			Property(x => x.ReturnTime).IsRequired();

			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
			HasRequired(t => t.Pigeon).WithMany().HasForeignKey(t => t.PigeonId);
		}
	}
}