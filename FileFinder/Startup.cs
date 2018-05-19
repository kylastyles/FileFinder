using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FileFinder.Data;

namespace FileFinder
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
            services.AddMvc();
            services.AddMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".RemindMe";
            });

            //services.AddRazorPagesOptions(options =>
            //{
            //    options.Conventions.AllowAnonymousToPage("/Home/Login");
            //    options.Conventions.AllowAnonymousToPage("/Home/Register");
            //    options.Conventions.AuthorizeFolder("/Buildings");
            //    options.Conventions.AuthorizeFolder("/CaseManagers");
            //    options.Conventions.AuthorizeFolder("/Consumers");
            //    options.Conventions.AuthorizeFolder("/Files");
            //    options.Conventions.AuthorizeFolder("/Programs");
            //    options.Conventions.AuthorizeFolder("/Rooms");
            //    options.Conventions.AuthorizeFolder("/Search");
            //});

            var connection = @"Server=(localdb)\mssqllocaldb;Database=FileFinder;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<FileFinderContext>(options => options.UseSqlServer(connection));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
