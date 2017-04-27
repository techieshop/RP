using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public class TranslationRepository : BaseRepository<EntityDbContext>, ITranslationRepository
	{
		public TranslationRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public IDictionary<int, string> GetTranslations(int languageId)
		{
			var parameters = new
			{
				LanguageId = languageId
			};
			return ExecuteSp<TranslationRef>("[stl].[spGetTranslations]", parameters)
				.ToDictionary(t => t.TranslationCodeId, t => t.Value);
		}
	}
}