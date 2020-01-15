using System.Globalization;

namespace Entrega2_IEI.Library
{
    public static class ScraperUtils
    {
        public static bool ContainsIgnoreCase(this string source, string value)
            => CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, value, CompareOptions.IgnoreCase) >= 0;
    }
}
