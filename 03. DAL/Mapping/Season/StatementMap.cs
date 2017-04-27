using RP.Model;

namespace RP.DAL.Mapping
{
	public class StatementMap : BaseModelMap<Statement>
	{
		public StatementMap()
		{
			ToTable("Statement", "race");

			Property(x => x.PigeonId).IsRequired();
			Property(x => x.SeasonId).IsRequired();

			HasRequired(t => t.Pigeon).WithMany().HasForeignKey(t => t.PigeonId);
			HasRequired(t => t.Season).WithMany().HasForeignKey(t => t.SeasonId);
		}
	}
}