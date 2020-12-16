using Confluent.Kafka;
using Messenger.Api.Authorization;
using Messenger.Api.WebSocketsHandlers;
using Messenger.Database;
using Messenger.EmailSending;
using Messenger.Facade;
using Messenger.Facade.KafkaConfiguration;
using Messenger.Facade.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            services.AddHttpContextAccessor();

            // Authorization middleware to authorize only request from client with a valid API Key
            string apiKey = Configuration["ApiKey"];

            services.AddTransient<IAuthorizationHandler, ApiKeyRequirementHandler>();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("ApiKeyPolicy",
                    policyBuilder => policyBuilder
                        .AddRequirements(new ApiKeyRequirement(new[] { apiKey})));
            });

            // Configure controllers with newtonsoft json serializer
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            });

            // Get connection string from config file
            string dbConnectString = Configuration.GetConnectionString("MessengerConnectionString");

            // Configure database context
            services.AddDbContext<MessengerDbContext>(options =>
            {
                options.UseMySql(dbConnectString, b => b.ServerVersion("8.0.20-mysql"));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Messenger API",
                    Description = "An ASP.NET Core Web API to manage Messenger Data",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Christopher Lenfant",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            // Authentication with JWT
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

            // Add app settings
            services.Configure<AppSettings>(options =>
            {
                options.WebAppUrl = Configuration["AppSettings:WebAppUrl"];
            });

            // Add email sender settings
            services.Configure<EmailSenderSettings>(options =>
            {
                options.ApiKey = Configuration["EmailSending:SendGrid:ApiKey"];
                options.SenderEmail = Configuration["EmailSending:SendGrid:SenderEmail"];
                options.SenderName = Configuration["EmailSending:SendGrid:SenderName"];
            });

            // Add email templates settings
            services.Configure<EmailTemplatesSettings>(options =>
            {
                options.ActivateAccountId = Configuration["EmailSending:SendGridTemplates:ActivateAccountId"];
                options.ResetPasswordId = Configuration["EmailSending:SendGridTemplates:ResetPasswordId"];
            });

            // Add dependency injection for Email sender service
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            // Add dependency injection for JWT Settings from appsettings
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));


            // Add dependency injection for producter and consumer kafka config
            var producerConfig = new ProducerConfig();
            var consumerConfig = new ConsumerConfig();
            var webSocketStore = new WebSocketStore();

            Configuration.Bind("KafkaSettings:Producer", producerConfig);
            Configuration.Bind("KafkaSettings:Consumer", consumerConfig);

            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);
            services.AddSingleton<WebSocketStore>(webSocketStore);

            //Kafka consumer hosted service
            services.AddHostedService<KafkaConsumerHostedService>();

            // Messenger utility class for depedency injection
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


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
