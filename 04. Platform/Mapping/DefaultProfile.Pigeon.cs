using AutoMapper;
using RP.Common.Extension;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigurePigeon()
		{
			Mapper.CreateMap<PigeonFilterViewModel, PigeonFilter>()
				.ForMember(dest => dest.SexIds, opt => opt.Ignore());

			Mapper.CreateMap<PigeonItem, PigeonItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel()
				.ForMember(dest => dest.Sex, opt => opt.Ignore())
				.ForMember(dest => dest.OwnerFormattedName, opt => opt.MapFrom(src => Format.FormattedInitials(src.OwnerLastName, src.OwnerFirstName, src.OwnerMiddleName)))
				.ForMember(dest => dest.Ring, opt => opt.MapFrom(src => Format.FormattedRing(src.Year, src.Code, src.Number)));

			Mapper.CreateMap<PigeonItems, PigeonItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<PigeonDetail, PigeonDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel()
				.ForMember(dest => dest.Sex, opt => opt.Ignore())
				.ForMember(dest => dest.SexItems, opt => opt.Ignore())
				.ForMember(dest => dest.OwnerFormattedName, opt => opt.MapFrom(src => Format.FormattedInitials(src.OwnerLastName, src.OwnerFirstName, src.OwnerMiddleName)))
				.ForMember(dest => dest.Ring, opt => opt.MapFrom(src => Format.FormattedRing(src.Year, src.Code, src.Number)));

			Mapper.CreateMap<PigeonDetails, PigeonDetailsViewModel>();
		}
	}
}