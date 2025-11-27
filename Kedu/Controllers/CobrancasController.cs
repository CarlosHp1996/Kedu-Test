using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kedu.Application.Commands.Pagamento;
using Kedu.Application.Commands.Cobranca;
using Kedu.Application.Queries.Cobranca;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace Kedu.Controllers;

[ApiController]
[Route("api/v1/cobrancas")]
[Produces("application/json")]
[SwaggerTag("Gestão de Cobranças e Pagamentos")]
public class CobrancasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CobrancasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id}/pagamentos")]
    [SwaggerOperation(Summary = "Registrar pagamento [OBRIGATÓRIO]", Description = "Registra pagamento de uma cobrança com aplicação automática das regras de negócio")]
    [ProducesResponseType(typeof(Result<RegistrarPagamentoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<RegistrarPagamentoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<RegistrarPagamentoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<RegistrarPagamentoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<RegistrarPagamentoResponse>>> RegistrarPagamento(int id, [FromBody] CreatePagamentoRequest request)
    {
        var command = new CreatePagamentoCommand
        {
            CobrancaId = id,
            Valor = request.Valor,
            DataPagamento = request.DataPagamento
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return CreatedAtAction(nameof(GetCobrancaById), new { id = result.Value.CobrancaId }, result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter detalhes da cobrança", Description = "Retorna detalhes completos incluindo plano, valor, data, método, código e status")]
    [ProducesResponseType(typeof(Result<CobrancaDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CobrancaDetailResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CobrancaDetailResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CobrancaDetailResponse>>> GetCobrancaById(int id)
    {
        var query = new GetCobrancaByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listar todas as cobranças", Description = "Obtém a lista completa de cobranças do sistema")]
    [ProducesResponseType(typeof(Result<List<CobrancaResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<CobrancaResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<CobrancaResponse>>>> GetAllCobrancas()
    {
        var query = new GetAllCobrancasQuery();
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar cobrança", Description = "Atualiza dados de uma cobrança (não permitido para status PAGA/CANCELADA)")]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CobrancaResponse>>> UpdateCobranca(int id, [FromBody] CobrancaRequest request)
    {
        var command = new UpdateCobrancaCommand
        {
            Id = id,
            Valor = request.Valor,
            DataVencimento = request.DataVencimento,
            MetodoPagamento = request.MetodoPagamento
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpPost("{id}/cancelar")]
    [SwaggerOperation(Summary = "Cancelar cobrança", Description = "Cancela uma cobrança (não permite cancelar cobranças já pagas)")]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CobrancaResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CobrancaResponse>>> CancelCobranca(int id, [FromBody] CancelCobrancaRequest request)
    {
        var command = new CancelCobrancaCommand
        {
            Id = id,
            Motivo = request.Motivo ?? "Cancelamento solicitado"
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remover cobrança", Description = "Remove permanentemente uma cobrança do sistema")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> DeleteCobranca(int id)
    {
        var command = new DeleteCobrancaCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }
}

public class CancelCobrancaRequest
{
    public string? Motivo { get; set; }
}