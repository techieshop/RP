namespace RP.Model
{
	public class EntityStateTransition : BaseModel
	{
		public virtual int? ActionBeforeCode { get; set; }
		public virtual int ActionAfterCode { get; set; }
		public virtual byte Order { get; set; }
		public virtual int? EntityStateFromId { get; set; }
		public virtual int EntityStateToId { get; set; }

		public virtual EntityState EntityStateFrom { get; set; }
		public virtual EntityState EntityStateTo { get; set; }
	}
}