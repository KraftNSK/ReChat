using System;
using System.Web;

namespace ReChat.Helpers
{
    public static class Auth
    {
        public static void LogIn(HttpContextBase httpContext, string cookies)
        {
            HttpCookie cookie = new HttpCookie("AUTH");
            cookie.Value = cookies;
            cookie.Expires = DateTime.Now.AddYears(1);
            

            httpContext.Response.Cookies.Add(cookie);
        }

        public static void LogOff(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["AUTH"] != null)
            {
                HttpCookie cookie = new HttpCookie("AUTH");
                cookie.Expires = DateTime.Now.AddYears(-2);

                httpContext.Response.Cookies.Add(cookie);
            }
        }

        public static User GetUser(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies["AUTH"];

            if (authCookie != null)
            {
                User user = DBase.GetUserByCookeis(authCookie.Value);

                return user;
            }

            return null;
        }

        public static bool IsAuthenticated(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies["AUTH"];

            if (authCookie != null)
            {
                User user = DBase.GetUserByCookeis(authCookie.Value);

                return user != null;
            }

            return false;
        }
    }

}
