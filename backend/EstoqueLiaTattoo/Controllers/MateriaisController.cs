using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using EstoqueLiaTattoo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueLiaTattoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaisController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MateriaisController(IMaterialService service) => _materialService = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialResponseDTO>>> Get()
        {
            return Ok(await _materialService.ListarTodosAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialResponseDTO>> Get(int id)
        {
            var dto = await _materialService.ObterPorIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialResponseDTO>> Post(Material material)
        {
            var novo = await _materialService.CriarAsync(material);
            var dto = await _materialService.ObterPorIdAsync(novo.Id);

            return CreatedAtAction(nameof(Get), new { id = novo.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _materialService.DeletarAsync(id) ? NoContent() : BadRequest("Erro ao deletar.");
        }

        [HttpGet("criticos")]
        public async Task<ActionResult<IEnumerable<Material>>> GetCriticos() => Ok(await _materialService.ListarCriticosAsync());
    }

}
