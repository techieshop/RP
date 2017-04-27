using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.Platform.Context
{
	public partial class StyleContext
	{
		public DomainValue Language { get; private set; }

		public IDictionary<int, string> Translations { get; private set; }

		public string GetTranslation(int? id, params object[] args)
		{
			string translation = string.Empty;
			int translationId = id.GetValueOrDefault();
			if (Translations != null && Translations.Any())
			{
				if (Translations.ContainsKey(translationId))
				{
					if (args != null && args.Length > 0)
					{
						translation = string.Format(Translations[translationId], args);
					}
					else
					{
						translation = Translations[translationId];
					}
				}
			}
			return translation;
		}

		private void InitTranslation()
		{
			Language = new DomainValue { Id = UserContext.Current.User!=null ? UserContext.Current.User.LanguageId : Dom.Language.System};
			Translations = _translationRepository.GetTranslations(
				Language.Id
			);
		}
	}
}