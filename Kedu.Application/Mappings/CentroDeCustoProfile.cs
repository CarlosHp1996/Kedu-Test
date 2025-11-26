using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;

namespace Kedu.Application.Mappings;

public class CentroDeCustoProfile : Profile
{
    public CentroDeCustoProfile()
    {
        // Request -> Entity
        CreateMap<CentroDeCustoRequest, CentroDeCusto>();

        // Entity -> Response
        CreateMap<CentroDeCusto, CentroDeCustoResponse>()
            .ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => GetTipoDescricao(src.Tipo)));
    }

    private static string GetTipoDescricao(TipoCentroDeCusto tipo)
    {
        return tipo switch
        {
            TipoCentroDeCusto.Matricula => "Matrícula",
            TipoCentroDeCusto.Mensalidade => "Mensalidade",
            TipoCentroDeCusto.Material => "Material Escolar",
            _ => tipo.ToString()
        };
    }
}