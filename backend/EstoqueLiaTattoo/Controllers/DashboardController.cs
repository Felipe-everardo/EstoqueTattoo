using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly EstoqueLiaTattooContext _context;

    public DashboardController(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    [HttpGet("resumo")]
    public async Task<ActionResult<DashboardResumoDTO>> GetResumo()
    {
        var ultimasMovimentacoes = await _context.Movimentacao
            .Include(m => m.Material)
            .OrderByDescending(m => m.Data)
            .Take(5)
            .Select(m => new MovimentacaoResponseDTO
            {
                Id = m.Id,
                MaterialId = m.MaterialId,
                NomeMaterial = m.Material != null ? m.Material.Nome : "Material Excluído",
                Quantidade = m.Quantidade,
                Tipo = m.Tipo,
                Data = m.Data,
                Observacao = m.Observacao
            })
            .ToListAsync();

        var resumo = new DashboardResumoDTO
        {
            TotalMateriais = await _context.Material.CountAsync(m => m.IsAtivo),
            MateriaisCriticos = await _context.Material.CountAsync(m => m.IsAtivo && m.QuantidadeAtual <= m.QuantidadeMinima),
            TintasEmUso = await _context.Tinta.CountAsync(),
            TintasBaixas = await _context.Tinta.CountAsync(t => t.PorcentagemRestante <= 20),
            TotalMovimentacoes = await _context.Movimentacao.CountAsync(),
            UltimasMovimentacoes = ultimasMovimentacoes
        };

        return Ok(resumo);
    }
}
