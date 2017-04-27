using RP.Model;

namespace RP.DAL.Mapping
{
	public class RaceResultCategoryMap : BaseModelMap<RaceResultCategory>
	{
		public RaceResultCategoryMap()
		{
			ToTable("RaceResultCategory", "race");

			Property(x => x.RaceResultId).IsRequired();
			Property(x => x.CategoryId).IsRequired();
			Property(x => x.Coefficient).IsRequired();
			Property(x => x.Mark).IsRequired();
			Property(x => x.IsOlymp).IsRequired();

			HasRequired(t => t.RaceResult).WithMany().HasForeignKey(t => t.RaceResultId);
			HasRequired(t => t.Category).WithMany().HasForeignKey(t => t.CategoryId);
		}
	}
}