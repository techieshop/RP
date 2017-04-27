using RP.Model;

namespace RP.DAL.Mapping
{
	public class DomainValueTypeMap : BaseModelMap<DomainValueType>
	{
		public DomainValueTypeMap()
		{
			ToTable("DomainValueType", "dom");

			Property(t => t.NameCode).IsRequired();
			Property(t => t.DescriptionCode).IsOptional();
		}
	}
}