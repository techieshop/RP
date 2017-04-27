namespace RP.Model
{
	public class Template : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual int TitleCode { get; set; }
		public virtual int ContentCode { get; set; }
	}
}