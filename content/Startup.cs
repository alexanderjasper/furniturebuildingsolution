using AutoMapper;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using FurnitureBuildingSolution.Repositories;
using FurnitureBuildingSolution.Database;
using System;
using RazorLight;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace FurnitureBuildingSolution
{
    public class Startup
    {
        private readonly IHostingEnvironment _currentEnvironment;

        public Startup(IHostingEnvironment env)
        {
            Configuration = AppSettings.GetConfiguration(env.ContentRootPath, env.EnvironmentName);
            _currentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new Exception("services variable is null");
            }

            services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();

            services.AddScoped<IRazorLightEngine>(sp =>
            {
                var engine = new RazorLightEngineBuilder()
                .UseFilesystemProject(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .UseMemoryCachingProvider()
                .Build();
                return engine;
            });

            // configure strongly typed settings objects
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            var appSettings = AppSettings.GetSettings(Configuration);

            // configure DI for application services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookcaseRepository, BookcaseRepository>();
            services.AddScoped<IBookcaseService, BookcaseService>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISkycivService, SkycivService>();
            services.AddScoped<IBookcaseMapper, BookcaseMapper>();
            services.AddScoped<IOrderMapper, OrderMapper>();
            services.AddScoped<IProductService, ProductService>();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(appSettings.DatabaseSettings.GetConnectionString()));

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            if (_currentEnvironment.IsProduction())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            loggerFactory.AddFile("Logs/shelfer-{Date}.log");

            var options = new RewriteOptions()
                .AddRedirect("^bookcase-designer$", "reoldesigner", 301)
                .AddRedirect("^shopping-cart$", "indkoebskurv", 301)
                .AddRedirect("^specifications$", "specifikationer", 301)
                .AddRedirect("^my-bookcases$", "mine-reoler", 301)
                .AddRedirect("^orders$", "ordrer", 301)
                .AddRedirect("^contact-us$", "kontakt-os", 301)
                .AddRedirect("^about-us$", "om-os", 301)
                .AddRedirect("^login$", "log-ind", 301)
                .AddRedirect("^register$", "opret-konto", 301)
                .AddRedirect("^order-placed$", "ordre-bekraeftet", 301)
                .AddRedirect("^order-confirmation/(.*)$", "ordrebekraeftelse/$1", 301)
                .AddRedirect("^forgot-password$", "glemt-adgangskode", 301)
                .AddRedirect("^password-change/(.*)$", "skift-adgangskode/$1", 301);
            app.UseRewriter(options);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
