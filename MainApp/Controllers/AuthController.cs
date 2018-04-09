using MainApp.Models.Auth;
using MainApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MainApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ContentResult AuthenticationInfo()
        {

            string CurrentWindowsIdentityName = WindowsIdentity.GetCurrent().Name;
            string IsUserAuthenticated = User.Identity.IsAuthenticated.ToString();
            string UserAuthenticationType = User.Identity.AuthenticationType;
            string UserName = User.Identity.Name;

            return Content(
                "Application code executed using: " + CurrentWindowsIdentityName + "<br/>" +
                "Is User Authenticated: " + IsUserAuthenticated + "<br/>" +
                "Authentication Type, if Authenticated: " + UserAuthenticationType + "<br/>" +
                "User Name, if Authenticated: " + UserName
                );      
        }

        AuthContext AuthContext = new AuthContext();

        [HttpGet]
        [ActionName("SignIn")]
        public ViewResult SignIn_GET()
        {
            return View("~/Views/Auth/Login/SignInPage.cshtml");
        }


        [HttpPost]
        [ActionName("SignIn")]
        public ActionResult SignIn_POST(User enteredUser)
        {
            User validateUser = AuthContext.Users.SingleOrDefault(x => x.Username == enteredUser.Username.ToLower());

            if (validateUser != null)
            {
                string username = enteredUser.Username.ToLower();
                string password = enteredUser.Password;
                string salt = validateUser.Salt;
                string hashedPassword = Encryption.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));

                if (username == validateUser.Username && hashedPassword == validateUser.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Registration");
                }
            }
            else
            {
                return RedirectToAction("Registration");
            }

        }

        [HttpGet]
        [ActionName("Registration")]
        public ViewResult Registration_GET()
        {
            return View("~/Views/Auth/Registration/SignUpPage.cshtml");
        }

        [HttpPost]
        [ActionName("Registration")]
        public void Registration_POST(User newUser)
        {
            if (AuthContext.Users.SingleOrDefault(x => x.Username == newUser.Username || x.Email == newUser.Email) == null)
            {
                byte[] Salt = Encryption.GenerateSalt(32);
                string StringifiedSalt = Convert.ToBase64String(Salt);

                Salt = Encoding.UTF8.GetBytes(StringifiedSalt);

                User userToRigister = new User();

                userToRigister.Username = newUser.Username.ToLower();
                userToRigister.Salt = StringifiedSalt;
                userToRigister.Password = Encryption.GenerateSaltedHash(Encoding.ASCII.GetBytes(newUser.Password), Salt);
                userToRigister.Email = newUser.Email.ToLower();

                AuthContext.Users.Add(userToRigister);
                AuthContext.SaveChanges();

                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                //return RedirectToAction("Registration");
            }
        }
    }
}