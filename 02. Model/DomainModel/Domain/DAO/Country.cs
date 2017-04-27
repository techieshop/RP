namespace RP.Model
{
	public class Country : BaseModel
	{
		public virtual string Code { get; set; }
		public virtual int NameCode { get; set; }
		public virtual string PhoneCode { get; set; }
		public virtual string Icon { get; set; }
	}
}