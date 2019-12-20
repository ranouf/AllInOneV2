using System;
using System.Linq;
using System.Net;

namespace AllInOne.Common.Extensions
{
    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(
                uri.AbsoluteUri,
                (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), WebUtility.UrlEncode(path.TrimStart('/')))
            ));
        }
    }
}
