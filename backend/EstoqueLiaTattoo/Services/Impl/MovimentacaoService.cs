using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Services.Impl;

public class MovimentacaoService : IMovimentacaoServico
{
    private readonly EstoqueLiaTattooContext _context;

    public MovimentacaoService(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MovimentacaoResponseDTO>> ListarHistoricoAsync()
    {
        return await _context.Movimentacao
            .Include(m => m.Material)
            .OrderByDescending(m => m.Data) // Histórico: mais recentes primeiro
            .Select(m => new MovimentacaoResponseDTO
            {
                Id = m.Id,
                MaterialId = m.MaterialId,
                NomeMaterial = m.Material != null ? m.Material.Nome : "Material Excluído",
                Quantidade = m.Quantidade,
                Tipo = m.Tipo,
                Data = m.Data,
                Observacao = m.Observacao
            }).ToListAsync();
    }

    public async Task<MovimentacaoResponseDTO?> ObterPorIdAsync(int id)
    {
        var m = await _context.Movimentacao
           .Include(m => m.Material)
           .FirstOrDefaultAsync(m => m.Id == id);

        if (m == null) return null;

        return new MovimentacaoResponseDTO
        {
            Id = m.Id,
            MaterialId = m.MaterialId,
            NomeMaterial = m.Material != null ? m.Material.Nome : "Material Excluído",
            Quantidade = m.Quantidade,
            Tipo = m.Tipo,
            Data = m.Data,
            Observacao = m.Observacao
        };
    }

    public async Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var material = await _context.Material.FindAsync(movimentacao.MaterialId);
            if (material == null) return null;

            if (movimentacao.Tipo.ToLower() == "saida")
            {
                if (material.QuantidadeAtual < movimentacao.Quantidade) return null;
                material.QuantidadeAtual -= movimentacao.Quantidade;
            }
            else
            {
                material.QuantidadeAtual += movimentacao.Quantidade;
            }

            _context.Movimentacao.Add(movimentacao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Movimentacao
            {
                Id = movimentacao.Id,
                MaterialId = movimentacao.MaterialId,
                Quantidade = movimentacao.Quantidade,
                Tipo = movimentacao.Tipo,
                Data = movimentacao.Data,
                Observacao = movimentacao.Observacao
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }
}

