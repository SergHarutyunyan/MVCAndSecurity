using MainApp.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MainApp.Security
{
    public class CustomMemberShipUser : MembershipUser
    {

        #region User Properties  

        public int UserId { get; set; }
        public ICollection<Role> Roles { get; set; }

        #endregion

        public CustomMemberShipUser(User user) : base("CustomMembership", user.Username, user.UserID, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserID;
            Roles = user.Roles;
        }
    }
}