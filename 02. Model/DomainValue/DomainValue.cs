namespace RP.Model
{
	public static partial class Dom
	{
		public static class AccessType
		{
			public const int Create = 4;
			public const int NoAccess = 1;
			public const int Read = 2;
			public const int ReadWrite = 3;
			public const int Undefined = 0;
		}

		public static class Category
		{
			public const int A = 9;
			public const int B = 10;
			public const int C = 11;
			public const int D = 12;
			public const int E = 24;
			public const int F = 25;
			public const int G = 26;
			public const int H = 27;
		}

		public static class Common
		{
			public const int OrganizationId = 1;
			public const int UserId = 1;
		}

		public static class ContentType
		{
			public const string PdfContentType = "application/pdf";
			public const string XmlContentType = "application/xml";
		}

		public static class Country
		{
			public const int Ukraine = 216;
		}

		public static class FileFormat
		{
			public const string Pdf = "pdf";
		}

		public static class Gender
		{
			public const int Female = 2;
			public const int Male = 1;
			public const int NotSpecified = 3;
		}

		public static class Language
		{
			public const int English = 7;
			public const int System = 8;
			public const int Ukraine = 8;
		}

		public static class MemberType
		{
			public const int Cashier = 18;
			public const int Deputy = 14;
			public const int Head = 13;
			public const int Judge = 16;
			public const int MainJudge = 15;
			public const int Member = 19;
			public const int Secretary = 17;
		}

		public static class OrganizationType
		{
			public const int Club = 3;
			public const int Country = 1;
			public const int Region = 2;
		}

		public static class Pagination
		{
			public const int P10 = 10;
			public const int P100 = 100;
			public const int P20 = 20;
			public const int P30 = 30;
			public const int P50 = 50;
		}

		public static class RaceType
		{
			public const int Common = 23;
			public const int Single = 22;
		}

		public static class SeasonType
		{
			public const int Adults = 20;
			public const int Youth = 21;
		}

		public static class Sex
		{
			public const int Cock = 4;
			public const int Hen = 5;
			public const int NotIdentified = 6;
		}

		public static class Zoom
		{
			public const int X12 = 12;
			public const int X6 = 6;
		}
	}
}