using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLiaTattoo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TintasController : ControllerBase
{
    private readonly ITintaService _service;

    public TintasController(ITintaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TintaResponseDTO>>> ListarTintasAbertas()
    {
        return Ok(await _service.ListarItensEmUso());
    }

    [HttpPost]
    public async Task<ActionResult<TintaResponseDTO>> AbrirNovaTinta(AbrirTintaDTO dto)
    {
        var resultado = await _service.AbrirFrasco(dto);
        if (!resultado.Success)
        {
            return BadRequest(Error(resultado.Code, resultado.Message));
        }

        return CreatedAtAction(nameof(ListarTintasAbertas), new { id = resultado.Value!.Id }, resultado.Value);
    }

    [HttpPost("abrir")]
    public Task<ActionResult<TintaResponseDTO>> AbrirNovaTintaCompatibilidade(AbrirTintaDTO dto)
    {
        return AbrirNovaTinta(dto);
    }

    [HttpPut("{id:int}/nivel")]
    public async Task<ActionResult<TintaResponseDTO>> AtualizarConsumo(int id, AtualizarConsumoTintaDTO dto)
    {
        var resultado = await _service.AtualizarPorcentagem(id, dto);
        if (!resultado.Success)
        {
            return NotFound(Error(resultado.Code, resultado.Message));
        }

        return Ok(resultado.Value);
    }

    [HttpPut("atualizar")]
    public Task<ActionResult<TintaResponseDTO>> AtualizarConsumoCompatibilidade(AtualizarConsumoCompatibilidadeDTO dto)
    {
        return AtualizarConsumo(dto.Id, new AtualizarConsumoTintaDTO
        {
            NovaPorcentagem = dto.NovaPorcentagem
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> JogarFora(int id)
    {
        return await _service.DescartarItem(id)
            ? NoContent()
            : NotFound(Error("INK_NOT_FOUND", "Item da bancada não encontrado."));
    }

    private static ApiErrorResponse Error(string code, string message)
    {
        return new ApiErrorResponse { Code = code, Message = message };
    }

    public class AtualizarConsumoCompatibilidadeDTO
    {
        public int Id { get; set; }
        public int NovaPorcentagem { get; set; }
    }
}
