using System;
using System.Web.Mvc;

namespace MyCompany.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MaxJsonSizeAttribute : ActionFilterAttribute
    {
        // Default: 10 MB worth of one byte chars
        private int maxLength = 10 * 1024 * 1024;

        public int MaxLength
        {
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "Value must be at least 0.");

                maxLength = value;
            }
            get { return maxLength; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            JsonResult json = filterContext.Result as JsonResult;
            if (json != null)
            {
                if (maxLength == 0)
                {
                    json.MaxJsonLength = int.MaxValue;
                }
                else
                {
                    json.MaxJsonLength = maxLength;
                }
            }
        }
    }
}