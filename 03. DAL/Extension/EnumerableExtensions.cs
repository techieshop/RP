using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.Common.Extension
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<KeyList> ToKeyList(this IEnumerable<int> list)
		{
			IEnumerable<KeyList> result = null;
			if (list != null)
				result = list.Select(t => new KeyList { Id = t });
			return result;
		}
	}
}