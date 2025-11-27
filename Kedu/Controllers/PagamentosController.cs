using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kedu.Application.Commands.Pagamento;
using Kedu.Application.Queries.Pagamento;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;

namespace Kedu.Controllers;

[ApiController]
[Route("api/v1/pagamentos")]
[Produces("application/json")]
public class PagamentosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagamentosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<PagamentoDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<PagamentoDetailResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<PagamentoDetailResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<PagamentoDetailResponse>>> GetPagamentoById(int id)
    {
        var query = new GetPagamentoByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("cobranca/{cobrancaId}")]
    [ProducesResponseType(typeof(Result<List<PagamentoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<PagamentoResponse>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<List<PagamentoResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<PagamentoResponse>>>> GetPagamentosByCobrancaId(int cobrancaId)
    {
        var query = new GetPagamentosByCobrancaIdQuery { CobrancaId = cobrancaId };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<List<PagamentoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<PagamentoResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<PagamentoResponse>>>> GetAllPagamentos()
    {
        var query = new GetAllPagamentosQuery();
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> DeletePagamento(int id)
    {
        var command = new DeletePagamentoCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }
}