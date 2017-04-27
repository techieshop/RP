namespace RP.Model
{
	public class Statement : BaseModel
	{
		public virtual int PigeonId { get; set; }
		public virtual int SeasonId { get; set; }

		public virtual Pigeon Pigeon { get; set; }
		public virtual Season Season { get; set; }
	}
}