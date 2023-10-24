using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class CoockieUtil
    {
        public static void AddCookie(HttpResponse response, string cookieName, string value, DateTime expireDate)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = value;
            cookie.Expires = expireDate;
            response.SetCookie(cookie);
        }

        public static HttpCookie GetCookie(HttpRequest request, string cookieName)
        {
            var result = request.Cookies[cookieName];
            return result != null ? result : null;
        }
    }
}
