using MediatR;
using AutoMapper;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.CentroDeCusto.Handlers;

public class UpdateCentroDeCustoHandler : IRequestHandler<UpdateCentroDeCustoCommand, Result<CentroDeCustoResponse>>
{
    private readonly ICentroDeCustoRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCentroDeCustoHandler(ICentroDeCustoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CentroDeCustoResponse>> Handle(UpdateCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var centroDeCusto = await _repository.GetByIdAsync(request.Id);
            if (centroDeCusto == null)
            {
                var notFoundResult = new Result<CentroDeCustoResponse>();
                notFoundResult.WithNotFound($"Id {request.Id} do Plano De Pagamento não encontrado");
                return notFoundResult;
            }

            // Validar se o novo nome já existe (exceto o próprio registro)
            var exists = await _repository.ExistsByNomeAsync(request.Nome);
            if (exists && centroDeCusto.Nome != request.Nome)
            {
                var validationResult = new Result<CentroDeCustoResponse>();
                validationResult.WithError($"Centro de custo com nome '{request.Nome}' já existe");
                return validationResult;
            }

            centroDeCusto.Nome = request.Nome;
            centroDeCusto.Tipo = request.Tipo;
            await _repository.UpdateAsync(centroDeCusto);

            var response = _mapper.Map<CentroDeCustoResponse>(centroDeCusto);
            return new Result<CentroDeCustoResponse>(response)
            {
                Message = "Centro de custo atualizado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CentroDeCustoResponse>();
            result.WithException($"Erro ao atualizar centro de custo: {ex.Message}");
            return result;
        }
    }
}