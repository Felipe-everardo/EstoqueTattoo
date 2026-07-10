using EstoqueLiaTattoo.Models;
using EstoqueLiaTattoo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static EstoqueLiaTattoo.DTOs.TintaResponseDTO;

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
    public async Task<IActionResult> ListarTintasAbertas()
    {
        var lista = await _service.ListarItensEmUso();
        return Ok(lista);
    }

    [HttpPost("abrir")]
    public async Task<IActionResult> AbrirNovaTinta([FromBody] AbrirTintaDTO dto)
    {
        var sucesso = await _service.AbrirFrasco(dto);

        if (!sucesso)
        {
            return BadRequest("Não foi possível abrir a tinta. Verifique se há estoque disponível.");
        }

        return Ok(new { mensagem = "Frasco aberto com sucesso!" });
    }

    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarConsumo([FromBody] AtualizarConsumoDTO dto)
    {
        var sucesso = await _service.AtualizarPorcentagem(dto);

        if (!sucesso) return NotFound("Tinta não encontrada.");

        return Ok(new { mensagem = "Nível de tinta atualizado." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> JogarFora(int id)
    {
        var sucesso = await _service.DescartarItem(id);

        if (!sucesso) return NotFound("Tinta não encontrada.");

        return NoContent();
    }
}

