using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Services.Impl;

public class TintaService : ITintaService
{
    private readonly EstoqueLiaTattooContext _context;

    public TintaService(EstoqueLiaTattooContext context)
    {
        _context = context;
    }

    public async Task<List<TintaResponseDTO>> ListarItensEmUso()
    {
        return await _context.Tinta
            .Include(t => t.Material)
            .ThenInclude(m => m.Categoria)
            .OrderBy(t => t.Material.Nome)
            .Select(t => ToResponseDto(t))
            .ToListAsync();
    }

    public async Task<ServiceResult<TintaResponseDTO>> AbrirFrasco(AbrirTintaDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var materialEstoque = await _context.Material
                .Include(m => m.Categoria)
                .FirstOrDefaultAsync(m => m.Id == dto.MaterialId && m.IsAtivo);

            if (materialEstoque == null)
            {
                return ServiceResult<TintaResponseDTO>.Fail(
                    "MATERIAL_NOT_FOUND",
                    "Material não encontrado ou inativo.");
            }

            if (materialEstoque.QuantidadeAtual <= 0)
            {
                return ServiceResult<TintaResponseDTO>.Fail(
                    "INSUFFICIENT_STOCK",
                    "Não há estoque disponível para enviar este item para a bancada.");
            }

            materialEstoque.QuantidadeAtual -= 1;

            var novaTinta = new Tinta
            {
                MaterialId = dto.MaterialId,
                PorcentagemRestante = 100,
                DataAbertura = DateTime.Now
            };

            _context.Tinta.Add(novaTinta);
            _context.Movimentacao.Add(new Movimentacao
            {
                MaterialId = dto.MaterialId,
                Quantidade = 1,
                Tipo = "Saida",
                Data = DateTime.Now,
                Observacao = "Material enviado para a bancada"
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            novaTinta.Material = materialEstoque;
            return ServiceResult<TintaResponseDTO>.Ok(ToResponseDto(novaTinta));
        }
        catch
        {
            await transaction.RollbackAsync();
            return ServiceResult<TintaResponseDTO>.Fail(
                "INK_OPEN_FAILED",
                "Não foi possível abrir o item na bancada.");
        }
    }

    public async Task<ServiceResult<TintaResponseDTO>> AtualizarPorcentagem(int id, AtualizarConsumoTintaDTO dto)
    {
        var tinta = await _context.Tinta
            .Include(t => t.Material)
            .ThenInclude(m => m.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tinta == null)
        {
            return ServiceResult<TintaResponseDTO>.Fail(
                "INK_NOT_FOUND",
                "Item da bancada não encontrado.");
        }

        tinta.PorcentagemRestante = dto.NovaPorcentagem;

        await _context.SaveChangesAsync();
        return ServiceResult<TintaResponseDTO>.Ok(ToResponseDto(tinta));
    }

    public async Task<bool> DescartarItem(int id)
    {
        var tinta = await _context.Tinta.FindAsync(id);
        if (tinta == null) return false;

        _context.Tinta.Remove(tinta);
        await _context.SaveChangesAsync();
        return true;
    }

    private static TintaResponseDTO ToResponseDto(Tinta tinta)
    {
        return new TintaResponseDTO
        {
            Id = tinta.Id,
            TintaNome = tinta.Material.Nome,
            Categoria = tinta.Material.Categoria?.Nome ?? "Sem Categoria",
            PorcentagemRestante = tinta.PorcentagemRestante,
            DataAbertura = tinta.DataAbertura
        };
    }
}
