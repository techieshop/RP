using RP.Model;

namespace RP.DAL.Mapping
{
	public class PigeonMap : BaseEntityMap<Pigeon>
	{
		public PigeonMap()
		{
			ToTable("Pigeon", "acc");

			Property(x => x.Name).HasMaxLength(32).IsOptional();
			Property(x => x.Year).IsRequired();
			Property(x => x.Code).HasMaxLength(16).IsOptional();
			Property(x => x.Number).HasMaxLength(16).IsRequired();
			Property(x => x.SexId).IsOptional();
			Property(x => x.MemberId).IsRequired();

			HasRequired(t => t.Member).WithMany().HasForeignKey(t => t.MemberId);
			HasRequired(t => t.Sex).WithMany().HasForeignKey(t => t.SexId);

			HasMany(t => t.PigeonReturnTimes).WithRequired().HasForeignKey(t => t.PigeonId);
			HasMany(t => t.RacePigeons).WithRequired().HasForeignKey(t => t.PigeonId);
			HasMany(t => t.RaceResults).WithRequired().HasForeignKey(t => t.PigeonId);
			HasMany(t => t.Statements).WithRequired().HasForeignKey(t => t.PigeonId);
		}
	}
}