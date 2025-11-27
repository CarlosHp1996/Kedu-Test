using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Kedu.Application.Mappings;
using Kedu.Application.Services;
using Kedu.Application.Services.Interfaces;
using FluentValidation;
using Kedu.Application.Validators;
using Kedu.Application.Commands.ResponsavelFinanceiro;
using Kedu.Application.Commands.PlanoDePagamento;
using Kedu.Application.Commands.Pagamento;
using Kedu.Application.Commands.Cobranca;
using Kedu.Application.Commands.CentroDeCusto;

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

        // Business Services
        services.AddScoped<IResponsavelFinanceiroService, ResponsavelFinanceiroService>();
        services.AddScoped<IPlanoDePagamentoService, PlanoDePagamentoService>();
        services.AddScoped<ICobrancaService, CobrancaService>();
        services.AddScoped<IPaymentCodeGenerator, PaymentCodeGenerator>();
       
        // ResponsavelFinanceiro Validators
        services.AddScoped<IValidator<CreateResponsavelFinanceiroCommand>, CreateResponsavelFinanceiroCommandValidator>();
        services.AddScoped<IValidator<UpdateResponsavelFinanceiroCommand>, UpdateResponsavelFinanceiroCommandValidator>();
        services.AddScoped<IValidator<DeleteResponsavelFinanceiroCommand>, DeleteResponsavelFinanceiroCommandValidator>();
        
        // PlanoDePagamento Validators
        services.AddScoped<IValidator<CreatePlanoDePagamentoCommand>, CreatePlanoDePagamentoCommandValidator>();
        services.AddScoped<IValidator<UpdatePlanoDePagamentoCommand>, UpdatePlanoDePagamentoCommandValidator>();
        services.AddScoped<IValidator<DeletePlanoDePagamentoCommand>, DeletePlanoDePagamentoCommandValidator>();
        
        // Pagamento Validators
        services.AddScoped<IValidator<CreatePagamentoCommand>, CreatePagamentoBusinessRuleValidator>();
        
        // Cobranca Validators
        services.AddScoped<IValidator<UpdateCobrancaCommand>, UpdateCobrancaCommandValidator>();
        services.AddScoped<IValidator<CancelCobrancaCommand>, CancelCobrancaCommandValidator>();
        services.AddScoped<IValidator<DeleteCobrancaCommand>, DeleteCobrancaCommandValidator>();
        
        // CentroDeCusto Validators
        services.AddScoped<IValidator<CreateCentroDeCustoCommand>, CreateCentroDeCustoCommandValidator>();
        services.AddScoped<IValidator<UpdateCentroDeCustoCommand>, UpdateCentroDeCustoCommandValidator>();
        services.AddScoped<IValidator<DeleteCentroDeCustoCommand>, DeleteCentroDeCustoCommandValidator>();

        return services;
    }
}