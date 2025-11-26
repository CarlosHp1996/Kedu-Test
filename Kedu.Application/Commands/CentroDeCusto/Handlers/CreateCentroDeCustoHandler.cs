using MediatR;
using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.CentroDeCusto.Handlers;

public class CreateCentroDeCustoHandler : IRequestHandler<CreateCentroDeCustoCommand, Result<CentroDeCustoResponse>>
{
    private readonly ICentroDeCustoRepository _repository;
    private readonly IMapper _mapper;

    public CreateCentroDeCustoHandler(ICentroDeCustoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CentroDeCustoResponse>> Handle(CreateCentroDeCustoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validar se já existe centro de custo com o mesmo nome
            var exists = await _repository.ExistsByNomeAsync(request.Nome);
            if (exists)
            {
                var validationResult = new Result<CentroDeCustoResponse>();
                validationResult.WithError($"Centro de custo com nome '{request.Nome}' já existe");
                return validationResult;
            }

            var centroDeCusto = new Domain.Entities.CentroDeCusto
            {
                Nome = request.Nome,
                Tipo = request.Tipo
            };

            var createdCentroDeCusto = await _repository.AddAsync(centroDeCusto);
            var response = _mapper.Map<CentroDeCustoResponse>(createdCentroDeCusto);
            
            return new Result<CentroDeCustoResponse>(response)
            {
                Message = "Centro de custo criado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<CentroDeCustoResponse>();
            result.WithException($"Erro ao criar centro de custo: {ex.Message}");
            return result;
        }
    }
}