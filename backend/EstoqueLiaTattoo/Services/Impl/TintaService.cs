using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;
using static EstoqueLiaTattoo.DTOs.TintaResponseDTO;

namespace EstoqueLiaTattoo.Services.Impl;

public class TintaService : ITintaService
{
    public readonly EstoqueLiaTattooContext _context;

    public TintaService(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    public async Task<List<TintaResponseDTO>> ListarItensEmUso()
    {
        return await _context.Tinta
                .Include(t => t.Material)
                .Select(t => new TintaResponseDTO
                {
                    Id = t.Id,
                    TintaNome = t.Material.Nome,
                    Categoria = t.Material.Categoria != null ? t.Material.Categoria.Nome : "Sem Categoria",
                    PorcentagemRestante = t.PorcentagemRestante,
                    DataAbertura = t.DataAbertura
                })
                .ToListAsync();
    }

    public async Task<bool> AbrirFrasco(TintaResponseDTO.AbrirTintaDTO dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // O TintaId do DTO e o ID do material no estoque.
            var materialEstoque = await _context.Material.FirstOrDefaultAsync(m => m.Id == dto.TintaId && m.IsAtivo);

            if (materialEstoque == null || materialEstoque.QuantidadeAtual <= 0)
            {
                return false;
            }

            materialEstoque.QuantidadeAtual -= 1;

            var novaTinta = new Tinta
            {
                MaterialId = dto.TintaId,
                PorcentagemRestante = 100,
                DataAbertura = DateTime.Now
            };

            _context.Tinta.Add(novaTinta);
            _context.Movimentacao.Add(new Movimentacao
            {
                MaterialId = dto.TintaId,
                Quantidade = 1,
                Tipo = "Saida",
                Data = DateTime.Now,
                Observacao = "Material enviado para a bancada"
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> AtualizarPorcentagem(AtualizarConsumoDTO dto)
    {
        var tinta = await _context.Tinta.FindAsync(dto.Id);

        if (tinta == null) return false;

        tinta.PorcentagemRestante = dto.NovaPorcentagem;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DescartarItem(int id)
    {
        var tinta = await _context.Tinta.FindAsync(id);
        if (tinta == null) return false;

        _context.Tinta.Remove(tinta);
        await _context.SaveChangesAsync();
        return true;
    }

    
}
