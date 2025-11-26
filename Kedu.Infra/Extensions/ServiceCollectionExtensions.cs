using Kedu.Application.Interfaces;
using Kedu.Infra.Data;
using Kedu.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kedu.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        // Register repositories
        services.AddScoped<IResponsavelFinanceiroRepository, ResponsavelFinanceiroRepository>();
        services.AddScoped<IPlanoDePagamentoRepository, PlanoDePagamentoRepository>();
        services.AddScoped<ICobrancaRepository, CobrancaRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<ICentroDeCustoRepository, CentroDeCustoRepository>();

        return services;
    }
}
