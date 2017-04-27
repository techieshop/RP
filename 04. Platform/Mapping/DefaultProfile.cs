using AutoMapper;
using RP.Model;
using System.Web.Mvc;

namespace RP.Platform.Mapping
{
	public partial class DefaultProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<SelectListItemCount, SelectListItem>()
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Count > 0 ? $"{src.Text} ({src.Count})" : src.Text))
				.ForMember(dest => dest.Disabled, opt => opt.Ignore())
				.ForMember(dest => dest.Group, opt => opt.Ignore())
				.ForMember(dest => dest.Selected, opt => opt.Ignore());

			ConfigureAddress();
			ConfigureMember();
			ConfigureOrganization();
			ConfigurePigeon();
			ConfigurePoint();
			ConfigureRace();
			ConfigureRole();
			ConfigureSeason();
			ConfigureUser();
		}
	}
}