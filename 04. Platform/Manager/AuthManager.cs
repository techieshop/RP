using RP.Common.Manager;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace RP.Platform.Manager
{
	public static class AuthManager
	{
		private static readonly object CacheLocker = new object();
		private static readonly TimeSpan AuthenticationContentTimeout;
		private static readonly TimeSpan AuthenticationLongTimeout;
		private static readonly TimeSpan AuthenticationShortTimeout;

		static AuthManager()
		{
			AuthenticationShortTimeout = new TimeSpan(0, 0, 20, 0);
			AuthenticationLongTimeout = FormsAuthentication.Timeout; // <system.web><authentication><forms timeout="Minutes">
			AuthenticationContentTimeout = new TimeSpan(0, 0, 1, 0);
		}

		public static void AuthenticateRequest()
		{
			var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

			if (cookie != null)
			{
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
				try
				{
					IIdentity userIdentity = new RPIdentity(authTicket);
					HttpContext.Current.User = new GenericPrincipal(userIdentity, null);
				}
				catch
				{
					FormsAuthentication.SignOut();
				}
			}
		}

		public static string DecryptAndAuthenticateContentUser(string token, string data)
		{ // should be called on content app
			string url = HttpContext.Current.Cache[GetCacheKey(token)] as string;
			if (string.IsNullOrEmpty(url))
				return null;

			RPIdentity rpIdentity;
			try
			{
				string userData = EncryptionManager.Decrypt(data);
				rpIdentity = new RPIdentity(userData);
			}
			catch
			{
				return null;
			}
			DateTime now = DateTime.Now;
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				2,
				rpIdentity.Name,
				now,
				now.Add(AuthenticationContentTimeout),
				false,
				rpIdentity.GetCookieString(),
				FormsAuthentication.FormsCookiePath
			);
			SetTicketToCookie(ticket);
			return url;
		}

		public static string GenerateContentAuthenticationRequest(HttpContextBase httpContext)
		{ // should be called on content app
			string token;
			lock (CacheLocker)
			{
				string cacheKey;
				do
				{
					var randomBytes = EncryptionManager.GetRandomBytes(24);
					token = HttpServerUtility.UrlTokenEncode(randomBytes);
					cacheKey = GetCacheKey(token);
				} while (httpContext.Cache[token] != null);

				httpContext.Cache.Insert(cacheKey, httpContext.Request.RawUrl, null, DateTime.UtcNow.AddSeconds(60), Cache.NoSlidingExpiration);
			}

			var uriBuilder = new UriBuilder
			{
				Host = httpContext.Request.Params["webhost"],
				Scheme = HttpContext.Current.Request.Url.Scheme,
				Path = "authorization",
				Query = $"token={token}"
			};

			return uriBuilder.ToString();
		}

		public static string GenerateContentAuthenticationResponse(string token)
		{ // should be called on web app
			RPIdentity rpIdentity = (RPIdentity)HttpContext.Current.User.Identity;

			string data = EncryptionManager.Encrypt(rpIdentity.GetCookieString());
			var uriBuilder = new UriBuilder
			{
				Host = WebUrlManager.Host,
				Scheme = HttpContext.Current.Request.Url.Scheme,
				Path = "user/authenticate",
				Query = $"token={token}&data={data}"
			};
			return uriBuilder.ToString();
		}

		public static int? GetOrganizationId()
		{
			RPIdentity rpIdentity = HttpContext.Current.User.Identity as RPIdentity;
			return rpIdentity?.OrganizationId;
		}

		public static string GetUserEmail()
		{
			RPIdentity rpIdentity = HttpContext.Current.User.Identity as RPIdentity;
			return rpIdentity?.Name;
		}

		public static int? GetUserId()
		{
			RPIdentity rpIdentity = HttpContext.Current.User.Identity as RPIdentity;
			return rpIdentity?.UserId;
		}

		public static void RenewAuthCookie()
		{
			if (!FormsAuthentication.SlidingExpiration)
				return;

			var requestCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

			if (requestCookie == null)
				return;

			FormsAuthenticationTicket oldTicket = FormsAuthentication.Decrypt(requestCookie.Value);

			// Standart .Net "sliding expiration" works when ticket's half-lifetime passed.
			// We will slide if at least 10 seconds passed.
			DateTime issueDate = oldTicket.IssueDate;
			DateTime now = DateTime.Now;
			if ((now - issueDate).TotalSeconds < 10)
				return;

			FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
				oldTicket.Version,
				oldTicket.Name,
				now,
				now + (oldTicket.Expiration - oldTicket.IssueDate),
				oldTicket.IsPersistent,
				oldTicket.UserData,
				oldTicket.CookiePath);

			SetTicketToCookie(newTicket);
		}

		public static void SetAuthCookie(string userName, int userId, int organizationId, bool isPersistent)
		{
			if (userName == null)
				userName = string.Empty;

			var totalMinutes = isPersistent ? AuthenticationLongTimeout : AuthenticationShortTimeout;

			var rpIdentity = RPIdentity.Create(userName, userId, organizationId);

			DateTime now = DateTime.Now;
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				2,
				rpIdentity.Name,
				now,
				now.Add(totalMinutes),
				isPersistent,
				rpIdentity.GetCookieString(),
				FormsAuthentication.FormsCookiePath
			);

			SetTicketToCookie(ticket);
		}

		public static void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		private static string GetCacheKey(string token)
		{
			return "CachedToken_" + token;
		}

		private static void SetTicketToCookie(FormsAuthenticationTicket ticket)
		{
			string strTicket = FormsAuthentication.Encrypt(ticket);

			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket)
			{
				HttpOnly = true,
				Path = FormsAuthentication.FormsCookiePath,
				Secure = FormsAuthentication.RequireSSL
			};

			// <system.web><authentication><forms path="str">
			// <system.web><authentication><forms requireSSL="bool">
			if (FormsAuthentication.CookieDomain != null)
				cookie.Domain = FormsAuthentication.CookieDomain; // <system.web><authentication><forms domain="str">
			if (ticket.IsPersistent)
				cookie.Expires = ticket.Expiration;

			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}

	public class RPIdentity : IIdentity
	{
		/// <summary>
		/// This constructor is intended to crash for any invalid "FormsAuthenticationTicket". Exception should be handled on caller side.
		/// </summary>
		public RPIdentity(FormsAuthenticationTicket ticket)
			: this(ticket.UserData)
		{
		}

		public RPIdentity(string data)
		{
			HasUserData = true;
			string[] auth = data.Split('|');
			if (auth.Length < 2)
				throw new IndexOutOfRangeException();

			Name = auth[0];
			UserId = int.Parse(auth[1]);
			OrganizationId = int.Parse(auth[2]);
		}

		private RPIdentity()
		{
		}

		public string AuthenticationType => "";
		public bool HasUserData { get; private set; }
		public bool IsAuthenticated => !Name.Equals("");
		public string Name { get; private set; }
		public int OrganizationId { get; private set; }
		public int UserId { get; private set; }

		public static RPIdentity Create(string name, int userId, int organizationId)
		{
			return new RPIdentity
			{
				Name = name,
				UserId = userId,
				OrganizationId = organizationId
			};
		}

		public string GetCookieString()
		{
			return $"{Name}|{UserId}|{OrganizationId}";
		}
	}
}