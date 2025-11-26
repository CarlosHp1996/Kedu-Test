using MediatR;
using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro.Handlers;

public class CreateResponsavelFinanceiroHandler : IRequestHandler<CreateResponsavelFinanceiroCommand, Result<ResponsavelFinanceiroResponse>>
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IMapper _mapper;

    public CreateResponsavelFinanceiroHandler(IResponsavelFinanceiroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ResponsavelFinanceiroResponse>> Handle(CreateResponsavelFinanceiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var responsavel = new Domain.Entities.ResponsavelFinanceiro
            {
                Nome = request.Nome
            };

            var createdResponsavel = await _repository.AddAsync(responsavel);
            var response = _mapper.Map<ResponsavelFinanceiroResponse>(createdResponsavel);
            
            return new Result<ResponsavelFinanceiroResponse>(response)
            {
                Message = "Responsável financeiro criado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<ResponsavelFinanceiroResponse>();
            result.WithException($"Erro ao criar responsável financeiro: {ex.Message}");
            return result;
        }
    }
}