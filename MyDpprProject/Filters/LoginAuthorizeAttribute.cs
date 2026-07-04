using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyDpprProject.Filters
{
    public class LoginAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                var controller = (Controller)context.Controller;

                controller.TempData["OpenLoginModal"] = true;
                controller.TempData["LoginMessage"] = "Bu sayfaya erişmek için giriş yapmalısınız.";

                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}