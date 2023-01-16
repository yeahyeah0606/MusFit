using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace MusFit.Utilities
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("Manager") == null && filterContext.HttpContext.Session.GetString("Coach") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "Controller", "Manage" },
                    { "Action", "Login" }
                });
            }
        }
    }
}
