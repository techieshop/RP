using RP.Model;

namespace RP.DAL.Mapping
{
	public class DomainValueMap : BaseModelMap<DomainValue>
	{
		public DomainValueMap()
		{
			ToTable("DomainValue", "dom");

			Property(t => t.NameCode).IsRequired();
			Property(t => t.DescriptionCode).IsOptional();
			Property(t => t.Icon).HasMaxLength(256).IsOptional();
			Property(t => t.Code).HasMaxLength(2).IsOptional();
			Property(t => t.DomainValueTypeId).IsRequired();

			HasRequired(t => t.DomainValueType).WithMany().HasForeignKey(t => t.DomainValueTypeId);
		}
	}
}