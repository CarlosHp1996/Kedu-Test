using MediatR;
using AutoMapper;
using Kedu.Domain.Enums;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.Cobranca.Handlers;

public class CancelCobrancaHandler : IRequestHandler<CancelCobrancaCommand, Result<CobrancaResponse>>
{
    private readonly ICobrancaRepository _repository;
    private readonly IMapper _mapper;

    public CancelCobrancaHandler(ICobrancaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CobrancaResponse>> Handle(CancelCobrancaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cobranca = await _repository.GetByIdAsync(request.Id);
            if (cobranca == null)
            {
                var notFoundResult = new Result<CobrancaResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} da Cobrança não encontrado");
                return notFoundResult;
            }

            if (cobranca.Status == StatusCobranca.Paga)
            {
                var validationResult = new Result<CobrancaResponse>();
                validationResult.WithError("Não é possível cancelar cobrança já paga");
                return validationResult;
            }

            if (cobranca.Status == StatusCobranca.Cancelada)
            {
                var alreadyCancelledResult = new Result<CobrancaResponse>();
                alreadyCancelledResult.WithError("Cobrança já está cancelada");
                return alreadyCancelledResult;
            }

            cobranca.Status = StatusCobranca.Cancelada;
            await _repository.UpdateAsync(cobranca);

            var response = _mapper.Map<CobrancaResponse>(cobranca);
            return new Result<CobrancaResponse>(response)
            {
                Message = $"Cobrança cancelada com sucesso. Motivo: {request.Motivo}"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CobrancaResponse>();
            result.WithException($"Erro ao cancelar cobrança: {ex.Message}");
            return result;
        }
    }
}