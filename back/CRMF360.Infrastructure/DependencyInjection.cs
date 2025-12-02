using CRMF360.Application.Abstractions;
using CRMF360.Application.Users;
using CRMF360.Infrastructure.Persistence;
using CRMF360.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRMF360.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("pg")
            ?? throw new InvalidOperationException("Connection string 'pg' not found.");
    Console.WriteLine($"[DEBUG] ConnectionString pg = {connectionString}");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
