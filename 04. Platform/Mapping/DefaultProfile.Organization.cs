using AutoMapper;
using RP.Common.Extension;
using RP.Model;
using RP.Platform.ViewModel;
using System.Web.Mvc;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureOrganization()
		{
			Mapper.CreateMap<OrganizationMemberTypeRef, OrganizationMemberTypeViewModel>();

			Mapper.CreateMap<OrganizationFilterViewModel, OrganizationFilter>();

			Mapper.CreateMap<OrganizationItem, OrganizationItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel()
				.ForMember(dest => dest.Heads, opt => opt.Ignore())
				.ForMember(dest => dest.Judges, opt => opt.Ignore());

			Mapper.CreateMap<OrganizationItems, OrganizationItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<OrganizationDetail, OrganizationDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel();

			Mapper.CreateMap<OrganizationDetails, OrganizationDetailsViewModel>()
				.ForMember(dest => dest.Heads, opt => opt.Ignore())
				.ForMember(dest => dest.Deputies, opt => opt.Ignore())
				.ForMember(dest => dest.MainJudges, opt => opt.Ignore())
				.ForMember(dest => dest.Judges, opt => opt.Ignore())
				.ForMember(dest => dest.Secretaries, opt => opt.Ignore())
				.ForMember(dest => dest.Cashiers, opt => opt.Ignore())
				.ForMember(dest => dest.CashierIds, opt => opt.Ignore())
				.ForMember(dest => dest.CashierItems, opt => opt.Ignore())
				.ForMember(dest => dest.DeputyIds, opt => opt.Ignore())
				.ForMember(dest => dest.DeputyItems, opt => opt.Ignore())
				.ForMember(dest => dest.HeadIds, opt => opt.Ignore())
				.ForMember(dest => dest.HeadItems, opt => opt.Ignore())
				.ForMember(dest => dest.JudgeIds, opt => opt.Ignore())
				.ForMember(dest => dest.JudgeItems, opt => opt.Ignore())
				.ForMember(dest => dest.MainJudgeIds, opt => opt.Ignore())
				.ForMember(dest => dest.MainJudgeItems, opt => opt.Ignore())
				.ForMember(dest => dest.SecretaryIds, opt => opt.Ignore())
				.ForMember(dest => dest.SecretaryItems, opt => opt.Ignore());

			Mapper.CreateMap<OrganizationMemberTypeRef, SelectListItem>()
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.MemberId))
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => Format.FormattedInitials(src.LastName, src.FirstName, src.MiddleName)))
				.ForMember(dest => dest.Disabled, opt => opt.Ignore())
				.ForMember(dest => dest.Group, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore());
		}
	}
}