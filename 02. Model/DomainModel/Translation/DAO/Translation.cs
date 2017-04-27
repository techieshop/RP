namespace RP.Model
{
	public class Translation : BaseModel
	{
		public virtual int LanguageId { get; set; }
		public virtual int TranslationCodeId { get; set; }
		public virtual string Value { get; set; }

		public virtual DomainValue Language { get; set; }
		public virtual TranslationCode TranslationCode { get; set; }
	}
}