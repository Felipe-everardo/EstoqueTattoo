using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services.Impl;

namespace EstoqueLiaTattoo.Tests;

[TestClass]
public class TintaServiceTests
{
    [TestMethod]
    public async Task AbrirFrasco_DeveReduzirEstoqueECriarMovimentacao()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new TintaService(context);

        var resultado = await service.AbrirFrasco(new AbrirTintaDTO { MaterialId = 1 });

        Assert.IsTrue(resultado.Success);
        Assert.AreEqual(2, context.Material.Find(1)!.QuantidadeAtual);
        Assert.AreEqual(1, context.Tinta.Count());
        Assert.IsTrue(context.Movimentacao.Any(m =>
            m.MaterialId == 1 &&
            m.Tipo == "Saida" &&
            m.Observacao == "Material enviado para a bancada"));
    }

    [TestMethod]
    public async Task AbrirFrasco_SemEstoque_DeveFalhar()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new TintaService(context);

        var resultado = await service.AbrirFrasco(new AbrirTintaDTO { MaterialId = 2 });

        Assert.IsFalse(resultado.Success);
        Assert.AreEqual("INSUFFICIENT_STOCK", resultado.Code);
        Assert.AreEqual(0, context.Tinta.Count());
    }
}
