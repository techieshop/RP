using RP.Model;

namespace RP.Platform.Context
{
	public interface IStyleContext
	{
		// Translation
		DomainValue Language { get; }

		string GetTranslation(int? id, params object[] args);
	}
}