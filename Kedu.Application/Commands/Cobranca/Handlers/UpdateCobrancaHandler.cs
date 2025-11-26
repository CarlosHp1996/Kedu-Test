using MediatR;
using AutoMapper;
using Kedu.Domain.Enums;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca.Handlers;

public class UpdateCobrancaHandler : IRequestHandler<UpdateCobrancaCommand, Result<CobrancaResponse>>
{
    private readonly ICobrancaRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCobrancaHandler(ICobrancaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CobrancaResponse>> Handle(UpdateCobrancaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cobranca = await _repository.GetByIdAsync(request.Id);
            if (cobranca == null)
            {
                var notFoundResult = new Result<CobrancaResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} da Cobrança  não encontrado");
                return notFoundResult;
            }

            // Não permitir atualização se estiver paga ou cancelada
            if (cobranca.Status == StatusCobranca.Paga || cobranca.Status == StatusCobranca.Cancelada)
            {
                var validationResult = new Result<CobrancaResponse>();
                validationResult.WithError("Não é possível alterar cobrança paga ou cancelada");
                return validationResult;
            }

            var originalMethod = cobranca.MetodoPagamento;
            cobranca.Valor = request.Valor;
            cobranca.DataVencimento = request.DataVencimento;
            cobranca.MetodoPagamento = request.MetodoPagamento;

            // Regenerar código de pagamento se método mudou
            if (originalMethod != request.MetodoPagamento)
            {
                cobranca.GeneratePaymentCode();
            }

            await _repository.UpdateAsync(cobranca);
            
            var response = _mapper.Map<CobrancaResponse>(cobranca);
            return new Result<CobrancaResponse>(response)
            {
                Message = "Cobrança atualizada com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CobrancaResponse>();
            result.WithException($"Erro ao atualizar cobrança: {ex.Message}");
            return result;
        }
    }
}