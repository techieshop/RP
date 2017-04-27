using RP.Model;

namespace RP.DAL.Mapping
{
	public class RegistrationMap : BaseModelMap<Registration>
	{
		public RegistrationMap()
		{
			ToTable("Registration", "acc");
			Property(t => t.DateTimeRequest).IsRequired();
			Property(t => t.IsRead).IsRequired();
			Property(t => t.OrganizationName).HasMaxLength(256).IsRequired();
			Property(t => t.CandidateInfo).HasMaxLength(2048).IsRequired();
			Property(t => t.HeadInfo).HasMaxLength(2048).IsRequired();
		}
	}
}