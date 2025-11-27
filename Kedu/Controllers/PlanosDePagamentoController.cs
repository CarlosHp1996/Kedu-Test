using Kedu.Application.Commands.PlanoDePagamento;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;
using Kedu.Application.Queries.PlanoDePagamento;
using Kedu.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kedu.Controllers;

[ApiController]
[Route("api/v1/planos-de-pagamento")]
[Produces("application/json")]
[SwaggerTag("Gestão de Planos de Pagamento")]
public class PlanosDePagamentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlanosDePagamentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar plano de pagamento", Description = "Cria um plano de pagamento com centro de custo e array de cobranças")]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<PlanoDePagamentoResponse>>> CreatePlanoDePagamento([FromBody] CreatePlanoDePagamentoRequest request)
    {
        var command = new CreatePlanoDePagamentoCommand
        {
            ResponsavelId = request.ResponsavelId,
            CentroDeCusto = request.CentroDeCusto,
            Cobrancas = request.Cobrancas.Select(c => new CobrancaData
            {
                Valor = c.Valor,
                DataVencimento = c.DataVencimento,
                MetodoPagamento = c.MetodoPagamento
            }).ToList()
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return CreatedAtAction(nameof(GetPlanoDePagamentoById), new { id = result.Value.Id }, result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter detalhes do plano", Description = "Retorna detalhes completos do plano incluindo centro de custo, cobranças e valor total")]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoDetailResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoDetailResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<PlanoDePagamentoDetailResponse>>> GetPlanoDePagamentoById(int id)
    {
        var query = new GetPlanoDePagamentoByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("{id}/total")]
    [SwaggerOperation(Summary = "Obter valor total do plano", Description = "Retorna o valor total calculado do plano de pagamento")]
    [ProducesResponseType(typeof(Result<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<decimal>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<decimal>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<decimal>>> GetPlanoDePagamentoTotal(int id)
    {
        var query = new GetPlanoDePagamentoTotalQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listar todos os planos", Description = "Obtém a lista completa de planos de pagamento")]
    [ProducesResponseType(typeof(Result<List<PlanoDePagamentoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<PlanoDePagamentoResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<PlanoDePagamentoResponse>>>> GetAllPlanosDePagamento()
    {
        var query = new GetAllPlanosDePagamentoQuery();
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar plano", Description = "Atualiza o centro de custo de um plano de pagamento")]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<PlanoDePagamentoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<PlanoDePagamentoResponse>>> UpdatePlanoDePagamento(int id, [FromBody] UpdatePlanoDePagamentoRequest request)
    {
        var command = new UpdatePlanoDePagamentoCommand
        {
            Id = id,
            CentroDeCustoId = request.CentroDeCustoId
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remover plano", Description = "Remove permanentemente um plano de pagamento")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> DeletePlanoDePagamento(int id)
    {
        var command = new DeletePlanoDePagamentoCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }
}