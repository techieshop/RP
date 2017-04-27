using RP.Model;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class UserRepository
	{
		public bool Exists(string email, int? userId = null)
		{
			bool exist = false;

			if (userId != null)
			{
				var user = GetQuery<User>().SingleOrDefault(u => u.Email.ToLower().Equals(email.ToLower()) && !u.Id.Equals(userId.Value));
				if (user != null)
					exist = true;
			}
			else
			{
				var user = GetQuery<User>().SingleOrDefault(u => u.Email.ToLower().Equals(email.ToLower()));
				if (user != null)
					exist = true;
			}

			return exist;
		}

		public User GetUser(string email, string password)
		{
			return GetQuery<User>().SingleOrDefault(t => t.Email.ToLower() == email.ToLower() && t.Password == password);
		}

		public User GetUser(int userId)
		{
			return GetEntity<User>(userId, u => u.UserRoles);
		}

		public UserAuth GetUserAuth(int? userId)
		{
			var parameters = new
			{
				UserId = userId
			};
			return ExecuteMultiResultSetSp<UserAuth>("[acc].[spGetUserAuth]", parameters);
		}

		public UserProfile GetUserProfile(int userId)
		{
			var parameters = new
			{
				UserId = userId
			};
			return ExecuteMultiResultSetSp<UserProfile>("[acc].[spGetUserProfile]", parameters);
		}

		public bool HasUserLoginAccess(int userId)
		{
			var user = GetEntity<User>(userId, u => u.EntityInfo, u => u.EntityInfo.EntityOrganizations);
			if (user?.EntityInfo?.EntityOrganizations != null)
			{
				if (user.EntityInfo
						.EntityOrganizations
						.FirstOrDefault(eo => eo.EntityStateId != Dom.EntityType.User.State.Active) != null ||
					user.EntityInfo
						.EntityOrganizations
						.FirstOrDefault(eo => eo.Organization
												.EntityInfo
												.EntityOrganizations
												.FirstOrDefault(m => m.EntityStateId != Dom.EntityType.Organization.State.Active) != null) != null ||
					user.UserRoles
						.Count(ur => ur.Role
										.EntityInfo
										.EntityOrganizations
										.FirstOrDefault(eo => eo.EntityStateId == Dom.EntityType.Role.State.Active) != null) == 0)
				{
					return false;
				}
				return true;
			}

			return false;
		}
	}
}