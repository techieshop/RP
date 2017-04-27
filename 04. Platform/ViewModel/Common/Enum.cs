using RP.Common.Attribute;

namespace RP.Platform.ViewModel
{
	public enum DataTypeUI : byte
	{
		[Metadata(Value = "text")]
		Undefined = 0,

		[Metadata(Value = "password")]
		Password = 1
	}

	public enum DateTimeFormatUI : byte
	{
		[Metadata(Value = "DD-MM-YYYY")]
		Date = 0,
		[Metadata(Value = "DD-MM-YYYY HH:mm:ss")]
		DateTime = 1
	}
}