namespace RP.Model
{
	public class CommonRace : BaseModel
	{
		public virtual int CommonRaceId { get; set; }
		public virtual int RaceId { get; set; }

		public virtual Race CommonnRace { get; set; }
		public virtual Race Race { get; set; }
	}
}