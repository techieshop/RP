namespace RP.Model
{
	public static partial class Dom
	{
		public static partial class EntityType
		{
			public static class Organization
			{
				public const int Id = 2;

				public static class State
				{
					public const int Created = 5;
					public const int Active = 6;
					public const int Inactive = 7;
					public const int Blocked = 8;
					public const int Closed = 9;
					public const int Deleted = 10;
				}
			}
		}
	}
}