using System.Security.Principal;
using ReChat.Models;

namespace ReChat.Helpers
{
    public class MyIdenty : IIdentity
    {
        private User user;
        public string AuthenticationType
        {
            get
            {
                return "local";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if(user!= null)
                    if(user.Token != "")
                        return true;
                return false;
            }
        }

        public string Name
        {
            get
            {
                if (user != null)
                    if (user.Token != "")
                        return user.Login;
                return ""; 
            }
        }

        public MyIdenty(User user)
        {
            this.user = user;
        }
    }
}
