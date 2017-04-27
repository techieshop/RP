namespace RP.Model
{
	public class DomainValueType : BaseModel
	{
		public virtual int NameCode { get; set; }
		public virtual int? DescriptionCode { get; set; }
	}
}