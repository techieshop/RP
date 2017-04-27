namespace RP.Model
{
	public static partial class Dom
	{
		public static partial class EntityType
		{
			public static class Point
			{
				public const int Id = 10;

				public static class State
				{
					public const int Created = 40;
					public const int Active = 41;
					public const int Inactive = 42;
					public const int Deleted = 43;
				}
			}
		}
	}
}