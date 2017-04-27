using RP.DAL.Repository;
using System.Web.Mvc;

namespace RP.Platform.Context
{
	public partial class StyleContext : IStyleContext
	{
		private readonly ITranslationRepository _translationRepository;

		public StyleContext(ITranslationRepository translationRepository)
		{
			_translationRepository = translationRepository;

			InitTranslation();
		}

		public static IStyleContext Current => DependencyResolver.Current.GetService<IStyleContext>();
	}
}