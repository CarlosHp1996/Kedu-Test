using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;

namespace Kedu.Application.Mappings;

public class PagamentoProfile : Profile
{
    public PagamentoProfile()
    {
        // Request -> Entity
        CreateMap<CreatePagamentoRequest, Pagamento>()
            .ForMember(dest => dest.DataPagamento, opt => opt.MapFrom(src => 
                src.DataPagamento ?? DateTime.Now));

        // Entity -> Response
        CreateMap<Pagamento, PagamentoResponse>();
        CreateMap<Pagamento, PagamentoDetailResponse>();
    }
}