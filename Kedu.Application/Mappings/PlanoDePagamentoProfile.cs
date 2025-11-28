using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;

namespace Kedu.Application.Mappings;

public class PlanoDePagamentoProfile : Profile
{
    public PlanoDePagamentoProfile()
    {
        // Request -> Entity
        CreateMap<CreatePlanoDePagamentoRequest, PlanoDePagamento>()
            .ForMember(dest => dest.ResponsavelFinanceiroId, opt => opt.MapFrom(src => src.ResponsavelId))
            .ForMember(dest => dest.CentroDeCustoId, opt => opt.MapFrom(src => src.CentroDeCusto));

        CreateMap<UpdatePlanoDePagamentoRequest, PlanoDePagamento>();

        // Entity -> Response
        CreateMap<PlanoDePagamento, PlanoDePagamentoResponse>()
            .ForMember(dest => dest.ResponsavelFinanceiroNome, opt => opt.MapFrom(src => src.ResponsavelFinanceiro.Nome))
            .ForMember(dest => dest.CentroDeCustoNome, opt => opt.MapFrom(src => src.CentroDeCusto.Nome))
            .ForMember(dest => dest.QuantidadeCobrancas, opt => opt.MapFrom(src => src.Cobrancas.Count));

        CreateMap<PlanoDePagamento, PlanoDePagamentoDetailResponse>()
            .ForMember(dest => dest.QuantidadeCobrancas, opt => opt.MapFrom(src => src.Cobrancas.Count));

        CreateMap<PlanoDePagamento, PlanoDePagamentoSummaryResponse>()
            .ForMember(dest => dest.CentroDeCustoNome, opt => opt.MapFrom(src => src.CentroDeCusto.Nome))
            .ForMember(dest => dest.QuantidadeCobrancas, opt => opt.MapFrom(src => src.Cobrancas.Count));
    }
}