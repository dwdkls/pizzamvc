using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Pizza.Contracts.Presentation.Security;
using Pizza.Contracts.Presentation.Security.ServiceContracts;
using Pizza.Contracts.Presentation.Security.ViewModels;
using Pizza.Framework.Security;
using Pizza.Framework.Utils.ValueInjection;
using Pizza.Mvc.Security;

namespace Pizza.Mvc.Controllers
{
    public class AccountController : AccountControllerBase<PizzaUserViewModel, PizzaPrincipalSerializeModel>
    {
        public AccountController(IAuthService membershipService) : base(membershipService)
        {
        }
    }

    public abstract class AccountControllerBase<TUserViewModel, TPrincipalSerializeModel> : CoreControllerBase 
        where TUserViewModel : PizzaUserViewModel, new() 
        where TPrincipalSerializeModel : new()
    {
        private readonly IAuthService userSecurityService;

        public AccountControllerBase(IAuthService membershipService)
        {
            this.userSecurityService = membershipService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var loginRequest = new LoginRequest(model.UserName, model.Password);

            var loginResult = this.userSecurityService.LoginUser<TUserViewModel>(loginRequest);

            if (loginResult.Succeed)
            {
                var userData = this.GetSerializedModel(loginResult.User);
                this.AddAuthenticationCookieToResponse(model, userData);

                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }
                else
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            else
            {
                this.ModelState.AddModelError("", "Incorrect user name or password.");
            }

            return View(model);
        }

        private string GetSerializedModel(TUserViewModel user)
        {
            var serializeModel = this.GetPrincipalSerializeModel(user);
            string userData = new JavaScriptSerializer().Serialize(serializeModel);
            return userData;
        }

        private TPrincipalSerializeModel GetPrincipalSerializeModel(TUserViewModel userViewModel)
        {
            var serializeModel = new TPrincipalSerializeModel();
            serializeModel.InjectFrom(userViewModel);

            return serializeModel;
        }

        private void AddAuthenticationCookieToResponse(LoginViewModel model, string userData)
        {
            var authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            Response.Cookies.Add(faCookie);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new ChangePasswordRequest(User.Identity.Name, model.OldPassword, model.NewPassword);

                var result = this.userSecurityService.ChangePassword(request);

                if (result.Succeed)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                }

            }

            return View(model);
        }
    }
}
