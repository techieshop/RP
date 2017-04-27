using System;

namespace RP.Model
{
	public class ResultTimeItem : BaseModel
	{
		public bool Ac { get; set; }
		public double Coefficient { get; set; }
		public double Distance { get; set; }
		public string FirstName { get; set; }
		public double FlyTime { get; set; }
		public TimeSpan FlyTimeSpan { get; set; }
		public string LastName { get; set; }
		public double Mark { get; set; }
		public string Member { get; set; }
		public string MiddleName { get; set; }
		public string OrganizationName { get; set; }
		public int Position { get; set; }
		public DateTime ReturnTime { get; set; }
		public string Sex { get; set; }
		public int SexId { get; set; }
		public double Speed { get; set; }
	}
}