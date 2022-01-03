using System.Linq;
using CMS_NetCore.DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using CMS_NetCore.Helpers;
using CMS_NetCore.Web.Configs.Extentions;
using CMS_NetCore.Web.Configs.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;



namespace CMS_NetCore.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }
        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration , IWebHostEnvironment environment , ILogger<Startup> logger)
        {
            Configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages(
                options =>
                {
                    options.Conventions.AllowAnonymousToAreaPage("Admin", "/Default/Index");
                    //options.Conventions.area(new );
                });
             services.AddAuthorization();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
             services.AddOurAuthentication(appSettings);

            services.AddOurSwagger();
            services.AddMvc(options =>
            {
                //options.Conventions.Add(
                //        new AuthorizeAreaConvention("Admin", "AdministratorPolicy"));
            });//.AddJsonOptions(options =>options.JsonSerializerOptions. = new DefaultContractResolver()).AddSessionStateTempDataProvider();

            services.AddSession();

            var connection = @"Data Source=.;Initial Catalog=CMS_NetCoreDb;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(connection, b => b.MigrationsAssembly("CMS_NetCore.Web")));

            //// configure DI for application services
            services.AddOurDIConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                 name: "admin",
                 areaName: "admin",
                 pattern: "Admin/{controller=Default}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                    "default", "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseStatusCodePages(async context =>
            {
                var response =  context.HttpContext.Response;

                if (response.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)System.Net.HttpStatusCode.Forbidden)
                    response.Redirect("/Account/Login");
            });

            app.UseSession();
            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });


            app.UseCookiePolicy();
            app.UseMiddleware<AuthorizationHeader>();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

        }
    }
}
