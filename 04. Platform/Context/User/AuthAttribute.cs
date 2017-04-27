using RP.Model;
using RP.Platform.Controller;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Context
{
	public class AuthAttribute : AuthorizeAttribute
	{
		public int[] Create { get; set; }
		public int[] Read { get; set; }
		public int[] Write { get; set; }

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			BaseController controller = filterContext.Controller as BaseController;

			if (!controller.UserContext.IsAuthenticated)
			{
				filterContext.Result = controller.LoginWithReturnResult();
			}
			else
			{
				if (Read != null && Read.Any())
				{
					for (int i = 0; i < Read.Length; i++)
					{
						if (!controller.UserContext.HasAccess(Read[i]))
						{
							filterContext.Result = controller.ForbiddenResult();
							break;
						}
					}
				}
				if (Write != null && Write.Any())
				{
					for (int i = 0; i < Write.Length; i++)
					{
						if (!controller.UserContext.HasAccess(Write[i], Dom.AccessType.ReadWrite))
						{
							filterContext.Result = controller.ForbiddenResult();
							break;
						}
					}
				}
				if (Create != null && Create.Any())
				{
					for (int i = 0; i < Create.Length; i++)
					{
						if (!controller.UserContext.HasAccess(Create[i], Dom.AccessType.Create))
						{
							filterContext.Result = controller.ForbiddenResult();
							break;
						}
					}
				}
			}
		}
	}
}