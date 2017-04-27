using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace RP.Common.Manager
{
	public static class NetworkManager
	{
		public static string ClientIpAddress
		{
			get
			{
				string clientIpAddress = null;
				if (HttpContext.Current != null)
					clientIpAddress = HttpContext.Current.Request.UserHostAddress;

				return clientIpAddress;
			}
		}

		public static string ServerIpAddress
		{
			get
			{
				string serverIpAddress = null;
				IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
				if (host?.AddressList != null && host.AddressList.Any())
				{
					IPAddress ipAddress = host.AddressList
						.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);

					if (ipAddress != null)
						serverIpAddress = ipAddress.ToString();
				}
				return serverIpAddress;
			}
		}
	}
}