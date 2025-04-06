using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace AuctriAPI.Application; 
    
public static class DependencyInjection
{   
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}

