using System;

namespace RP.Model
{
	public class PigeonReturnTime : BaseModel
	{
		public virtual int RaceId { get; set; }
		public virtual int PigeonId { get; set; }
		public virtual DateTime ReturnTime { get; set; }

		public virtual Race Race { get; set; }
		public virtual Pigeon Pigeon { get; set; }
	}
}