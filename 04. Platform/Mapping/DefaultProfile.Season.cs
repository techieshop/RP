using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureSeason()
		{
			Mapper.CreateMap<SeasonFilterViewModel, SeasonFilter>();

			Mapper.CreateMap<SeasonItem, SeasonItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel()
				.ForMember(dest => dest.SeasonTypeName, opt => opt.Ignore());

			Mapper.CreateMap<SeasonItems, SeasonItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<SeasonDetail, SeasonDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel()
				.ForMember(dest => dest.SeasonTypeItems, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonTypeName, opt => opt.Ignore());

			Mapper.CreateMap<SeasonDetails, SeasonDetailsViewModel>()
				.ForMember(dest => dest.StatementItems, opt => opt.Ignore())
				.ForMember(dest => dest.StatementIds, opt => opt.MapFrom(src=>src.StatementPigeonIds));
		}
	}
}
