namespace RP.Model
{
	public class RaceResultCategory : BaseModel
	{
		public virtual int RaceResultId { get; set; }
		public virtual int CategoryId { get; set; }
		public virtual double Coefficient { get; set; }
		public virtual double Mark { get; set; }
		public virtual bool IsOlymp { get; set; }

		public virtual RaceResult RaceResult { get; set; }
		public virtual DomainValue Category { get; set; }
	}
}