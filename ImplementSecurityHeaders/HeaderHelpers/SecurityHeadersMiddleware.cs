using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ImplementSecurityHeaders.HeaderHelpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("HeaderType", "HeaderValue");
            httpContext.Response.Headers.Add("Content-Security-Policy","style-src 'none'");//no css
            httpContext.Response.Headers.Add("Content-Security-Policy", "style-src 'none' 'unsafe inline'");//allow inline css
            httpContext.Response.Headers.Add("Content-Security-Policy", "style-src 'self'"+
                "https://stackpath.bootstrapcdn.com" +//bootstrap is usable
                "frame-ancestors ''none");//no iframes

            //httpContext.Response.Headers.Add("X-Frame-Options","ALLOW-FROM 'https://example.com'");
            //httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            httpContext.Response.Headers.Add("Feature-Policy", "camera 'none'");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");//turnoff the MIME type sniffing to prevent XSS
            httpContext.Response.Headers.Add("Content-Security-Policy", "require-sri-for 'script' 'style'");//use integrity has for cdn links in css and script
            httpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SeurityHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseSeurityHeadersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}
