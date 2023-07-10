using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WalletWatch.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WalletWatch.Domain.Entities.UserAggregate;
using Microsoft.Extensions.Logging;
using WalletWatch.WebAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using HealthChecks.UI.Client;
using WalletWatch.WebAPI.HealthChecks;
using System.Linq;
using WalletWatch.EF.MySql;
using Microsoft.FeatureManagement;

namespace WalletWatch.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            
            Configuration = configuration;
        }

        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureAppConfiguration();
            services.AddFeatureManagement();

            //Ckeck environment of project
            if (_env.IsDevelopment())
            {
                services.AddDbContext<WalletWatchDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                // Add identity
                services.AddIdentity<User, IdentityRole>(config =>
                {
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireLowercase = false;
                })
                    .AddEntityFrameworkStores<WalletWatchDbContext>()
                    .AddDefaultTokenProviders();

            }
            else
            {
                services.AddDbContext<WalletWatchMySqlDbContext>(options =>
                options.UseMySql(Configuration["DefaultConnection"], new MySqlServerVersion(new Version(8, 0))));

                // Add identity
                services.AddIdentity<User, IdentityRole>(config =>
                {
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireLowercase = false;
                })
                    .AddEntityFrameworkStores<WalletWatchMySqlDbContext>()
                    .AddDefaultTokenProviders();
            }

            // Configure identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // Add logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Register the Swagger services
            services.AddVersionedApiExplorer(options =>
                options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            //Add API versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        
            // Add API controllers
            services.AddControllers();

            // Add health checks
            services.AddHealthChecks()
                //.AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
                .AddCheck<MemoryHealthCheck>(nameof(MemoryHealthCheck))
                .AddCheck<SqlHealthCheck>(nameof(SqlHealthCheck));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<RequestLoggingMiddleware>();
            }
            else
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var dbContext = services.GetRequiredService<WalletWatchMySqlDbContext>();
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }
            }
                
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    config.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    config.RoutePrefix = "swagger";
                }
            });

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAzureAppConfiguration();
            // Add authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseApiVersioning();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
