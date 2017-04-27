using System;

namespace RP.Common.Extension
{
	public static class StringExtension
	{
		public static string ToUrlViewMode(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return "";

			return value.StartsWith("http") ? value.Substring(value.IndexOf("://", StringComparison.Ordinal) + 3).TrimEnd('/') : value.TrimEnd('/');
		}
	}
}