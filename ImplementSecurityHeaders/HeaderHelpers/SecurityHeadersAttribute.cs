using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImplementSecurityHeaders.HeaderHelpers
{
    public class SecurityHeadersAttribute:ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                context.HttpContext.Response.Headers.Add("HeaderType", "HeaderValue");
            }
        }
    }
}
