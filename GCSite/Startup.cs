using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCSite.Data;
using Microsoft.AspNetCore.Identity;
using GCSite.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;

namespace GCSite
{
    public class Startup
    {
        IGDBAPIHelper igdbAPI;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            igdbAPI = new IGDBAPIHelper();
            IGDBAPIHelper.InitializeClient();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_MYSQLCONNECT");
            //string test = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".glb"] = "model/gltf-binary";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });     
        }
    }
}
