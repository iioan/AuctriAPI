using System.Text;
using AuctriAPI.Application.Common.Interfaces.Authentication;
using AuctriAPI.Application.Common.Interfaces.Persistence;
using AuctriAPI.Application.Common.Interfaces.Security;
using AuctriAPI.Application.Services.DateTime;
using AuctriAPI.Core.Entitites;
using AuctriAPI.Infrastructure.Authentication;
using AuctriAPI.Infrastructure.Persistence;
using AuctriAPI.Infrastructure.Repository;
using AuctriAPI.Infrastructure.Security;
using AuctriAPI.Infrastructure.Services.DateTime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuctriAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        // Database
        services.AddDbContext<AuctriDbContext>(options =>
            options.UseSqlite(
                builderConfiguration.GetConnectionString("DBConnection"),
                b => b.MigrationsAssembly(typeof(AuctriDbContext).Assembly.FullName)));

        // Identity
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                // Configure password requirements
                // options.Password.RequiredLength = 8;
                // options.Password.RequireDigit = true;
                // options.Password.RequireLowercase = true;
                // options.Password.RequireUppercase = true;
                // options.Password.RequireNonAlphanumeric = true;

                // Configure user requirements
                options.User.RequireUniqueEmail = true;

                // Configure lockout
                // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                // options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AuctriDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<User>>();

        // Authentication and Token Generation
        services.AddAuth(builderConfiguration);

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.Configure<JwtSettings>(builderConfiguration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.Configure<JwtSettings>(builderConfiguration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builderConfiguration.GetSection(JwtSettings.SectionName)["Issuer"],
                ValidAudience = builderConfiguration.GetSection(JwtSettings.SectionName)["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builderConfiguration.GetSection(JwtSettings.SectionName)["Secret"]!))
            });

        return services;
    }
}