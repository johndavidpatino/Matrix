using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MatrixNext.Web
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent Truncate(this IHtmlHelper htmlHelper, string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || maxLength <= 0)
            {
                return new HtmlString(string.Empty);
            }

            if (value.Length <= maxLength)
            {
                return new HtmlString(value);
            }

            var truncated = value.Substring(0, maxLength).TrimEnd();
            return new HtmlString($"{truncated}...");
        }
    }
}
