using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Linq;

namespace RP.Platform.DependencyInjection
{
	public static class DependencyResolverConfig
	{
		public static IUnityContainer Container { get; private set; }

		public static void RegisterType(params ITypeResolver[] typeResolvers)
		{
			Container = BuildUnityContainer(typeResolvers);
		}

		public static IUnityContainer BuildUnityContainer(params ITypeResolver[] typeResolvers)
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();
			if (typeResolvers != null && typeResolvers.Any())
			{
				foreach (var typeResolver in typeResolvers)
				{
					typeResolver.RegisterType(container);
				}
			}
			return container;
		}
	}
}