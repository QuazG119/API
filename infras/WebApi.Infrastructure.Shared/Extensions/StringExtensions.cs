using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Infrastructure.Shared.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);
    public static string Truncate(this string? value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || maxLength <= 0) return string.Empty;
        if (value.Length <= maxLength) return value;
        var keep = Math.Max(0, maxLength - suffix.Length);
        return value[..keep] + suffix;
    }
    public static string RemoveDiacritics(this string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace('đ', 'd')
            .Replace('Đ', 'D');
    }

    public static string ToSlug(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var s = value.RemoveDiacritics().ToLowerInvariant();
        s = Regex.Replace(s, @"[^a-z0-9\s-]", string.Empty);
        s = Regex.Replace(s, @"[\s-]+", "-");
        return s.Trim('-');
    }

    public static string ToTitleCase(this string? value)
        => string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.Trim().ToLowerInvariant());
    public static string Mask(this string? value, int visible = 4, char maskChar = '*')
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        if (value.Length <= visible) return new string(maskChar, value.Length);
        return new string(maskChar, value.Length - visible) + value[^visible..];
    }

    public static int ToIntOrDefault(this string? value, int defaultValue = 0)
        => int.TryParse(value, out var result) ? result : defaultValue;
}
