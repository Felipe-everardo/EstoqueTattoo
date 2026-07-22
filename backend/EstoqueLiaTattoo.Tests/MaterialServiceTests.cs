using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services.Impl;

namespace EstoqueLiaTattoo.Tests;

[TestClass]
public class MaterialServiceTests
{
    [TestMethod]
    public async Task CriarAsync_ComQuantidadeInicial_DeveCriarMovimentacaoDeEntrada()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new MaterialService(context);

        var resultado = await service.CriarAsync(new CriarMaterialDTO
        {
            Nome = "Batoque Descartável",
            CategoriaId = 1,
            QuantidadeAtual = 20,
            QuantidadeMinima = 5,
            PrecoUnitario = 0.10m
        });

        Assert.IsTrue(resultado.Success);
        Assert.AreEqual(20, resultado.Value!.QuantidadeAtual);
        Assert.IsTrue(context.Movimentacao.Any(m =>
            m.MaterialId == resultado.Value.Id &&
            m.Tipo == "Entrada" &&
            m.Quantidade == 20));
    }

    [TestMethod]
    public async Task DeletarAsync_DeveDesativarMaterialSemApagarHistorico()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new MaterialService(context);

        var removido = await service.DeletarAsync(1);
        var materialAtivo = await service.ObterPorIdAsync(1);
        var materialNoBanco = await context.Material.FindAsync(1);

        Assert.IsTrue(removido);
        Assert.IsNull(materialAtivo);
        Assert.IsNotNull(materialNoBanco);
        Assert.IsFalse(materialNoBanco!.IsAtivo);
    }

    [TestMethod]
    public async Task ListarCriticosAsync_DeveRetornarSomenteMateriaisAbaixoDoMinimo()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new MaterialService(context);

        var criticos = await service.ListarCriticosAsync();

        Assert.AreEqual(1, criticos.Count());
        Assert.AreEqual("Agulha 3RL", criticos.Single().Nome);
    }
}
