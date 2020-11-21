using Messenger.Api.WebSocketsHandlers;
using Messenger.Database;
using Messenger.EmailSending;
using Messenger.Facade.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace Messenger.Api
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
            services.AddCors();

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            });

            string dbConnectString = Configuration.GetConnectionString("MessengerConnectionString");

            services.AddDbContext<MessengerDbContext>(options =>
            {
                options.UseMySql(dbConnectString, b => b.ServerVersion("8.0.20-mysql"));
            });

            //Authentication with JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.Configure<AppSettings>(options =>
            {
                options.WebAppUrl = Configuration["AppSettings:WebAppUrl"];

            });

            services.Configure<EmailSenderSettings>(options =>
            {
                options.ApiKey = Configuration["EmailSending:SendGrid:ApiKey"];
                options.SenderEmail = Configuration["EmailSending:SendGrid:SenderEmail"];
                options.SenderName = Configuration["EmailSending:SendGrid:SenderName"];
            });

            services.AddTransient<IEmailSender, SendGridEmailSender>();

            //Add dependency injection for JWT Settings from appsettings
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            //Messenger utility class for depedency injection
            MessengerRegister.Configuration(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Do all migrations to the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<MessengerDbContext>();

                dbContext.Database.Migrate();
            }

            app.UseCors(
              options =>
              {
                  options.AllowAnyHeader();
                  options.AllowAnyMethod();
                  options.AllowAnyOrigin();
              }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<WebSocketAuthenticationMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<ChatWebSocketHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
