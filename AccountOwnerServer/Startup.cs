using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.Extensions.FileProviders;
using SpotOnAccountServer.Models;

namespace AccountOwnerServer
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
            services.ConfigureCors();

            services.ConfigureIISIntegration();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddMvcCore(options =>
                {
                    options.RequireHttpsPermanent = true; // does not affect api requests
                    options.RespectBrowserAcceptHeader = true; // false by default 
                }) 
                .AddFormatterMappings() 
                .AddJsonFormatters(); // JSON, or you can build your own custom one (above)



            var connection = @"Server=SQL6010.site4now.net;Database=DB_A5DE44_medicall;User Id=DB_A5DE44_medicall_admin;Password=Rexonery@1;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DB_A5DE44_RussellContext>(options => options.UseSqlServer(connection));


            // configure jwt authentication
            var appSettings = "RHAJADEJ0WEKZHY007AMEN";
            var key = Encoding.ASCII.GetBytes(appSettings);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole",
                     policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireHealthWorkerRole",
                     policy => policy.RequireRole("HealthWorker"));
                options.AddPolicy("RequireUserRole",
                     policy => policy.RequireRole("User"));
            });
            services.AddDefaultIdentity<IdentityUser>()
        .AddRoles<IdentityRole>();

            // configure DI for application services
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IMailService, MailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            //app.UseCors("CorsPolicy");
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseStaticFiles(); 
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")),
                RequestPath = "/Images"
            });

            app.UseMvc();
            
        }
    }
}
