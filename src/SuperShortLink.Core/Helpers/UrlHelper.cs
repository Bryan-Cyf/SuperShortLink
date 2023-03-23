using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink.Helpers
{
    internal class UrlHelper
    {
        public static bool CheckVaild(string url)
        {
            bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);
            return isValid && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
