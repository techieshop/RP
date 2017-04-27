namespace RP.Model
{
	public class DomainValue : BaseModel
	{
		public virtual int NameCode { get; set; }
		public virtual int? DescriptionCode { get; set; }
		public virtual string Icon { get; set; }
		public virtual string Code { get; set; }
		public virtual int DomainValueTypeId { get; set; }

		public virtual DomainValueType DomainValueType { get; set; }
	}
}