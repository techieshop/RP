using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;

namespace RP.Platform.App_Start
{
	public static class UnityConfig
	{
		public static void RegisterComponents()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}