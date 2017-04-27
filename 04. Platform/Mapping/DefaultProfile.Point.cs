using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigurePoint()
		{
			Mapper.CreateMap<PointFilterViewModel, PointFilter>();

			Mapper.CreateMap<PointItem, PointItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel();

			Mapper.CreateMap<PointItems, PointItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<PointDetail, PointDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel();

			Mapper.CreateMap<PointDetails, PointDetailsViewModel>();
		}
	}
}