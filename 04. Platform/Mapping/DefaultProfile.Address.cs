using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureAddress()
		{
			Mapper.CreateMap<Address, AddressViewModel>()
				.ForMember(dest => dest.CountryName, opt => opt.Ignore())
				.ForMember(dest => dest.Search, opt => opt.Ignore());

			Mapper.CreateMap<AddressRef, AddressViewModel>()
				.ForMember(dest => dest.CountryName, opt => opt.Ignore())
				.ForMember(dest => dest.Search, opt => opt.Ignore())
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(dest => dest.Latitude.ToString().Replace(",",".")))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(dest => dest.Longitude.ToString().Replace(",", ".")));

			Mapper.CreateMap<AddressViewModel, Address>()
				.ForMember(dest => dest.Country, opt => opt.Ignore())
				.ForMember(dest => dest.Latitude, opt => opt.MapFrom(dest => double.Parse(dest.Latitude)))
				.ForMember(dest => dest.Longitude, opt => opt.MapFrom(dest => double.Parse(dest.Longitude)));

			Mapper.CreateMap<Website, WebsiteViewModel>();

			Mapper.CreateMap<WebsiteRef, WebsiteViewModel>();
		}
	}
}