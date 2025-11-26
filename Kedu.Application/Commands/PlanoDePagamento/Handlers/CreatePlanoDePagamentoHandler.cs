using MediatR;
using AutoMapper;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Application.Interfaces;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Commands.PlanoDePagamento.Handlers;

public class CreatePlanoDePagamentoHandler : IRequestHandler<CreatePlanoDePagamentoCommand, Result<PlanoDePagamentoResponse>>
{
    private readonly IPlanoDePagamentoRepository _planoDePagamentoRepository;
    private readonly IResponsavelFinanceiroRepository _responsavelRepository;
    private readonly ICentroDeCustoRepository _centroDeCustoRepository;
    private readonly IMapper _mapper;

    public CreatePlanoDePagamentoHandler(
        IPlanoDePagamentoRepository planoDePagamentoRepository,
        IResponsavelFinanceiroRepository responsavelRepository,
        ICentroDeCustoRepository centroDeCustoRepository,
        IMapper mapper)
    {
        _planoDePagamentoRepository = planoDePagamentoRepository;
        _responsavelRepository = responsavelRepository;
        _centroDeCustoRepository = centroDeCustoRepository;
        _mapper = mapper;
    }

    public async Task<Result<PlanoDePagamentoResponse>> Handle(CreatePlanoDePagamentoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validações
            var responsavel = await _responsavelRepository.GetByIdAsync(request.ResponsavelId);
            if (responsavel == null)
            {
                var responsavelNotFoundResult = new Result<PlanoDePagamentoResponse>();
                responsavelNotFoundResult.WithNotFound($"Id {request.ResponsavelId} do Responsavel Financeiro não encontrado");
                return responsavelNotFoundResult;
            }

            var centroDeCusto = await _centroDeCustoRepository.GetByIdAsync(request.CentroDeCusto);
            if (centroDeCusto == null)
            {
                var centroNotFoundResult = new Result<PlanoDePagamentoResponse>();
                centroNotFoundResult.WithNotFound($"Id {request.CentroDeCusto} do Centro De Custo não encontrado");
                return centroNotFoundResult;
            }

            if (request.Cobrancas == null || !request.Cobrancas.Any())
            {
                var validationResult = new Result<PlanoDePagamentoResponse>();
                validationResult.WithError("Plano de pagamento deve ter pelo menos uma cobrança");
                return validationResult;
            }

            // Criar plano
            var plano = new Domain.Entities.PlanoDePagamento
            {
                ResponsavelFinanceiroId = request.ResponsavelId,
                CentroDeCustoId = request.CentroDeCusto,
                ResponsavelFinanceiro = responsavel,
                CentroDeCusto = centroDeCusto,
                Cobrancas = request.Cobrancas.Select(c => new Domain.Entities.Cobranca
                {
                    Valor = c.Valor,
                    DataVencimento = c.DataVencimento,
                    MetodoPagamento = c.MetodoPagamento,
                    Status = StatusCobranca.Emitida,
                    CodigoPagamento = GeneratePaymentCode(c.MetodoPagamento)
                }).ToList()
            };

            var createdPlano = await _planoDePagamentoRepository.AddAsync(plano);
            var response = _mapper.Map<PlanoDePagamentoResponse>(createdPlano);
            
            return new Result<PlanoDePagamentoResponse>(response)
            {
                Message = "Plano de pagamento criado com sucesso"
            };
        }
        catch (Exception ex)
        {
            var result = new Result<PlanoDePagamentoResponse>();
            result.WithException($"Erro ao criar plano de pagamento: {ex.Message}");
            return result;
        }
    }

    private static string GeneratePaymentCode(MetodoPagamento metodo)
    {
        return metodo switch
        {
            MetodoPagamento.Boleto => $"BOL{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(100000, 999999)}",
            MetodoPagamento.Pix => $"PIX{Guid.NewGuid().ToString("N")[..16]}",
            _ => $"PAY{Guid.NewGuid().ToString("N")[..16]}"
        };
    }
}