using Microsoft.Practices.Unity.InterceptionExtension;
using RP.Common.Extension;

namespace RP.Common.Manager
{
	public class CacheHandler : ICallHandler
	{
		private readonly ICacheManager _cacheManager;
		private readonly string _category;
		private readonly int _durationInSec;

		public CacheHandler(ICacheManager cacheManager, string category, int durationInSec)
		{
			_cacheManager = cacheManager;
			_category = category;
			_durationInSec = durationInSec;
		}

		public int Order { get; set; }

		public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
		{
			string key = input.GetDescription();
			IMethodReturn methodReturn = _cacheManager.Get<IMethodReturn>(_category, key);
			if (methodReturn == null)
			{
				methodReturn = getNext()(input, getNext);
				_cacheManager.Set(_category, key, methodReturn, _durationInSec);
			}
			return methodReturn;
		}
	}
}