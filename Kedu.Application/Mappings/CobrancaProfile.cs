using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;

namespace Kedu.Application.Mappings;

public class CobrancaProfile : Profile
{
    public CobrancaProfile()
    {
        // Request -> Entity
        CreateMap<CobrancaRequest, Cobranca>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StatusCobranca.Emitida));

        // Entity -> Response
        CreateMap<Cobranca, CobrancaResponse>()
            .ForMember(dest => dest.MetodoPagamentoDescricao, opt => opt.MapFrom(src => GetMetodoPagamentoDescricao(src.MetodoPagamento)))
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => GetStatusDescricao(src.Status)))
            .ForMember(dest => dest.QuantidadePagamentos, opt => opt.MapFrom(src => src.Pagamentos.Count));

        CreateMap<Cobranca, CobrancaDetailResponse>()
            .ForMember(dest => dest.MetodoPagamentoDescricao, opt => opt.MapFrom(src => GetMetodoPagamentoDescricao(src.MetodoPagamento)))
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => GetStatusDescricao(src.Status)));
    }

    private static string GetMetodoPagamentoDescricao(MetodoPagamento metodo)
    {
        return metodo switch
        {
            MetodoPagamento.Boleto => "Boleto Bancário",
            MetodoPagamento.Pix => "PIX",
            _ => metodo.ToString()
        };
    }

    private static string GetStatusDescricao(StatusCobranca status)
    {
        return status switch
        {
            StatusCobranca.Emitida => "Emitida",
            StatusCobranca.Paga => "Paga",
            StatusCobranca.Cancelada => "Cancelada",
            _ => status.ToString()
        };
    }
}