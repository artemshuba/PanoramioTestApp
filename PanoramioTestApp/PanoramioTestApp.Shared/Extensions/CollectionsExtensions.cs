using System.Collections;

namespace PanoramioTestApp.Extensions
{
    public static class CollectionsExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count == 0;
        }
    }
}
