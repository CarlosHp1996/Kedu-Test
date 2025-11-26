using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Kedu.Application.Mappings;

namespace Kedu.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper manual configuration
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ResponsavelFinanceiroProfile());
            mc.AddProfile(new CentroDeCustoProfile());
            mc.AddProfile(new PlanoDePagamentoProfile());
            mc.AddProfile(new CobrancaProfile());
            mc.AddProfile(new PagamentoProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}