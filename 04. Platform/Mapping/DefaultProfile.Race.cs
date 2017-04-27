using System;
using AutoMapper;
using RP.Model;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System.Collections.Generic;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureRace()
		{
			Mapper.CreateMap<RaceFilterViewModel, RaceFilter>();

			Mapper.CreateMap<RaceItem, RaceItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel()
				.ForMember(dest => dest.RaceTypeName, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonName, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonTypeName, opt => opt.Ignore());

			Mapper.CreateMap<RaceItems, RaceItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<RaceDetail, RaceDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel()
				.ForMember(dest => dest.PointItems, opt => opt.Ignore())
				.ForMember(dest => dest.RaceTypeItems, opt => opt.Ignore())
				.ForMember(dest => dest.RaceTypeName, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonItems, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonName, opt => opt.Ignore())
				.ForMember(dest => dest.SeasonTypeName, opt => opt.Ignore());

			Mapper.CreateMap<RaceDetails, RaceDetailsViewModel>()
				.ForMember(dest => dest.StatementItems, opt => opt.Ignore())
				.ForMember(dest => dest.StatementIds, opt => opt.MapFrom(src => src.StatementPigeonIds))
				.ForMember(dest => dest.CanCalculateResults, opt => opt.Ignore());

			Mapper.CreateMap<ResultDetail, ResultDetailViewModel>()
				.ForMember(dest => dest.DurationOfCompetitionTimeSpan, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationOfCompetition)));

			Mapper.CreateMap<RaceResultReturnTimes, RaceResultReturnTimesViewModel>()
				.ForMember(dest => dest.PigeonReturnItems, opt => opt.Ignore())
				.ForMember(dest => dest.PigeonReturnTimesJson, opt => opt.MapFrom(src => JsonHelper.JsonSerializer<ICollection<PigeonReturnTimeRef>>(src.PigeonReturnTimes)));
		}
	}
}