using AutoMapper;
using System.Linq;

namespace RP.Platform.Mapping
{
	public static class MappingConfig
	{
		public static void RegisterMapping(params Profile[] profiles)
		{
			if (profiles != null && profiles.Any())
			{
				Mapper.Initialize(config =>
				{
					foreach (var profile in profiles)
						config.AddProfile(profile);
				});
				Mapper.AssertConfigurationIsValid();
			}
		}
	}
}