using Microsoft.Practices.Unity;

namespace RP.Platform.DependencyInjection
{
	public interface ITypeResolver
	{
		void RegisterType(IUnityContainer container);
	}
}