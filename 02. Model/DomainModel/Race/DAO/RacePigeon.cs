namespace RP.Model
{
	public class RacePigeon : BaseModel
	{
		public virtual int RaceId { get; set; }
		public virtual int PigeonId { get; set; }

		public virtual Race Race { get; set; }
		public virtual Pigeon Pigeon { get; set; }
	}
}