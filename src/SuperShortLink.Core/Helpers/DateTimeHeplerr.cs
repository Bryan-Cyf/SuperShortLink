using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SuperShortLink.Helpers
{
    public class DateTimeConvertor : JsonConverter<DateTime>
    {
        private readonly string _format;

        public DateTimeConvertor(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }

        public static JsonSerializerOptions Serializer => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
                {
                    new DateTimeConvertor("yyyy-MM-dd HH:mm:ss")
                }
        };
    }

    public static class DateTimeExtensions
    {

        public static double ToUnixTimestamp(this DateTime target)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Math.Floor((target - dateTime).TotalSeconds);
        }

        public static DateTime FromUnixTimestamp(this double unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTime);
        }

        public static DateTime ToDayEnd(this DateTime target)
        {
            return target.Date.AddDays(1.0).AddMilliseconds(-1.0);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int num = dt.DayOfWeek - startOfWeek;
            if (num < 0)
            {
                num += 7;
            }

            return dt.AddDays(-1 * num).Date;
        }

        public static IEnumerable<DateTime> DaysOfMonth(int year, int month)
        {
            return from day in Enumerable.Range(0, DateTime.DaysInMonth(year, month))
                   select new DateTime(year, month, day + 1);
        }

        public static int WeekDayInstanceOfMonth(this DateTime dateTime)
        {
            int y = 0;
            return (from date in DaysOfMonth(dateTime.Year, dateTime.Month)
                    where dateTime.DayOfWeek.Equals(date.DayOfWeek)
                    select date into x
                    select new
                    {
                        n = ++y,
                        date = x
                    } into x
                    where x.date.Equals(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day))
                    select x.n).FirstOrDefault();
        }

        public static int TotalDaysInMonth(this DateTime dateTime)
        {
            return DaysOfMonth(dateTime.Year, dateTime.Month).Count();
        }

        public static DateTime ToDateTimeUnspecified(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
            {
                return date;
            }

            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Unspecified);
        }

        public static DateTime TrimMilliseconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }
    }
}
