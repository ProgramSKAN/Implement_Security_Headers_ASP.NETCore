using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImplementSecurityHeaders.HeaderHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImplementSecurityHeaders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //SECURITY HEADERS
            //services.AddControllersWithViews(c =>
            //{
            //    c.Filters.Add(new SecurityHeadersAttribute());
            //});
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.Use(async(context,next)=> {
            //    context.Response.Headers.Add("HeaderType", "HeaderValue");
            //    await next();
            //});
            //OR
            app.UseSeurityHeadersMiddleware();
            //OR
            //USE WEBCONFIG, so these headers are configured by IIS
            //OR
            //can use more symantic APIs instead of using strings
            //NWebSec.AspNetCore.Middleware nuget package
            //app.UseCsp()
            /*
             "Content-Security-Policy : style-src 'none'"//>prevent no css,no inline styles.  'none' is too strict

                 *       >allow all urls/allow default browser behaviour
                 self    >restrict the loading of css to the url your application is running on,called the origin

             */
            //and
            //app.UseCsp(options => options.DefaultSources(s => s.Self())
            //        .FrameAncestors(f=>f.None())
            //       .StyleSources(s => s.Self().CustomSources("https://stackpath.bootstrapcdn.com"))
            //       .ReportUris(r => r.Uris("/report")));
            //app.UseXContentTypeOptions();
            //and
            //use integrity hasing for cdn links in css
            //< link src = "https://stackpath.bootstrapcdn.com/" integrity = "sha384-OLBgp1GsljhM2TJ+sbHjaiH9txEUvgdDTAzHv2P24donTt6/529l+9Ua0vFImLlb" crossorigin = "anonymous" />
            //and
            //Preventing Leaking of URL Information with the Referrer-policy Header
            //when referer site runs on https then response has referer header has http link,
            //when referer site runs on http then response has no referer header, chrome by default uses a referer policy of no referer when downgrade.so whenever a site running with https links to a non secure http site the referer header is hidden.this rules out man in the middle collecting information.so we r sure ,only the site with this link gets the header,
            //and
            //app.UseReferrerPolicy(p => p.NoReferrer());
            //or<a href="https://localhost:5000" rel="noreferrer">go to loyality site</a>
            //and 
            //Preventing Caching with the Cache-Control Header
            //"Cache-Control":"no-cache, no-store"//in browser default
            //public  > its ok to cache the response and caches that are for the user only ,as well as shared caches
            //private > only allowed in caches that are private to the user
            //no-cache>whenever the data from the cache is used, the server must first be checked if there is a new version
            //no-store>turns off caching compltely
            //max-age,max-stale,...


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
