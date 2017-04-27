namespace RP.Model
{
	public class BaseEntity : BaseModel
	{
		public virtual int EntityInfoId { get; set; }
		public virtual EntityInfo EntityInfo { get; set; }
	}
}