namespace RP.Model
{
	public class Address : BaseModel
	{
		public virtual string City { get; set; }
		public virtual Country Country { get; set; }
		public virtual int CountryId { get; set; }
		public virtual string FormattedAddress { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
		public virtual string Number { get; set; }
		public virtual string PostalCode { get; set; }
		public virtual string Street { get; set; }

		public static Address Empty()
		{
			return new Address();
		}
	}
}