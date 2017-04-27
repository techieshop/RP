using System;
using System.Device.Location;

namespace RP.Common.Manager
{
	/// <summary>
	/// All calculation in meters
	/// </summary>
	public static class CoordinatesManager
	{
		public const double EarthRadius = 6371000.8;
		private static readonly double MinLatitude = ConvertDegreesToRadians(-90);
		private static readonly double MaxLatitude = ConvertDegreesToRadians(90);
		private static readonly double MinLongitude = ConvertDegreesToRadians(-180);
		private static readonly double MaxLongitude = ConvertDegreesToRadians(180);

		public static double ConvertDegreesToRadians(double degrees)
		{
			return Math.PI / 180 * degrees;
		}

		public static double ConvertRadiansToDegrees(double radian)
		{
			return radian * (180.0 / Math.PI);
		}

		public static double ConvertFromDdmmsstoDddddd(int degree, int minute, int second)
		{
			return degree + minute / 60 + second / 3600;
		}


		/// <summary>
		/// Result in meters
		/// </summary>
		/// <param name="latitude1"></param>
		/// <param name="longitude1"></param>
		/// <param name="latitude2"></param>
		/// <param name="longitude2"></param>
		/// <returns></returns>
		public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
		{
			double latitudeDelta = ConvertDegreesToRadians(latitude2 - latitude1);
			double longitudeDelta = ConvertDegreesToRadians(longitude2 - longitude1);

			double a = Math.Sin(latitudeDelta / 2) * Math.Sin(latitudeDelta / 2) +
					Math.Cos(ConvertDegreesToRadians(latitude1)) * Math.Cos(ConvertDegreesToRadians(latitude2)) * (Math.Sin(longitudeDelta / 2) * Math.Sin(longitudeDelta / 2));
			double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return angle * EarthRadius;
		}

		public static void GetBounds(double latitude, double longitude, double distance, out double minLatitude, out double maxLatitude, out double minLongitude, out double maxLongitude)
		{
			if (distance < 0)
				throw new Exception("Distance cannot be less than 0");

			double lat = ConvertDegreesToRadians(latitude);
			double lon = ConvertDegreesToRadians(longitude);
			double deltaLatitude = distance / EarthRadius;
			minLatitude = lat - deltaLatitude;
			maxLatitude = lat + deltaLatitude;

			if (minLatitude > MinLatitude && maxLatitude < MaxLatitude)
			{
				double deltaLon = Math.Asin(Math.Sin(deltaLatitude) / Math.Cos(lat));

				minLongitude = lon - deltaLon;
				if (minLongitude < MinLongitude)
					minLongitude += 2d * Math.PI;

				maxLongitude = lon + deltaLon;
				if (maxLongitude > MaxLongitude)
					maxLongitude -= 2d * Math.PI;
			}
			else
			{
				minLatitude = Math.Max(minLatitude, MinLatitude);
				maxLatitude = Math.Min(maxLatitude, MaxLatitude);
				minLongitude = MinLongitude;
				maxLongitude = MaxLongitude;
			}

			minLatitude = ConvertRadiansToDegrees(minLatitude);
			maxLatitude = ConvertRadiansToDegrees(maxLatitude);
			minLongitude = ConvertRadiansToDegrees(minLongitude);
			maxLongitude = ConvertRadiansToDegrees(maxLongitude);
		}

		public static double CalculateDistanceUsingGeoCoordinate(double latitude1, double longitude1, double latitude2, double longitude2)
		{
			GeoCoordinate geoCoordinate1 = new GeoCoordinate(latitude1, longitude1);
			GeoCoordinate geoCoordinate2 = new GeoCoordinate(latitude2, longitude2);

			//return int meters
			return geoCoordinate1.GetDistanceTo(geoCoordinate2);
		}
	}
}