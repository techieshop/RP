namespace RP.Model
{
	public class AddressRef : BaseModel
	{
		public string City { get; set; }
		public int CountryId { get; set; }
		public string FormattedAddress { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public string Number { get; set; }
		public string PostalCode { get; set; }
		public string Street { get; set; }
	}
}