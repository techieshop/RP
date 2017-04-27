using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class AddressViewModel : BaseViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Address.City)]
		[RPMaxLength(128)]
		public string City { get; set; }

		public int CountryId { get; set; }

		public string CountryName { get; set; }

		public string FormattedAddress { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Address.Latitude)]
		public string Latitude { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Address.Longitude)]
		public string Longitude { get; set; }

		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Address.Number)]
		public string Number { get; set; }

		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Address.PostalCode)]
		public string PostalCode { get; set; }

		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Address.Singular, PlaceholderCode = Dom.Translation.Address.EnterAddress)]
		public string Search { get; set; }

		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Address.Street)]
		public string Street { get; set; }
	}
}