
using Formula1Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Formula1Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Створення логера для запису у файли
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Налаштування EmailSender з попередньої роботи
            var emailSettings = Configuration
                .GetSection("EmailSettings")
                .Get<EmailSettings>();

            services.AddSingleton(emailSettings!);
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddControllersWithViews();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Логування кожного HTTP-запиту
            app.Use(async (context, next) =>
            {
                var request = context.Request;

                string fullUrl =
                    $"{request.Scheme}://{request.Host}" +
                    $"{request.PathBase}{request.Path}{request.QueryString}";

                string ipAddress =
                    context.Connection.RemoteIpAddress?.ToString()
                    ?? "Unknown";

                DateTime requestTime = DateTime.Now;

                Log.Information(
                    "URL: {Url} | Time: {Time} | IP: {IP}",
                    fullUrl,
                    requestTime,
                    ipAddress);

                await next();
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
