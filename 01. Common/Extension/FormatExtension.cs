using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RP.Common.Extension
{
	public static class Format
	{
		public static string FormattedAddress(string street, string number, string city, string country)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.AppendFormat("{0} {1}, {2}", city, street, number);
			if (!string.IsNullOrEmpty(country))
				strBuilder.AppendFormat(", {0}", country);
			return strBuilder.ToString();
		}

		public static string FormattedFullName(string lastName, string firstName, string middleName)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.AppendFormat("{0} {1} {2}", lastName, firstName, middleName);

			return strBuilder.ToString();
		}

		public static string FormattedInitials(string lastName, string firstName, string middleName)
		{
			StringBuilder strBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(lastName))
			{
				strBuilder.AppendFormat("{0}", lastName);
			}
			if (!string.IsNullOrWhiteSpace(firstName))
			{
				strBuilder.AppendFormat(" {0}.", firstName[0]);
			}
			if (!string.IsNullOrWhiteSpace(middleName))
			{
				strBuilder.AppendFormat("{0}.", middleName[0]);
			}

			return strBuilder.ToString();
		}

		public static string FormattedNumber(int number, int? version = null)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.AppendFormat("#{0}", number);
			if (version != null)
				strBuilder.AppendFormat(" ({0})", version);
			return strBuilder.ToString();
		}

		public static string FormattedNumber(double? number, string format)
		{
			string result = string.Empty;
			if (number != null)
			{
				result = string.IsNullOrEmpty(format) ? number.ToString() : string.Format(format, number);
			}
			return result;
		}

		public static string FormattedPeriod(DateTime? startDate, DateTime? endDate, string format)
		{
			StringBuilder strBuilder = new StringBuilder();
			if (startDate != null)
			{
				strBuilder.Append(startDate.ToShortDateString(format));
				if (endDate != null)
					strBuilder.AppendFormat(" - {0}", endDate.ToShortDateString(format));
			}
			return strBuilder.ToString();
		}

		public static string FormattedRange(int? min, int? max)
		{
			return $"{min} - {max}";
		}

		public static string FormattedRange(double? min, double? max, string format)
		{
			return $"{FormattedNumber(min, format)} - {FormattedNumber(max, format)}";
		}

		public static string FormattedRing(int year, string code, string number)
		{
			StringBuilder strBuilder = new StringBuilder();

			if (!string.IsNullOrEmpty(code))
			{
				strBuilder.AppendFormat("{0}-{1}", year.ToString().Substring(2), code);
			}
			else
			{
				strBuilder.AppendFormat("{0}", year.ToString().Substring(2));
			}
			strBuilder.AppendFormat("-{0}", number);

			return strBuilder.ToString();
		}

		public static string FormattedTime(TimeSpan time, string format)
		{
			return (DateTime.Today + time).ToString(format);
		}

		public static string FormattedValueInBrackets(string value)
		{
			return $"({value})";
		}

		public static string Join(string separator, params string[] values)
		{
			return Join(separator, (IEnumerable<string>)values);
		}

		public static string Join(string separator, IEnumerable<string> values)
		{
			StringBuilder joinValues = new StringBuilder();
			var enumerable = values as IList<string> ?? values.ToList();
			enumerable.ForEach(value =>
			{
				if (!string.IsNullOrEmpty(value))
				{
					joinValues.Append(value);
					if (!enumerable.IsLast(value))
					{
						joinValues.Append(separator);
					}
				}
			});
			return joinValues.ToString();
		}

		public static string ToShortDateString(this DateTime? dateTime, string format)
		{
			string result = null;
			if (dateTime.HasValue && dateTime.Value != default(DateTime))
			{
				result = string.IsNullOrEmpty(format) ? dateTime.Value.ToShortDateString() : dateTime.Value.ToString(format);
			}
			return result;
		}

		public static string ToShortDateString(this DateTime dateTime, string format)
		{
			string result = null;
			if (dateTime != default(DateTime))
			{
				result = string.IsNullOrEmpty(format) ? dateTime.ToShortDateString() : dateTime.ToString(format);
			}
			return result;
		}
	}
}