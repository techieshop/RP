using System;
using System.Collections.Generic;

namespace RP.Common.Manager
{
	public static class RaceManager
	{
		private static int _a = 9;
		private static int _b = 10;
		private static int _c = 11;
		//private static int _d = 12;
		private static int _e = 24;
		private static int _f = 25;
		private static int _g = 26;
		private static int _h = 27;

		public static double CalculateCoefficient(int pigeonCount, int position)
		{
			return position * 1000.0 / pigeonCount;
		}

		public static double CalculateMark(int numberOfPrizes, int position)
		{
			return (numberOfPrizes - position + 1) * 100.0 / numberOfPrizes;
		}

		public static List<KeyValuePair<int, bool>> GetCategory(int year, double distance, int memberCount, int pigeonCount)
		{
			var dkm = distance / 1000;
			var age = DateTime.Now.Year - year;
			List<KeyValuePair<int, bool>> categories = new List<KeyValuePair<int, bool>>();

			if (dkm >= 100 && dkm <= 400)
			{
				categories.Add(new KeyValuePair<int, bool>(_a, memberCount >= 20 && pigeonCount >= 250));
			}
			if (dkm >= 300 && dkm <= 600)
			{
				categories.Add(new KeyValuePair<int, bool>(_b, memberCount >= 20 && pigeonCount >= 250));
			}
			if (dkm >= 500)
			{
				categories.Add(new KeyValuePair<int, bool>(_c, memberCount >= 20 && pigeonCount >= 150));
			}
			if (dkm >= 700)
			{
				categories.Add(new KeyValuePair<int, bool>(_e, memberCount >= 20 && pigeonCount >= 250));
			}
			if (dkm >= 100 && age == 0)
			{
				categories.Add(new KeyValuePair<int, bool>(_f, memberCount >= 20 && pigeonCount >= 250));
			}
			if (dkm >= 100 && age == 1)
			{
				categories.Add(new KeyValuePair<int, bool>(_g, memberCount >= 20 && pigeonCount >= 250));
			}
			if (dkm >= 300 && age > 1)
			{
				categories.Add(new KeyValuePair<int, bool>(_h, memberCount >= 20 && pigeonCount >= 250));
			}

			return categories;
		}
	}
}