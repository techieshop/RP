using AutoMapper;
using RP.Model;
using RP.Platform.ViewModel;

namespace RP.Platform.Mapping
{
	public static class MappingExtensions
	{
		public static IMappingExpression<TSrc, TDest> IgnoreAllMembers<TSrc, TDest>(this IMappingExpression<TSrc, TDest> mapping)
			where TSrc : class
			where TDest : class
		{
			mapping
				.ForAllMembers(opt => opt.Ignore());

			return mapping;
		}

		public static IMappingExpression<TSrc, TDest> ApplyBaseItemToBaseItemViewModel<TSrc, TDest>(this IMappingExpression<TSrc, TDest> mapping)
			where TSrc : BaseItem
			where TDest : BaseItemViewModel
		{
			mapping
				.ForMember(dest => dest.EntityStateNameCode, opt => opt.Ignore());

			return mapping;
		}

		public static IMappingExpression<TSrc, TDest> ApplyBaseDetailToBaseDetailViewModel<TSrc, TDest>(this IMappingExpression<TSrc, TDest> mapping)
			where TSrc : BaseDetail
			where TDest : BaseDetailViewModel
		{
			mapping
				.ForMember(dest => dest.EntityStateChange, opt => opt.Ignore())
				.ForMember(dest => dest.EntityStateNameCode, opt => opt.Ignore())
				.ForMember(dest => dest.HasEntityStateChangeAccess, opt => opt.Ignore());

			return mapping;
		}
	}
}