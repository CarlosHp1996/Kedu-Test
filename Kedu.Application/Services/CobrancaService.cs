using Kedu.Application.Interfaces;
using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services;

public class CobrancaService : ICobrancaService
{
    private readonly ICobrancaRepository _repository;
    private readonly IPlanoDePagamentoRepository _planoRepository;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IPaymentCodeGenerator _paymentCodeGenerator;

    public CobrancaService(
        ICobrancaRepository repository,
        IPlanoDePagamentoRepository planoRepository,
        IPagamentoRepository pagamentoRepository,
        IPaymentCodeGenerator paymentCodeGenerator)
    {
        _repository = repository;
        _planoRepository = planoRepository;
        _pagamentoRepository = pagamentoRepository;
        _paymentCodeGenerator = paymentCodeGenerator;
    }

    public async Task<Result<Cobranca>> CreateCobrancaAsync(Cobranca cobranca)
    {
        var result = new Result<Cobranca>();

        try
        {
            // Validação de negócios: Verificar se o plano existe
            var planoExists = await _planoRepository.GetByIdAsync(cobranca.PlanoDePagamentoId);
            if (planoExists == null)
            {
                result.WithError("Plano de pagamento não encontrado");
                return result;
            }

            // Validação comercial: Validar valor
            if (cobranca.Valor <= 0)
            {
                result.WithError("O valor de Cobrança deve ser maior que zero.");
                return result;
            }

            // Validação comercial: Validar data de vencimento
            if (cobranca.DataVencimento < DateTime.Today)
            {
                result.WithError("A data de vencimento não pode ser anterior ao passado.");
                return result;
            }

            // Gerar código de pagamento
            cobranca.CodigoPagamento = _paymentCodeGenerator.GeneratePaymentCode(cobranca.MetodoPagamento, cobranca.Id);
            cobranca.Status = StatusCobranca.Emitida;

            var createdCobranca = await _repository.AddAsync(cobranca);
            result.Value = createdCobranca;
            result.Message = "Cobrança criada com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao criar cobrança: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<Cobranca>> UpdateCobrancaAsync(int id, Cobranca cobranca)
    {
        var result = new Result<Cobranca>();

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Validação comercial: Não é possível atualizar cobranças pagas ou canceladas.
            if (existing.Status == StatusCobranca.Paga)
            {
                result.WithError("Não é possível atualizar uma cobrança paga");
                return result;
            }

            if (existing.Status == StatusCobranca.Cancelada)
            {
                result.WithError("Não é possível atualizar uma cobrança cancelada");
                return result;
            }

            // Validação comercial: Validar valor
            if (cobranca.Valor <= 0)
            {
                result.WithError("O valor da cobrança deve ser maior que zero");
                return result;
            }

            // Validação comercial: Validar data de vencimento
            if (cobranca.DataVencimento < DateTime.Today)
            {
                result.WithError("A data de vencimento não pode ser anterior ao passado");
                return result;
            }

            // Atualizar campos permitidos
            existing.Valor = cobranca.Valor;
            existing.DataVencimento = cobranca.DataVencimento;

            // Gerar novo código de pagamento se o método mudou
            if (existing.MetodoPagamento != cobranca.MetodoPagamento)
            {
                existing.MetodoPagamento = cobranca.MetodoPagamento;
                existing.CodigoPagamento = _paymentCodeGenerator.GeneratePaymentCode(cobranca.MetodoPagamento, id);
            }

            await _repository.UpdateAsync(existing);
            result.Value = existing;
            result.Message = "Cobrança atualizada com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao atualizar cobrança: {ex.Message}");
            return result;
        }
    }

    public async Task<Result> CancelCobrancaAsync(int id)
    {
        var result = new Result();

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Validação comercial: Não é possível cancelar cobranças pagas
            if (existing.Status == StatusCobranca.Paga)
            {
                result.WithError("Não é possível cancelar uma cobrança paga");
                return result;
            }

            // Validação comercial: Já cancelada
            if (existing.Status == StatusCobranca.Cancelada)
            {
                result.WithError("Cobrança já cancelada");
                return result;
            }

            existing.Status = StatusCobranca.Cancelada;
            await _repository.UpdateAsync(existing);

            result.Message = "Cobrança cancelada com sucesso";
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao cancelar cobrança: {ex.Message}");
            return result;
        }
    }

    public async Task<Result> DeleteCobrancaAsync(int id)
    {
        var result = new Result();

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Validação comercial: Não é possível excluir cobranças pagas
            if (existing.Status == StatusCobranca.Paga)
            {
                result.WithError("Não é possível excluir uma cobrança paga");
                return result;
            }

            await _repository.DeleteByIdAsync(id);

            result.Message = "Cobrança excluída com sucesso";
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao excluir cobrança: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<Cobranca>> GetByIdAsync(int id)
    {
        var result = new Result<Cobranca>();

        try
        {
            var cobranca = await _repository.GetByIdAsync(id);
            if (cobranca == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Atualizar status para o valor calculado
            cobranca.Status = await CalculateStatusAsync(id);

            result.Value = cobranca;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar cobrança: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<IEnumerable<Cobranca>>> GetAllAsync()
    {
        var result = new Result<IEnumerable<Cobranca>>();

        try
        {
            var cobrancas = await _repository.GetAllAsync();

            // Atualizar status de cada cobrança
            foreach (var cobranca in cobrancas)
            {
                cobranca.Status = await CalculateStatusAsync(cobranca.Id);
            }

            result.Value = cobrancas;
            result.Count = cobrancas.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar cobranças: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<IEnumerable<Cobranca>>> GetByResponsavelIdAsync(int responsavelId)
    {
        var result = new Result<IEnumerable<Cobranca>>();

        try
        {
            var cobrancas = await _repository.GetByResponsavelIdAsync(responsavelId);

            // Atualizar status de cada cobrança
            foreach (var cobranca in cobrancas)
            {
                cobranca.Status = await CalculateStatusAsync(cobranca.Id);
            }

            result.Value = cobrancas;
            result.Count = cobrancas.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar cobranças por responsável: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<int>> GetQuantidadeByResponsavelIdAsync(int responsavelId)
    {
        var result = new Result<int>();

        try
        {
            var cobrancas = await _repository.GetByResponsavelIdAsync(responsavelId);
            result.Value = cobrancas.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar quantidade de cobranças por responsável: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<Pagamento>> ProcessPaymentAsync(int cobrancaId, decimal valor)
    {
        var result = new Result<Pagamento>();

        try
        {
            var canProcess = await CanProcessPaymentAsync(cobrancaId);
            if (!canProcess.HasSuccess)
            {
                return new Result<Pagamento>
                {
                    HasSuccess = canProcess.HasSuccess,
                    Errors = canProcess.Errors,
                    HttpStatusCode = canProcess.HttpStatusCode
                };
            }

            if (!canProcess.Value)
            {
                result.WithError("Não é possível processar o pagamento desta cobrança");
                return result;
            }

            var cobranca = await _repository.GetByIdAsync(cobrancaId);
            if (cobranca == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Validação da empresa: Verificar o valor do pagamento
            if (valor != cobranca.Valor)
            {
                result.WithError($"O valor do pagamento deve ser exatamente {cobranca.Valor:C}");
                return result;
            }

            // Criar pagamento
            var pagamento = new Pagamento
            {
                CobrancaId = cobrancaId,
                Valor = valor,
                DataPagamento = DateTime.Now
            };

            var createdPagamento = await _pagamentoRepository.AddAsync(pagamento);

            // Atualizar status da cobrança
            cobranca.Status = StatusCobranca.Paga;
            await _repository.UpdateAsync(cobranca);

            result.Value = createdPagamento;
            result.Message = "Pagamento processado com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao processar pagamento: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> CanProcessPaymentAsync(int cobrancaId)
    {
        var result = new Result<bool>();

        try
        {
            var cobranca = await _repository.GetByIdAsync(cobrancaId);
            if (cobranca == null)
            {
                result.WithNotFound("Cobrança não encontrada");
                return result;
            }

            // Regra comercial: Não é possível pagar cobranças canceladas.
            if (cobranca.Status == StatusCobranca.Cancelada)
            {
                result.WithError("Não foi possível processar o pagamento da cobrança cancelada.");
                result.Value = false;
                return result;
            }

            // Regra comercial: Não é possível pagar cobranças já pagas
            if (cobranca.Status == StatusCobranca.Paga)
            {
                result.WithError("Cobrança já está paga");
                result.Value = false;
                return result;
            }

            result.Value = true;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se o pagamento pode ser processado: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> CobrancaExistsAsync(int id)
    {
        var result = new Result<bool>();

        try
        {
            var cobranca = await _repository.GetByIdAsync(id);
            result.Value = cobranca != null;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se a cobrança existe: {ex.Message}");
            return result;
        }
    }

    public async Task<StatusCobranca> CalculateStatusAsync(int cobrancaId)
    {
        try
        {
            var cobranca = await _repository.GetByIdAsync(cobrancaId);
            if (cobranca == null)
                return StatusCobranca.Emitida;

            // Se já estiver definido como pago ou cancelado, mantenha esse status.
            if (cobranca.Status == StatusCobranca.Paga || cobranca.Status == StatusCobranca.Cancelada)
                return cobranca.Status;

            // Calcular se está em atraso (vencida)
            if (DateTime.Now > cobranca.DataVencimento)
                return StatusCobranca.Emitida; // Observação: Não existe um valor de enumeração VENCIDA, portanto, está sendo usada uma propriedade calculada.

            return StatusCobranca.Emitida;
        }
        catch
        {
            return StatusCobranca.Emitida;
        }
    }
}