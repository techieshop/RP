using RP.Platform.Binder;
using RP.Platform.Bundling;
using RP.Platform.DependencyInjection;
using RP.Platform.Filtering;
using RP.Platform.Mapping;
using RP.Platform.Provider;
using RP.Platform.Routing;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity.Mvc5;

namespace RP.Platform.App_Start
{
	public static class AppStart
	{
		public static void Initialize()
		{
			DependencyResolverConfig.RegisterType(new WebTypeResolver());

			DependencyResolver.SetResolver(new UnityDependencyResolver(DependencyResolverConfig.Container));

			RouteConfig.RegisterRoute(new DefaultRouteResolver());
			FilterConfig.RegisterFilter(new DefaultFilterResolver());
			BundleConfig.RegisterBundle(new DefaultBundleResolver());
			MappingConfig.RegisterMapping(new DefaultProfile());

			DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

			foreach (ModelValidatorProvider provider in ModelValidatorProviders.Providers)
			{
				if (provider is ClientDataTypeModelValidatorProvider)
				{
					ModelValidatorProviders.Providers.Remove(provider);
					break;
				}
			}

			ModelMetadataProviders.Current = new RPModelMetadataProvider();

			ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
			ModelBinders.Binders.Add(typeof(ICollection<int>), new IntCollectionModelBinder());
			ModelBinders.Binders.Add(typeof(KeyValuePair<,>), new KeyValuePairModelBinder());
		}
	}
}