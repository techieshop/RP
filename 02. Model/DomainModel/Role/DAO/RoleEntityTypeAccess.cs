namespace RP.Model
{
	public class RoleEntityTypeAccess : BaseModel
	{
		public virtual int RoleId { get; set; }
		public virtual int EntityTypeId { get; set; }
		public virtual int AccessTypeId { get; set; }
	}
}