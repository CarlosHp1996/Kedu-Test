using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;

namespace Kedu.Application.Mappings;

public class ResponsavelFinanceiroProfile : Profile
{
    public ResponsavelFinanceiroProfile()
    {
        // Request -> Entity
        CreateMap<ResponsavelFinanceiroRequest, ResponsavelFinanceiro>();

        // Entity -> Response
        CreateMap<ResponsavelFinanceiro, ResponsavelFinanceiroResponse>();
        
        CreateMap<ResponsavelFinanceiro, ResponsavelFinanceiroDetailResponse>()
            .ForMember(dest => dest.TotalPlanos, opt => opt.MapFrom(src => src.PlanosDePagamento.Count))
            .ForMember(dest => dest.ValorTotalPlanos, opt => opt.MapFrom(src => 
                src.PlanosDePagamento.Sum(p => p.ValorTotal)));
    }
}