using System.Security.Principal;

namespace ReChat.Helpers
{
    public class MyIdenty : IIdentity
    {
        private ReChat.User user;
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

        public MyIdenty(ReChat.User user)
        {
            this.user = user;
        }
    }
}
