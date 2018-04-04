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

            // By Default anonumous authentication is enabled in IIS.

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

            // in Web.Config file, in <system.web> tag, we can put <authorization> </authorization>.
            // It allows us to allow or deny users.
        }

        /*
            ___________________________________________  Anonymous Authentication and Impersonation  ___________________________________________

        
            By Default anonumous authentication is enabled in IIS. 
            Anonymous authentication is fine for web sites that contain public information, that every one can see.
         
            To enable impersonation, set impersonate = "true" for the identity element in web.config.
            The application pool's managed pipeline mode must be classic in that case.
            The impersonate can also be enabled or disabled from IIS.

            When the application uses anonymous authentication and 
            1. if IMPERSONATION is disabled, then, the application pool identity is used to execute the application code.
            2. if IMPERSONATION is enabled, then, 'NT AUTHORITY\IUSR' account is used to execute the application code.

            If there are 2 or more websites hosted on a machine with IUSR as the anonymous account, then they can access each other's content.
            If we want to isolate, each application content, the applications can be deployed to different application pools, and
            the NTFS file permissions can be set for the respective application pool identity. 
            
            It is also possible to impersonate, with a specific user name and password. With this setting, whenever any user belonging to the
            "Administrators" group requests page from the Admin folder, the code will be executed using specified account.
        

        _______________________________________________   Windows Authentication  ___________________________________________________________

        
            Indentifies and authorizes users based on the server's user list.
            Access to resources on the server is then granted or denied based on the user account's privileges.

            Windows authentication is best suited for Intranet Web Applications.

            The advantage of windows authentication is that, the Web application can use the exact same security scheme
            that applies to yout corporate work. User names, passwords, and permissions are the same for network resources and Web applications.
            
            Security can be configured in IIS, and also in the application itself.

            If both, anonymous and windows authentication are enabled in IIS, and, if we don't have a deny entry for anonymous users,
            in the web.config file, than the resources on the web server are accessed using anonymous authentication.

            We must disable anonymous authentication. We can do it in IIS, or in web.config - <deny users = "?"/>.

            My Windows User is HELPSYSTEMS\Sergey.Harutyunyan.
            Our application code can be executed by logged in user identity. For that we must enable impersonation.
            Now if we turn on the impersonation, our application code will run with HELPSYSTEMS\Sergey.Harutyunyan.
          
            If impersonation is enabled, the application executing using the permissions found in your user account.

            ------------------------------------------- Authorization --------------------------------------------------------------------------

            We can allow access to specific users like - <allow users="HELPSYSTEMS\Sergey.Harutyunyan"/>          
            We can use Windows roles to control access like - <allow roles="Administrators"/>

            Programmitically we can check if the user belongs to specific role -             
            if (User.IsInRole("Administrators"))
            {
                // Do Admin stuff
            }
            else
            {
                // Do Non-Admin stuff
            }

            ------------------------------------------- Folder Level Authorization --------------------------------------------------------------------------
            
            We can add a new folder - Admins in our project.
            Then we add a view in it.
            So, we can make this view accessable only for administrators. For that we need to create config file in that Admins folder.
            Just putting allow roles administrators and deny all users will make this task completed.
            
        
      ____________________________________________________   Forms Authentication  ___________________________________________________________

            
            To enable form authentication with config file, we can add <authentication> tag with mode="Forms" attribute in <system.web>
            We can specify our login page and default page with - <forms loginUrl = "" defaultUrl = ""> 
        */

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