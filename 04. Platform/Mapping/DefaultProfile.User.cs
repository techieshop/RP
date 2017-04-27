using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile
	{
		private void ConfigureUser()
		{
			Mapper.CreateMap<RegistrationViewModel, Registration>()
				.ForMember(dest => dest.DateTimeRequest, opt => opt.Ignore())
				.ForMember(dest => dest.IsRead, opt => opt.Ignore());

			Mapper.CreateMap<UserFilterViewModel, UserFilter>();

			Mapper.CreateMap<UserItem, UserItemViewModel>()
				.ApplyBaseItemToBaseItemViewModel();

			Mapper.CreateMap<UserItems, UserItemsViewModel>()
				.ForMember(dest => dest.Filter, opt => opt.Ignore());

			Mapper.CreateMap<UserDetail, UserDetailViewModel>()
				.ApplyBaseDetailToBaseDetailViewModel()
				.ForMember(dest => dest.FormattedName, opt => opt.Ignore())
				.ForMember(dest => dest.Gender, opt => opt.Ignore())
				.ForMember(dest => dest.GenderItems, opt => opt.Ignore())
				.ForMember(dest => dest.Language, opt => opt.Ignore())
				.ForMember(dest => dest.LanguageItems, opt => opt.Ignore())
				.ForMember(dest => dest.OrganizationItems, opt => opt.Ignore())
				.ForMember(dest => dest.RoleIds, opt => opt.Ignore())
				.ForMember(dest => dest.RoleItems, opt => opt.Ignore())
				.ForMember(dest => dest.RoleOrganizationId, opt => opt.Ignore());

			Mapper.CreateMap<UserDetails, UserDetailsViewModel>();

			Mapper.CreateMap<UserProfile, UserProfileViewModel>()
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
				.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Profile.DateOfBirth))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Profile.Email))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Profile.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Profile.LastName))
				.ForMember(dest => dest.FormattedName, opt => opt.Ignore())
				.ForMember(dest => dest.Gender, opt => opt.Ignore())
				.ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.Profile.GenderId))
				.ForMember(dest => dest.GenderItems, opt => opt.Ignore())
				.ForMember(dest => dest.Language, opt => opt.Ignore())
				.ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.Profile.LanguageId))
				.ForMember(dest => dest.LanguageItems, opt => opt.Ignore())
				.ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Profile.MiddleName))
				.ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Profile.Mobile))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Profile.Password))
				.ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Profile.Phone))
				.ForMember(dest => dest.Salutation, opt => opt.MapFrom(src => src.Profile.Salutation))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Profile.Id));
		}
	}
}