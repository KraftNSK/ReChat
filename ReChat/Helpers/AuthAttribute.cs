using System.Web;
using System.Web.Mvc;

namespace ReChat.Helpers
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public string UserRoles { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (httpContext.Request.Cookies["AUTH"] != null) ? true : false;
        }
    }
}
