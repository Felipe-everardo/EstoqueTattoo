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
            .OrderByDescending(m => m.Data)
            .Select(m => ToResponseDto(m))
            .ToListAsync();
    }

    public async Task<MovimentacaoResponseDTO?> ObterPorIdAsync(int id)
    {
        var movimentacao = await _context.Movimentacao
           .Include(m => m.Material)
           .FirstOrDefaultAsync(m => m.Id == id);

        return movimentacao == null ? null : ToResponseDto(movimentacao);
    }

    public async Task<ServiceResult<MovimentacaoResponseDTO>> ProcessarMovimentacaoAsync(RegistrarMovimentacaoDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var material = await _context.Material.FirstOrDefaultAsync(m => m.Id == dto.MaterialId && m.IsAtivo);
            if (material == null)
            {
                return ServiceResult<MovimentacaoResponseDTO>.Fail(
                    "MATERIAL_NOT_FOUND",
                    "Material não encontrado ou inativo.");
            }

            if (dto.Tipo == "Saida" && material.QuantidadeAtual < dto.Quantidade)
            {
                return ServiceResult<MovimentacaoResponseDTO>.Fail(
                    "INSUFFICIENT_STOCK",
                    "Estoque insuficiente para realizar a saída.");
            }

            material.QuantidadeAtual += dto.Tipo == "Entrada" ? dto.Quantidade : -dto.Quantidade;

            var movimentacao = new Movimentacao
            {
                MaterialId = dto.MaterialId,
                Quantidade = dto.Quantidade,
                Tipo = dto.Tipo,
                Data = DateTime.Now,
                Observacao = dto.Observacao
            };

            _context.Movimentacao.Add(movimentacao);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var criada = await ObterPorIdAsync(movimentacao.Id);
            return ServiceResult<MovimentacaoResponseDTO>.Ok(criada!);
        }
        catch
        {
            await transaction.RollbackAsync();
            return ServiceResult<MovimentacaoResponseDTO>.Fail(
                "MOVEMENT_CREATE_FAILED",
                "Não foi possível processar a movimentação.");
        }
    }

    private static MovimentacaoResponseDTO ToResponseDto(Movimentacao movimentacao)
    {
        return new MovimentacaoResponseDTO
        {
            Id = movimentacao.Id,
            MaterialId = movimentacao.MaterialId,
            NomeMaterial = movimentacao.Material?.Nome ?? "Material Excluído",
            Quantidade = movimentacao.Quantidade,
            Tipo = movimentacao.Tipo,
            Data = movimentacao.Data,
            Observacao = movimentacao.Observacao
        };
    }
}
