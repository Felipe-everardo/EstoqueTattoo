using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EstoqueLiaTattoo.Tests;

internal static class ServiceTestFactory
{
    public static EstoqueLiaTattooContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EstoqueLiaTattooContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new EstoqueLiaTattooContext(options);
        Seed(context);
        return context;
    }

    private static void Seed(EstoqueLiaTattooContext context)
    {
        context.Categoria.AddRange(
            new Categoria { Id = 1, Nome = "Agulhas" },
            new Categoria { Id = 2, Nome = "Tintas" });

        context.Material.AddRange(
            new Material
            {
                Id = 1,
                Nome = "Tinta Preta",
                CategoriaId = 2,
                QuantidadeAtual = 3,
                QuantidadeMinima = 1,
                PrecoUnitario = 100,
                IsAtivo = true
            },
            new Material
            {
                Id = 2,
                Nome = "Agulha 3RL",
                CategoriaId = 1,
                QuantidadeAtual = 0,
                QuantidadeMinima = 5,
                PrecoUnitario = 8,
                IsAtivo = true
            });

        context.SaveChanges();
    }
}
