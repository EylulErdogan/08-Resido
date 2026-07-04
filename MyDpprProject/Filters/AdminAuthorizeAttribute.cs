using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyDpprProject.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var roleId = context.HttpContext.Session.GetInt32("RoleId");

            if (userId == null || roleId != 1)
            {
                var controller = (Controller)context.Controller;

                controller.TempData["OpenLoginModal"] = true;
                controller.TempData["LoginMessage"] = "Bu sayfaya erişmek için admin olarak giriş yapmalısınız.";

                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}