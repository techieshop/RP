using AutoMapper;
using RP.Common.Extension;
using RP.Model;
using RP.Platform.ViewModel;
using System.Web.Mvc;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureMember()
		{
			Mapper.CreateMap<MemberFilterViewModel, MemberFilter>();

			Mapper.CreateMap<MemberItem, MemberItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel()
				.ForMember(dest => dest.FormattedName, opt => opt.Ignore());

			Mapper.CreateMap<MemberItems, MemberItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<MemberDetail, MemberDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel()
				.ForMember(dest => dest.Gender, opt => opt.Ignore())
				.ForMember(dest => dest.MemberType, opt => opt.Ignore())
				.ForMember(dest => dest.GenderItems, opt => opt.Ignore());

			Mapper.CreateMap<MemberDetails, MemberDetailsViewModel>();

			Mapper.CreateMap<MemberRef, SelectListItem>()
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => Format.FormattedInitials(src.LastName, src.FirstName, src.MiddleName)))
				.ForMember(dest => dest.Disabled, opt => opt.Ignore())
				.ForMember(dest => dest.Group, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore());
		}
	}
}