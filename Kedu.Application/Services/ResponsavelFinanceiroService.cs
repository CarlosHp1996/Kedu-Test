using Kedu.Application.Interfaces;
using Kedu.Application.Services.Interfaces;
using Kedu.Domain.Entities;
using Kedu.Domain.Helpers;

namespace Kedu.Application.Services;

public class ResponsavelFinanceiroService : IResponsavelFinanceiroService
{
    private readonly IResponsavelFinanceiroRepository _repository;
    private readonly IPlanoDePagamentoRepository _planoRepository;

    public ResponsavelFinanceiroService(
        IResponsavelFinanceiroRepository repository,
        IPlanoDePagamentoRepository planoRepository)
    {
        _repository = repository;
        _planoRepository = planoRepository;
    }

    public async Task<Result<ResponsavelFinanceiro>> CreateResponsavelFinanceiroAsync(ResponsavelFinanceiro responsavel)
    {
        var result = new Result<ResponsavelFinanceiro>();

        try
        {
            var createdResponsavel = await _repository.AddAsync(responsavel);
            result.Value = createdResponsavel;
            result.Message = "Responsável financeiro criado com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao criar responsável financeiro: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<ResponsavelFinanceiro>> UpdateResponsavelFinanceiroAsync(int id, ResponsavelFinanceiro responsavel)
    {
        var result = new Result<ResponsavelFinanceiro>();

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                result.WithNotFound("Responsável financeiro não encontrado");
                return result;
            }

            // Atualizar campos
            existing.Nome = responsavel.Nome ?? existing.Nome;

            await _repository.UpdateAsync(existing);
            result.Value = existing;
            result.Message = "Responsável financeiro atualizado com sucesso";

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao atualizar responsável financeiro: {ex.Message}");
            return result;
        }
    }

    public async Task<Result> DeleteResponsavelFinanceiroAsync(int id)
    {
        var result = new Result();

        try
        {
            var canDelete = await CanDeleteResponsavelFinanceiroAsync(id);
            if (!canDelete.HasSuccess)
                return canDelete;            

            if (!canDelete.Value)
            {
                result.WithError("Não é possível excluir o responsável financeiro com planos de pagamento existentes");
                return result;
            }

            await _repository.DeleteByIdAsync(id);

            result.Message = "Responsável financeiro excluído com sucesso";
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao excluir responsável financeiro: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<ResponsavelFinanceiro>> GetByIdAsync(int id)
    {
        var result = new Result<ResponsavelFinanceiro>();

        try
        {
            var responsavel = await _repository.GetByIdAsync(id);
            if (responsavel == null)
            {
                result.WithNotFound("Responsável financeiro não encontrado");
                return result;
            }

            result.Value = responsavel;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar responsável financeiro: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<IEnumerable<ResponsavelFinanceiro>>> GetAllAsync()
    {
        var result = new Result<IEnumerable<ResponsavelFinanceiro>>();

        try
        {
            var responsaveis = await _repository.GetAllAsync();
            result.Value = responsaveis;
            result.Count = responsaveis.Count();
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao recuperar responsáveis ​​financeiros: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> CanDeleteResponsavelFinanceiroAsync(int id)
    {
        var result = new Result<bool>();

        try
        {
            var exists = await ResponsavelExistsAsync(id);
            if (!exists.HasSuccess)
                return exists;            

            if (!exists.Value)
            {
                result.WithNotFound("Responsável financeiro não encontrado");
                return result;
            }

            // Verifique se existem planos de pagamento associados.
            var planos = await _planoRepository.GetByResponsavelIdAsync(id);
            result.Value = !planos.Any();

            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se o responsável financeiro pode ser excluído: {ex.Message}");
            return result;
        }
    }

    public async Task<Result<bool>> ResponsavelExistsAsync(int id)
    {
        var result = new Result<bool>();

        try
        {
            var responsavel = await _repository.GetByIdAsync(id);
            result.Value = responsavel != null;
            return result;
        }
        catch (Exception ex)
        {
            result.WithException($"Erro ao verificar se existe responsável financeiro: {ex.Message}");
            return result;
        }
    }
}