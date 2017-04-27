using System.Collections.Generic;

namespace RP.Model
{
	public class TranslationCode : BaseModel
	{
		public virtual string Name { get; set; }

		public virtual ICollection<Translation> Translations { get; set; }

		public static TranslationCode Empty(string name)
		{
			return new TranslationCode
			{
				Name = name,
				Translations = new List<Translation>()
			};
		}
	}
}