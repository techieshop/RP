namespace RP.Model
{
	public class UserRole : BaseModel
	{
		public virtual int UserId { get; set; }
		public virtual int RoleId { get; set; }
		public virtual int OrganizationId { get; set; }

		public virtual User User { get; set; }
		public virtual Role Role { get; set; }
		public virtual Organization Organization { get; set; }
	}
}