using MyCompany.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyCompany.Authorize
{
    public class Admin : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
           
            if ((int?)HttpContext.Current.Session["RoleId"]== 1)
            {
                return;
            }
            else { 

            filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Home"},
                    {"Action", "Login"}
                });
            
            base.OnActionExecuting(filterContext);
            }
        }
    }

    public class RegulUser : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

            if ((int?)HttpContext.Current.Session["RoleId"] == 2)
            {
                return;
            }
            else
            {

                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Home"},
                    {"Action", "Login"}
                });

                base.OnActionExecuting(filterContext);
            }
        }
    }

    public class AllUsers : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

            if ((int?)HttpContext.Current.Session["RoleId"] == 2 || (int?)HttpContext.Current.Session["RoleId"] == 1)
            {
                return;
            }
            else
            {

                filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    {"Controller", "Home"},
                    {"Action", "Login"}
                });

                base.OnActionExecuting(filterContext);
            }
        }
    }



}