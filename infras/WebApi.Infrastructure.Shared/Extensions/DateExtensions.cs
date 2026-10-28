using System.Globalization;

namespace WebApi.Infrastructure.Shared.Extensions;

public static class DateExtensions
{
    private static readonly TimeSpan VietnamOffset = TimeSpan.FromHours(7);
    public const string DefaultFormat = "dd/MM/yyyy HH:mm:ss";
    public static DateTime ToVietnamTime(this DateTime utc)
        => DateTime.SpecifyKind(utc, DateTimeKind.Utc).Add(VietnamOffset);

    public static DateTime StartOfDay(this DateTime date) => date.Date;

    public static DateTime EndOfDay(this DateTime date) => date.Date.AddDays(1).AddTicks(-1);

    public static DateTime StartOfMonth(this DateTime date) => new(date.Year, date.Month, 1, 0, 0, 0, date.Kind);

    public static DateTime EndOfMonth(this DateTime date) => date.StartOfMonth().AddMonths(1).AddTicks(-1);

    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public static bool IsBetween(this DateTime date, DateTime from, DateTime to)
        => date >= from && date <= to;

    public static long ToUnixSeconds(this DateTime date)
        => new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Utc)).ToUnixTimeSeconds();

    public static string ToDisplayString(this DateTime date, string format = DefaultFormat)
        => date.ToString(format, CultureInfo.InvariantCulture);

    public static string ToDisplayString(this DateTime? date, string format = DefaultFormat, string fallback = "")
        => date.HasValue ? date.Value.ToDisplayString(format) : fallback;
}
