using System;

namespace RP.Common.Attribute
{
	[AttributeUsage(AttributeTargets.All)]
	public class MetadataAttribute : System.Attribute
	{
		public int IntValue { get; set; }
		public bool IsIgnored { get; set; }
		public int Order { get; set; }
		public string Value { get; set; }
	}
}