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
        var materiaisAtivos = await _context.Material
            .AsNoTracking()
            .Where(m => m.IsAtivo)
            .OrderBy(m => m.Nome)
            .Select(m => new DashboardMaterialDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                Categoria = m.Categoria != null ? m.Categoria.Nome : "Sem categoria",
                QuantidadeAtual = m.QuantidadeAtual,
                QuantidadeMinima = m.QuantidadeMinima
            })
            .ToListAsync();

        var itensBancada = await _context.Tinta
            .AsNoTracking()
            .OrderBy(t => t.Material.Nome)
            .Select(t => new DashboardTintaDTO
            {
                Id = t.Id,
                MaterialId = t.MaterialId,
                Nome = t.Material.Nome,
                Categoria = t.Material.Categoria != null ? t.Material.Categoria.Nome : "Sem categoria",
                PorcentagemRestante = t.PorcentagemRestante
            })
            .ToListAsync();

        var materiaisAbaixoMinimo = materiaisAtivos
            .Where(m => m.QuantidadeAtual <= m.QuantidadeMinima)
            .ToList();

        var itensBancadaEmAlerta = itensBancada
            .Where(t => t.PorcentagemRestante <= 20)
            .ToList();

        var resumo = new DashboardResumoDTO
        {
            TotalMateriais = materiaisAtivos.Count,
            MateriaisCriticos = materiaisAbaixoMinimo.Count,
            TintasEmUso = itensBancada.Count,
            TintasBaixas = itensBancadaEmAlerta.Count,
            MateriaisAtivos = materiaisAtivos,
            MateriaisAbaixoMinimo = materiaisAbaixoMinimo,
            ItensBancada = itensBancada,
            ItensBancadaEmAlerta = itensBancadaEmAlerta
        };

        return Ok(resumo);
    }
}
