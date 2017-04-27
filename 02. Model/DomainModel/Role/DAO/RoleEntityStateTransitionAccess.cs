namespace RP.Model
{
	public class RoleEntityStateTransitionAccess : BaseModel
	{
		public virtual int RoleId { get; set; }
		public virtual int EntityTypeId { get; set; }
		public virtual int EntityStateTransitionId { get; set; }
	}
}