using CarCrash.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarCrash
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
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(400);
            });

            services.AddDbContext<CrashDbContext>(options =>
            {
                options.UseMySql(Environment.GetEnvironmentVariable("CrashDbString"));
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseMySql(Environment.GetEnvironmentVariable("IdentityDbString")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole",
                     policy => policy.RequireRole("Admin"));
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<ICarCrashRepository, EFCarCrashRepository>();

            services.Configure<IdentityOptions>(options =>
            {

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 3;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            //services.ConfigureApplicationCookie(options =>
            //{
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.Cookie.Name = "YourAppCookieName";
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                //options.SlidingExpiration = true;
            //});

            services.AddSingleton<InferenceSession>(
              new InferenceSession("wwwroot/crash.onnx")
            );

            //services.AddDbContext<RoadDbContext>(options =>
            //{
            //    options.UseMySql(Configuration["ConnectionStrings:CrashesDbConnection"]);
            //});
            //services.AddDbContext<RoadDbContext>(options =>
            //{
            //    options.UseMySql(Configuration["ConnectionStrings:CrashesDbConnection"]);
            //});
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("~/error");

            //This is the CSP header stuff. If anything is broken comment this out to debug.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; style-src 'self' 'unsafe-inline'; style-src-elem * 'unsafe-inline'; script-src 'self' maps.googleapis.com; script-src-elem * 'unsafe-inline'; connect-src https://maps.googleapis.com/; frame-src 'self' https://public.tableau.com/; font-src *; img-src 'self' https://*.googleapis.com https://*.gstatic.com *.google.com  *.googleusercontent.com https://public.tableau.com/ data:");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Paging", //We can give whatever name we want to the endpoint
                    pattern: "{pageNum}", //Go see and check if it matches the number of pageNum, it also once it finds that pattern will then just write the number instead of pageNum=2
                                          //We can also add something to the pattern where if it doesn't find the pattern, it will print it in the url...
                                          // like "Page{pageNum}
                    defaults: new { Controller = "Home", action = "Data", pageNum = 1 } //We don't have to include the "defaults: " part
                    );
                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
