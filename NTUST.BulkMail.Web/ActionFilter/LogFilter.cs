using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTUST.BulkMail.Web.ActionFilter
{
    public class LogFilter : ActionFilterAttribute
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller.ControllerContext;
            var userName = controller.HttpContext.User.Identity.Name;
            var ip = controller.HttpContext.Request.UserHostAddress;
            var actionName = controller.RouteData.Values["action"];

            logger
                .WithProperty("Property1", userName)
                .WithProperty("Property2", ip)
                .Info($"{userName} Into {actionName} Page");

            base.OnActionExecuting(filterContext);
        }
    }
}