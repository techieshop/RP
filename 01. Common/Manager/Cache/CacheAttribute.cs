using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;

namespace RP.Common.Manager
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method)]
	public class CacheAttribute : HandlerAttribute
	{
		public string Category { get; set; }

		public int DurationInSec { get; set; }

		public override ICallHandler CreateHandler(IUnityContainer container)
		{
			var durationInSec = DurationInSec != default(int) ? DurationInSec : 60;
			var category = !string.IsNullOrEmpty(Category) ? Category : CacheManager.CategoryName.Default;

			return new CacheHandler(container.Resolve<ICacheManager>(), category, durationInSec);
		}
	}
}