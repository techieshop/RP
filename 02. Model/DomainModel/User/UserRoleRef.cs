namespace RP.Model
{
	public class UserRoleRef : BaseModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Name { get; set; }
		public int OrganizationId { get; set; }
		public int RoleId { get; set; }
		public int UserId { get; set; }
	}
}