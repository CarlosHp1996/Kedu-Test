using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kedu.Application.Commands.ResponsavelFinanceiro;
using Kedu.Application.Queries.ResponsavelFinanceiro;
using Kedu.Application.Queries.PlanoDePagamento;
using Kedu.Application.Queries.Cobranca;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace Kedu.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[SwaggerTag("Gestão de Responsáveis Financeiros")]
public class ResponsaveisController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResponsaveisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar responsável financeiro", Description = "Cria um novo responsável financeiro no sistema")]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<ResponsavelFinanceiroResponse>>> CreateResponsavelFinanceiro([FromBody] ResponsavelFinanceiroRequest request)
    {
        var command = new CreateResponsavelFinanceiroCommand
        {
            Nome = request.Nome
        };

        var result = await _mediator.Send(command);

        if (result.HasError) 
            return StatusCode((int)result.HttpStatusCode, result);        

        return CreatedAtAction(nameof(GetResponsavelFinanceiroById), new { id = result.Value.Id }, result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter responsável por ID", Description = "Retorna os dados de um responsável financeiro específico")]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<ResponsavelFinanceiroResponse>>> GetResponsavelFinanceiroById(int id)
    {
        var query = new GetResponsavelFinanceiroByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("{id}/planos-de-pagamento")]
    [SwaggerOperation(Summary = "Listar planos do responsável", Description = "Obtém todos os planos de pagamento de um responsável financeiro")]
    [ProducesResponseType(typeof(Result<List<PlanoDePagamentoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<PlanoDePagamentoResponse>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<List<PlanoDePagamentoResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<PlanoDePagamentoResponse>>>> GetPlanosDePagamentoByResponsavel(int id)
    {
        var query = new GetPlanosByResponsavelIdQuery { ResponsavelId = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("{id}/cobrancas")]
    [SwaggerOperation(Summary = "Listar cobranças do responsável", Description = "Obtém todas as cobranças de um responsável com detalhes completos")]
    [ProducesResponseType(typeof(Result<CobrancaListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CobrancaListResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CobrancaListResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CobrancaListResponse>>> GetCobrancasByResponsavel(int id)
    {
        var query = new GetCobrancasByResponsavelIdQuery { ResponsavelId = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("{id}/cobrancas/quantidade")]
    [SwaggerOperation(Summary = "Contar cobranças do responsável", Description = "Retorna a quantidade total de cobranças de um responsável")]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<int>>> GetQuantidadeCobrancasByResponsavel(int id)
    {
        var query = new GetCobrancasQuantidadeByResponsavelIdQuery { ResponsavelId = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listar todos os responsáveis", Description = "Obtém a lista completa de responsáveis financeiros cadastrados")]
    [ProducesResponseType(typeof(Result<List<ResponsavelFinanceiroResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<ResponsavelFinanceiroResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<ResponsavelFinanceiroResponse>>>> GetAllResponsaveisFinanceiros()
    {
        var query = new GetAllResponsaveisFinanceirosQuery();
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar responsável", Description = "Atualiza os dados de um responsável financeiro existente")]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<ResponsavelFinanceiroResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<ResponsavelFinanceiroResponse>>> UpdateResponsavelFinanceiro(int id, [FromBody] ResponsavelFinanceiroRequest request)
    {
        var command = new UpdateResponsavelFinanceiroCommand
        {
            Id = id,
            Nome = request.Nome
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remover responsável", Description = "Remove permanentemente um responsável financeiro")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> DeleteResponsavelFinanceiro(int id)
    {
        var command = new DeleteResponsavelFinanceiroCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }
}