using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace DAL.Helper
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RedirectOnErrorAttribute : ActionFilterAttribute
    {
        public RedirectOnErrorAttribute()
        {
            ErrorMessage = "An error occurred while processing your request.";
        }
        public string ReturnController { get; set; }
        public string ReturnAction { get; set; }
        public string ErrorMessage { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null && !filterContext.ExceptionHandled)
            {
                ErrorViewModel viewModel = new ErrorViewModel();
                viewModel.ErrorMessage = ErrorMessage;
                viewModel.ErrorDetails = filterContext.Exception.Message;
                string controller = ReturnController;
                string action = ReturnAction;
                if (String.IsNullOrWhiteSpace(controller))
                {
                    controller = (string)filterContext.RequestContext.RouteData.Values["controller"];
                }
                if (String.IsNullOrWhiteSpace(action))
                {
                    action = "Index";
                }
                UrlHelper helper = new UrlHelper(filterContext.RequestContext);
                viewModel.ReturnUrl = helper.Action(action, controller);
                string url = helper.Action("Index", "Home", viewModel);
                filterContext.Result = new RedirectResult(url);
                filterContext.ExceptionHandled = true;
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Dictionary<int, CheckSessionData> dictionary = CheckSessionData.GetSessionValues();
            int companyId = Convert.ToInt32(dictionary[1].Id == "" ? 0 : Convert.ToInt32(dictionary[1].Id));
            if (companyId == 0)
            {
                HttpContext.Current.Session["logInSession"] = "false";               
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
