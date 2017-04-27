namespace RP.Model
{
	public class RoleEntityStateAccess : BaseModel
	{
		public virtual int RoleId { get; set; }
		public virtual int EntityTypeId { get; set; }
		public virtual int EntityStateId { get; set; }
		public virtual int AccessTypeId { get; set; }
	}
}