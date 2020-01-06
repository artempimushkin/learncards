using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("RDSConnection")));
            //services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddTransient<IAccessService, AccessService>();
            services.AddTransient<IAnalyticsService, AnalyticsService>();
            services.AddTransient<ICardService, CardService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IDeckService, DeckService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ISpeachService, AmazonPollyService>();
            services.AddTransient<IUserService, UserService>();

            //services.AddOptions<AmazonPollyService>(configuration);
            //services.AddScoped<AmazonPollyService>(provider => provider. //(/*provider => provider.GetService<ApplicationDbContext>()*/);

            var configurationSection = configuration.GetSection("AWS_Polly");
            services.Configure<AWSPollyOptions>(options =>
            {
                options.awsAccessKeyId = configurationSection.GetValue<string>("awsAccessKeyId");
                options.awsSecretAccessKey = configurationSection.GetValue<string>("awsSecretAccessKey");
            });

            return services;
        }
    }
}
