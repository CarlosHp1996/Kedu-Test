using MediatR;
using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Application.Services.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro.Handlers;

public class CreateResponsavelFinanceiroHandler : IRequestHandler<CreateResponsavelFinanceiroCommand, Result<ResponsavelFinanceiroResponse>>
{
    private readonly IResponsavelFinanceiroService _service;
    private readonly IMapper _mapper;

    public CreateResponsavelFinanceiroHandler(IResponsavelFinanceiroService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<Result<ResponsavelFinanceiroResponse>> Handle(CreateResponsavelFinanceiroCommand request, CancellationToken cancellationToken)
    {
        var responsavel = new Domain.Entities.ResponsavelFinanceiro
        {
            Nome = request.Nome
        };

        var result = await _service.CreateResponsavelFinanceiroAsync(responsavel);
        
        if (!result.HasSuccess)
        {
            return new Result<ResponsavelFinanceiroResponse>
            {
                HasSuccess = result.HasSuccess,
                Errors = result.Errors,
                HttpStatusCode = result.HttpStatusCode,
                Message = result.Message
            };
        }

        var response = _mapper.Map<ResponsavelFinanceiroResponse>(result.Value);
        
        return new Result<ResponsavelFinanceiroResponse>(response)
        {
            Message = result.Message
        };
    }
}