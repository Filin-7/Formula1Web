
using Formula1Web.Services;
using Formula1Web.Models;
using Formula1Web.Validators;

using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.Globalization;

namespace Formula1Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Логування у файли
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // =====================================
            // Налаштування EmailSender
            // =====================================

            var emailSettings = Configuration
                .GetSection("EmailSettings")
                .Get<EmailSettings>();

            services.AddSingleton(emailSettings!);

            services.AddTransient<IEmailSender, EmailSender>();


            // =====================================
            // Реєстрація локалізації UA / EN
            // =====================================

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });


            // =====================================
            // Реєстрація MVC
            // =====================================

            services.AddControllersWithViews();


            // =====================================
            // Реєстрація FluentValidation
            // =====================================

            services.AddScoped<
                IValidator<DriverRegistration>,
                DriverRegistrationValidator>();

        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            // =====================================
            // Обробка помилок
            // =====================================

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            // =====================================
            // Підтримувані мови
            // =====================================

            var supportedCultures = new[]
            {
                new CultureInfo("uk"),
                new CultureInfo("en")
            };


            // =====================================
            // Налаштування локалізації
            // =====================================

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uk"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };


            // Визначення мови через cookie
            localizationOptions.RequestCultureProviders =
                new IRequestCultureProvider[]
                {
                    new CookieRequestCultureProvider()
                };


            // Увімкнення локалізації
            app.UseRequestLocalization(localizationOptions);


            // =====================================
            // HTTPS
            // =====================================

            app.UseHttpsRedirection();


            // =====================================
            // Логування HTTP-запитів
            // =====================================

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


            // =====================================
            // Статичні файли
            // =====================================

            app.UseStaticFiles();


            // =====================================
            // Маршрутизація
            // =====================================

            app.UseRouting();

            app.UseAuthorization();


            // =====================================
            // MVC маршрути
            // =====================================

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
