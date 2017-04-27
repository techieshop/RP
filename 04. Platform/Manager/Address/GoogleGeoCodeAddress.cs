using Newtonsoft.Json;
using RP.DAL.Repository;
using RP.Model;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace RP.Platform.Manager
{
	public class GoogleGeoCodeAddress
	{
		private readonly IDomainValueRepository _domainValueRepository;

		public GoogleGeoCodeAddress(IDomainValueRepository domainValueRepository)
		{
			_domainValueRepository = domainValueRepository;
		}

		public const string StreetNumber = "street_number";
		public const string Street = "route";
		public const string City = "locality";
		public const string Country = "country";
		public const string PostalCode = "postal_code";
		public const string Latitude = "lat";
		public const string Longitude = "lng";
		public const string FormattedAddress = "formatted_address";

		public class GoogleGeoCodeAddressResponse
		{
			[JsonProperty("status")]
			public string Status { get; set; }

			[JsonProperty("results")]
			public Results[] Results { get; set; }
		}

		public class Results
		{
			[JsonProperty("formatted_address")]
			public string FormattedAddress { get; set; }

			[JsonProperty("geometry")]
			public Geometry Geometry { get; set; }

			[JsonProperty("types")]
			public string[] Types { get; set; }

			[JsonProperty("address_components")]
			public AddressComponent[] AddressComponents { get; set; }

			[JsonProperty("place_id")]
			public string PlaceId { get; set; }
		}

		public class Geometry
		{
			[JsonProperty("location_type")]
			public string LocationType { get; set; }

			[JsonProperty("location")]
			public Location Location { get; set; }
		}

		public class Location
		{
			[JsonProperty("lat")]
			public string Lat { get; set; }

			[JsonProperty("lng")]
			public string Lng { get; set; }
		}

		public class AddressComponent
		{
			[JsonProperty("long_name")]
			public string LongName { get; set; }

			[JsonProperty("short_name")]
			public string ShortName { get; set; }

			[JsonProperty("types")]
			public string[] Types { get; set; }
		}

		private GoogleGeoCodeAddressResponse GetGoogleGeoCodeAddressResponse(string address, string countryIso2Code = "ua", string language = "uk")
		{
			var addressApiUrl = $"http://maps.google.com/maps/api/geocode/json?address={address.Replace(" ", "+")}&country={countryIso2Code}&language={language}&sensor=false";
			WebClient wc = new WebClient {Encoding = Encoding.UTF8};
			var result = wc.DownloadString(addressApiUrl);
			return JsonConvert.DeserializeObject<GoogleGeoCodeAddressResponse>(result);
		}

		public Address GetAddress(string address, string countryIso2Code = "ua", string language = "uk")
		{
			var googleAddress = GetGoogleGeoCodeAddressResponse(address, countryIso2Code, language);
			Address result = Address.Empty();

			if (googleAddress.Status.ToUpper() == "OK")
			{
				var addrComponents = googleAddress.Results[0].AddressComponents;
				var addressComponentCountry = addrComponents.FirstOrDefault(addr => addr.Types.Contains(Country));

				if (addressComponentCountry != null)
				{
					var country = _domainValueRepository.GetCountry(addressComponentCountry.ShortName);
					if (country != null)
					{
						result.CountryId = country.Id;
					}
				}

				var city = addrComponents.FirstOrDefault(addr => addr.Types.Contains(City));
				if (city != null)
					result.City = city.ShortName;

				var street = addrComponents.FirstOrDefault(addr => addr.Types.Contains(Street));
				if (street != null)
					result.Street = street.ShortName;

				var number = addrComponents.FirstOrDefault(addr => addr.Types.Contains(StreetNumber));
				if (number != null)
					result.Number = number.ShortName;

				var postalCode = addrComponents.FirstOrDefault(addr => addr.Types.Contains(PostalCode));
				if (postalCode != null)
					result.PostalCode = postalCode.ShortName;

				var geometry = googleAddress.Results[0].Geometry;

				var latitude = geometry.Location.Lat;

				if (latitude != null)
				{
					double lat;
					double.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
					result.Latitude = lat;
				}

				var longitude = geometry.Location.Lng;

				if (longitude != null)
				{
					double lng;
					double.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lng);
					result.Longitude = lng;
				}

				var formattedAddress = googleAddress.Results[0].FormattedAddress;
				result.FormattedAddress = formattedAddress;
			}

			return result;
		}

		public Address GetAddress(string street, string number, string postalCode, string city, string countryIso2Code = "ua", string language = "uk")
		{
			StringBuilder fullAddress = new StringBuilder();
			if (!string.IsNullOrEmpty(street))
				fullAddress.Append(street);
			if (!string.IsNullOrEmpty(number))
			{
				if (!string.IsNullOrEmpty(street))
					fullAddress.Append(",");
				fullAddress.Append(number);
			}

			if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(number))
				fullAddress.Append(",");

			if (!string.IsNullOrEmpty(postalCode))
				fullAddress.Append(postalCode);
			if (!string.IsNullOrEmpty(city))
			{
				if (!string.IsNullOrEmpty(postalCode))
					fullAddress.Append(",");
				fullAddress.Append(city);
			}

			return GetAddress(fullAddress.ToString(), countryIso2Code, language);
		}
	}
}