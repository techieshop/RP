namespace RP.Model
{
	public class RaceDistance : BaseModel
	{
		public virtual int RaceId { get; set; }
		public virtual int MemberId { get; set; }
		public virtual double Distance { get; set; }

		public virtual Race Race { get; set; }
		public virtual Member Member { get; set; }
	}
}