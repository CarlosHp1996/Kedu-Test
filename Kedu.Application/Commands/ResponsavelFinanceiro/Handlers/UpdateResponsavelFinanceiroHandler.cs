using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.ResponsavelFinanceiro.Handlers;

public class UpdateResponsavelFinanceiroHandler : IRequestHandler<UpdateResponsavelFinanceiroCommand, Result<ResponsavelFinanceiroResponse>>
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IMapper _mapper;

    public UpdateResponsavelFinanceiroHandler(IResponsavelFinanceiroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ResponsavelFinanceiroResponse>> Handle(UpdateResponsavelFinanceiroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var responsavel = await _repository.GetByIdAsync(request.Id);
            if (responsavel == null)
            {
                var notFoundResult = new Result<ResponsavelFinanceiroResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Responsável Financeiro não encontrado");
                return notFoundResult;
            }

            responsavel.Nome = request.Nome;
            await _repository.UpdateAsync(responsavel);

            var response = _mapper.Map<ResponsavelFinanceiroResponse>(responsavel);
            return new Result<ResponsavelFinanceiroResponse>(response)
            {
                Message = "Responsável financeiro atualizado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<ResponsavelFinanceiroResponse>();
            result.WithException($"Erro ao atualizar responsável financeiro: {ex.Message}");
            return result;
        }
    }
}