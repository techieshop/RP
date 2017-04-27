namespace RP.Model
{
	public static partial class Dom
	{
		public static partial class EntityType
		{
			public static class User
			{
				public const int Id = 1;

				public static class State
				{
					public const int Created = 1;
					public const int Active = 2;
					public const int Blocked = 3;
					public const int Deleted = 4;
				}
			}
		}
	}
}