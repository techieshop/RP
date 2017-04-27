using Microsoft.Practices.Unity;
using RP.Platform.Context;
using System.Web.Mvc;

namespace RP.Platform.View
{
	public class BaseViewPage<TModel> : WebViewPage<TModel>
	{
		[InjectionMethod]
		public void InitContext(IEntityContext entityContext, IStyleContext styleContext, IUserContext userContext)
		{
			EntityContext = entityContext;
			StyleContext = styleContext;
			UserContext = userContext;
		}

		public IEntityContext EntityContext { get; private set; }

		public IStyleContext StyleContext { get; private set; }

		public IUserContext UserContext { get; private set; }

		public override void Execute()
		{
		}
	}
}