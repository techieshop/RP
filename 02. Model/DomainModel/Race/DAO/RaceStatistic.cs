namespace RP.Model
{
	public class RaceStatistic : BaseModel
	{
		public virtual double Mark { get; set; }
		public virtual Member Member { get; set; }
		public virtual int MemberId { get; set; }
		public virtual int PrizePigeonCount { get; set; }
		public virtual Race Race { get; set; }
		public virtual int RaceId { get; set; }
		public virtual int StatedPigeonCount { get; set; }
		public virtual double Success { get; set; }
	}
}