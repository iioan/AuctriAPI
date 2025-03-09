using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AuctriAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure (this IServiceCollection services,
        IConvertible Configuration)
    {
        return services;
    }
}
