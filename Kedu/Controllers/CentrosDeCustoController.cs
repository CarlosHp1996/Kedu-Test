using Microsoft.AspNetCore.Mvc;
using MediatR;
using Kedu.Application.Commands.CentroDeCusto;
using Kedu.Application.Queries.CentroDeCusto;
using Kedu.Application.Models.Requests;
using Kedu.Application.Models.Responses;
using Kedu.Domain.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace Kedu.Controllers;

[ApiController]
[Route("api/v1/centros-de-custo")]
[Produces("application/json")]
[SwaggerTag("Centros de Custo Customizáveis [PLUS]")]
public class CentrosDeCustoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CentrosDeCustoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar centro de custo [PLUS]", Description = "Cadastra um centro de custo customizável")]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CentroDeCustoResponse>>> CreateCentroDeCusto([FromBody] CentroDeCustoRequest request)
    {
        var command = new CreateCentroDeCustoCommand
        {
            Nome = request.Nome
        };

        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return CreatedAtAction(nameof(GetCentroDeCustoById), new { id = result.Value.Id }, result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listar centros de custo [PLUS]", Description = "Lista todos os centros de custo (padrões + customizados)")]
    [ProducesResponseType(typeof(Result<List<CentroDeCustoResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<CentroDeCustoResponse>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<List<CentroDeCustoResponse>>>> GetAllCentrosDeCusto()
    {
        var query = new GetAllCentrosDeCustoQuery();
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter centro de custo por ID", Description = "Retorna os dados de um centro de custo específico")]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CentroDeCustoResponse>>> GetCentroDeCustoById(int id)
    {
        var query = new GetCentroDeCustoByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar centro de custo", Description = "Atualiza os dados de um centro de custo existente")]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CentroDeCustoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<CentroDeCustoResponse>>> UpdateCentroDeCusto(int id, [FromBody] CentroDeCustoRequest request)
    {
        var command = new UpdateCentroDeCustoCommand
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
    [SwaggerOperation(Summary = "Remover centro de custo", Description = "Remove um centro de custo (não permite se estiver em uso)")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result<bool>>> DeleteCentroDeCusto(int id)
    {
        var command = new DeleteCentroDeCustoCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.HasError)
            return StatusCode((int)result.HttpStatusCode, result);        

        return Ok(result);
    }
}