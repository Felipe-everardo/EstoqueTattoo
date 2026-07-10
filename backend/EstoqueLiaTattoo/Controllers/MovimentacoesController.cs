using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using EstoqueLiaTattoo.Services;
using EstoqueLiaTattoo.Services.Impl;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoResponseDTO>> Get(int id)
        {
            var dto = await _movimentacaoService.ObterPorIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Movimentacao>> PostMovimentacao(Movimentacao movimentacao)
        {
            var resultado = await _movimentacaoService.ProcessarMovimentacaoAsync(movimentacao);

            if (resultado == null)
            {
                return BadRequest("Erro ao processar movimentação. Verifique o estoque disponível ou o ID do material.");
            }

            // Retorna Status 201 (Created), o link para o GET do item e o próprio objeto
            return CreatedAtAction(nameof(Get), new { id = resultado.Id }, resultado);
        }
    }
}
