using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLiaTattoo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MateriaisController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MateriaisController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialResponseDTO>>> Get()
    {
        return Ok(await _materialService.ListarTodosAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MaterialResponseDTO>> Get(int id)
    {
        var dto = await _materialService.ObterPorIdAsync(id);
        return dto == null ? NotFound(Error("MATERIAL_NOT_FOUND", "Material não encontrado.")) : Ok(dto);
    }

    [HttpGet("criticos")]
    public async Task<ActionResult<IEnumerable<MaterialResponseDTO>>> GetCriticos()
    {
        return Ok(await _materialService.ListarCriticosAsync());
    }

    [HttpPost]
    public async Task<ActionResult<MaterialResponseDTO>> Post(CriarMaterialDTO dto)
    {
        var resultado = await _materialService.CriarAsync(dto);
        if (!resultado.Success)
        {
            return BadRequest(Error(resultado.Code, resultado.Message));
        }

        return CreatedAtAction(nameof(Get), new { id = resultado.Value!.Id }, resultado.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _materialService.DeletarAsync(id)
            ? NoContent()
            : NotFound(Error("MATERIAL_NOT_FOUND", "Material não encontrado ou já removido."));
    }

    private static ApiErrorResponse Error(string code, string message)
    {
        return new ApiErrorResponse { Code = code, Message = message };
    }
}
