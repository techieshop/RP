using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureRole()
		{
			Mapper.CreateMap<RoleFilterViewModel, RoleFilter>();

			Mapper.CreateMap<RoleItem, RoleItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel();

			Mapper.CreateMap<RoleItems, RoleItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<RoleDetail, RoleDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel();

			Mapper.CreateMap<RoleDetails, RoleDetailsViewModel>();
		}
	}
}