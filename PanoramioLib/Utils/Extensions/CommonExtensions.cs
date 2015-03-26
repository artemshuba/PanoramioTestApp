using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramioLib.Utils.Extensions
{
    internal static class CommonExtensions
    {
        public static string ToQueryString(this IDictionary<string, string> dict)
        {
            if (dict == null)
                return string.Empty;

            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }
    }
}
