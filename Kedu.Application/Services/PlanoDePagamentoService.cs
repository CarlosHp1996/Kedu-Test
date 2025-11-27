using Kedu.Application.Interfaces;
using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services;

public class PlanoDePagamentoService : IPlanoDePagamentoService
{
    private readonly IPlanoDePagamentoRepository _repository;
    private readonly IResponsavelFinanceiroRepository _responsavelRepository;
    private readonly ICentroDeCustoRepository _centroDeCustoRepository;
    private readonly ICobrancaRepository _cobrancaRepository;
    private readonly IPaymentCodeGenerator _paymentCodeGenerator;

    public PlanoDePagamentoService(
        IPlanoDePagamentoRepository repository,
        IResponsavelFinanceiroRepository responsavelRepository,
        ICentroDeCustoRepository centroDeCustoRepository,
        ICobrancaRepository cobrancaRepository,
        IPaymentCodeGenerator paymentCodeGenerator)
    {
        _repository = repository;
        _responsavelRepository = responsavelRepository;
        _centroDeCustoRepository = centroDeCustoRepository;
        _cobrancaRepository = cobrancaRepository;
        _paymentCodeGenerator = paymentCodeGenerator;
    }

    public async Task<Result<PlanoDePagamento>> CreatePlanoDePagamentoAsync(PlanoDePagamento plano)
    {
        var result = new Result<PlanoDePagamento>();

        try
        {
            // Validação de negócios: Verificar se o responsável existe.
            var responsavelExists = await _responsavelRepository.GetByIdAsync(plano.ResponsavelFinanceiroId);
            if (responsavelExists == null)
            {
                result.WithError("Responsável financeiro não encontrado");
                return result;
            }

            // Validação de negócios: Verificar se o centro de custo existe
            var centroDeCustoExists = await _centroDeCustoRepository.GetByIdAsync(plano.CentroDeCustoId);
            if (centroDeCustoExists == null)
            {
                result.WithError("Centro de custo não encontrado");
                return result;
            }

            // Validação comercial: Verifique se há plano duplicado (mesmo responsável + centro de custo)
            var existingPlano = await _repository.GetByResponsavelAndCentroDeCustoAsync(
                plano.ResponsavelFinanceiroId, 
                plano.CentroDeCustoId);

            if (existingPlano != null)
            {
                result.WithError("Já existe um plano de pagamento para esta combinação de responsável e centro de custo.");
                return result;
            }

            // Validação de negócio: Validar cobranças
            if (plano.Cobrancas == null || !plano.Cobrancas.Any())
            {
                result.WithError("O plano de pagamento deve ter pelo menos uma cobrança.");
                return result;
            }

            // Gerar códigos de pagamento para cobranças
            foreach (var cobranca in plano.Cobrancas)
            {
                cobranca.CodigoPagamento = _paymentCodeGenerator.GeneratePaymentCode(cobranca.MetodoPagamento);
            }

            var createdPlano = await _repository.AddAsync(plano);
            result.Value = createdPlano;
            result.Message = "Plano de pagamento criado com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao criar o plano de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<PlanoDePagamento>> UpdatePlanoDePagamentoAsync(int id, PlanoDePagamento plano)
    {
        var result = new Result<PlanoDePagamento>();

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                result.WithNotFound("Plano de pagamento não encontrado");
                return result;
            }

            // Validação de negócios: Verificar se o centro de custo existe (se alterado)
            if (plano.CentroDeCustoId != existing.CentroDeCustoId)
            {
                var centroDeCustoExists = await _centroDeCustoRepository.GetByIdAsync(plano.CentroDeCustoId);
                if (centroDeCustoExists == null)
                {
                    result.WithError("Centro de custo não encontrado");
                    return result;
                }

                // Verificar se há plano duplicado (mesmo responsável + novo centro de custo)
                var duplicatePlano = await _repository.GetByResponsavelAndCentroDeCustoAsync(
                    existing.ResponsavelFinanceiroId,
                    plano.CentroDeCustoId);

                if (duplicatePlano != null && duplicatePlano.Id != id)
                {
                    result.WithError("Já existe um plano de pagamento para esta combinação de responsável e centro de custo.");
                    return result;
                }
            }

            // Atualizar apenas os campos permitidos (centro de custo)
            existing.CentroDeCustoId = plano.CentroDeCustoId;

            await _repository.UpdateAsync(existing);
            result.Value = existing;
            result.Message = "Plano de pagamento atualizado com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao atualizar o plano de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result> DeletePlanoDePagamentoAsync(int id)
    {
        var result = new Result();

        try
        {
            var canDelete = await CanDeletePlanoAsync(id);
            if (!canDelete.HasSuccess)
            {
                return canDelete;
            }

            if (!canDelete.Value)
            {
                result.WithError("Não é possível excluir um plano de pagamento com cobranças existentes que foram pagas");
                return result;
            }

            await _repository.DeleteByIdAsync(id);

            result.Message = "Plano de pagamento excluído com sucesso";
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao excluir o plano de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<PlanoDePagamento>> GetByIdAsync(int id)
    {
        var result = new Result<PlanoDePagamento>();

        try
        {
            var plano = await _repository.GetByIdAsync(id);
            if (plano == null)
            {
                result.WithNotFound("Plano de pagamento não encontrado");
                return result;
            }

            result.Value = plano;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar o plano de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<IEnumerable<PlanoDePagamento>>> GetAllAsync()
    {
        var result = new Result<IEnumerable<PlanoDePagamento>>();

        try
        {
            var planos = await _repository.GetAllAsync();
            result.Value = planos;
            result.Count = planos.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar os planos de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<IEnumerable<PlanoDePagamento>>> GetByResponsavelIdAsync(int responsavelId)
    {
        var result = new Result<IEnumerable<PlanoDePagamento>>();

        try
        {
            // Validação de negócios: Verificar se o responsável existe.
            var responsavelExists = await _responsavelRepository.GetByIdAsync(responsavelId);
            if (responsavelExists == null)
            {
                result.WithNotFound("Responsável financeiro não encontrado");
                return result;
            }

            var planos = await _repository.GetByResponsavelIdAsync(responsavelId);
            result.Value = planos;
            result.Count = planos.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar os planos de pagamento pelo responsável: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<decimal>> GetTotalAsync(int id)
    {
        var result = new Result<decimal>();

        try
        {
            var plano = await _repository.GetByIdAsync(id);
            if (plano == null)
            {
                result.WithNotFound("Plano de pagamento não encontrado");
                return result;
            }

            // Calcular total das cobranças
            result.Value = plano.ValorTotal;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao calcular o total do plano de pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> CanDeletePlanoAsync(int id)
    {
        var result = new Result<bool>();

        try
        {
            var planoExists = await PlanoExistsAsync(id);
            if (!planoExists.HasSuccess)
            {
                return planoExists;
            }

            if (!planoExists.Value)
            {
                result.WithNotFound("Plano de pagamento não encontrado");
                return result;
            }

            // Verifique se as taxas foram pagas.
            var cobrancas = await _cobrancaRepository.GetByPlanoDePagamentoIdAsync(id);
            var hasPaidCobrancas = cobrancas.Any(c => c.Status == Domain.Enums.StatusCobranca.Paga);

            result.Value = !hasPaidCobrancas;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se o plano de pagamento pode ser excluído: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> PlanoExistsAsync(int id)
    {
        var result = new Result<bool>();

        try
        {
            var plano = await _repository.GetByIdAsync(id);
            result.Value = plano != null;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se o plano de pagamento existe: {ex.Message}");
            return result;
        }
    }
}