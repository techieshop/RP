using RP.Model;
using System;
using System.Web;

namespace RP.Platform.Context
{
	public partial class EntityContext
	{
		public void AddEntityProgress(EntityInfo entityInfo, EntityProgress entityProgress)
		{
			entityProgress.DateTime = DateTime.UtcNow;
			entityProgress.IpAddress = HttpContext.Current.Request.UserHostAddress;

			if (entityProgress.UserId == null && _userContext.IsAuthenticated)
				entityProgress.UserId = _userContext.User.Id;

			entityInfo.EntityProgress.Add(entityProgress);
		}
	}
}