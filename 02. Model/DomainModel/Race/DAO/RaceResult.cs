using System.Collections.Generic;

namespace RP.Model
{
	public class RaceResult : BaseModel
	{
		public virtual bool Ac { get; set; }
		public virtual double Distance { get; set; }
		public virtual double FlyTime { get; set; }
		public virtual Pigeon Pigeon { get; set; }
		public virtual int PigeonId { get; set; }
		public virtual int Position { get; set; }
		public virtual Race Race { get; set; }
		public virtual int RaceId { get; set; }
		public virtual double Speed { get; set; }

		public virtual ICollection<RaceResultCategory> RaceResultCategories { get; set; }
	}
}