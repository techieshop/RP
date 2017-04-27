using RP.Common.Attribute;

namespace RP.DAL.DBContext
{
	public enum DbConnectionEnum
	{
		Undefined = 0,

		[Metadata(Value = "Name=Platform")]
		Platform = 1
	}
}