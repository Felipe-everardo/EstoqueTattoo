using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLiaTattoo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimentacoesController : ControllerBase
{
    private readonly IMovimentacaoServico _movimentacaoService;

    public MovimentacoesController(IMovimentacaoServico movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimentacaoResponseDTO>>> Get()
    {
        return Ok(await _movimentacaoService.ListarHistoricoAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovimentacaoResponseDTO>> Get(int id)
    {
        var dto = await _movimentacaoService.ObterPorIdAsync(id);
        return dto == null ? NotFound(Error("MOVEMENT_NOT_FOUND", "Movimentação não encontrada.")) : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<MovimentacaoResponseDTO>> Post(RegistrarMovimentacaoDTO dto)
    {
        var resultado = await _movimentacaoService.ProcessarMovimentacaoAsync(dto);
        if (!resultado.Success)
        {
            return BadRequest(Error(resultado.Code, resultado.Message));
        }

        return CreatedAtAction(nameof(Get), new { id = resultado.Value!.Id }, resultado.Value);
    }

    private static ApiErrorResponse Error(string code, string message)
    {
        return new ApiErrorResponse { Code = code, Message = message };
    }
}
