using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Services.Impl;

public class MaterialService : IMaterialService
{
    private readonly EstoqueLiaTattooContext _context;

    public MaterialService(EstoqueLiaTattooContext context) => _context = context;

    public async Task<IEnumerable<MaterialResponseDTO>> ListarTodosAsync()
    {
        return await _context.Material
            .Include(m => m.Categoria)
            .Where(m => m.IsAtivo)
            .OrderBy(m => m.Nome)
            .Select(m => ToResponseDto(m))
            .ToListAsync();
    }

    public async Task<MaterialResponseDTO?> ObterPorIdAsync(int id)
    {
        var material = await _context.Material
            .Include(m => m.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id && m.IsAtivo);

        return material == null ? null : ToResponseDto(material);
    }

    public async Task<IEnumerable<MaterialResponseDTO>> ListarCriticosAsync()
    {
        return await _context.Material
            .Include(m => m.Categoria)
            .Where(m => m.IsAtivo && m.QuantidadeAtual <= m.QuantidadeMinima)
            .OrderBy(m => m.Nome)
            .Select(m => ToResponseDto(m))
            .ToListAsync();
    }

    public async Task<ServiceResult<MaterialResponseDTO>> CriarAsync(CriarMaterialDTO dto)
    {
        var categoriaExiste = await _context.Categoria.AnyAsync(c => c.Id == dto.CategoriaId);
        if (!categoriaExiste)
        {
            return ServiceResult<MaterialResponseDTO>.Fail(
                "CATEGORY_NOT_FOUND",
                "A categoria informada não existe.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var quantidadeInicial = dto.QuantidadeAtual;
            var material = new Material
            {
                Nome = dto.Nome.Trim(),
                CategoriaId = dto.CategoriaId,
                QuantidadeAtual = 0,
                QuantidadeMinima = dto.QuantidadeMinima,
                PrecoUnitario = dto.PrecoUnitario,
                IsAtivo = true
            };

            _context.Material.Add(material);
            await _context.SaveChangesAsync();

            if (quantidadeInicial > 0)
            {
                material.QuantidadeAtual = quantidadeInicial;
                _context.Movimentacao.Add(new Movimentacao
                {
                    MaterialId = material.Id,
                    Quantidade = quantidadeInicial,
                    Tipo = "Entrada",
                    Data = DateTime.Now,
                    Observacao = "Entrada inicial cadastrada em materiais"
                });

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            var criado = await ObterPorIdAsync(material.Id);
            return ServiceResult<MaterialResponseDTO>.Ok(criado!);
        }
        catch
        {
            await transaction.RollbackAsync();
            return ServiceResult<MaterialResponseDTO>.Fail(
                "MATERIAL_CREATE_FAILED",
                "Não foi possível cadastrar o material.");
        }
    }

    public async Task<bool> AtualizarAsync(Material material)
    {
        _context.Entry(material).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletarAsync(int id)
    {
        var material = await _context.Material.FirstOrDefaultAsync(m => m.Id == id && m.IsAtivo);
        if (material == null) return false;

        material.IsAtivo = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static MaterialResponseDTO ToResponseDto(Material material)
    {
        return new MaterialResponseDTO
        {
            Id = material.Id,
            Nome = material.Nome,
            QuantidadeAtual = material.QuantidadeAtual,
            QuantidadeMinima = material.QuantidadeMinima,
            PrecoUnitario = material.PrecoUnitario,
            NomeCategoria = material.Categoria?.Nome ?? "Sem Categoria"
        };
    }
}
