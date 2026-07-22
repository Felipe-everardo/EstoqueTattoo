using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Services.Impl;

namespace EstoqueLiaTattoo.Tests;

[TestClass]
public class MovimentacaoServiceTests
{
    [TestMethod]
    public async Task ProcessarMovimentacaoAsync_SaidaMaiorQueEstoque_DeveFalhar()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new MovimentacaoService(context);

        var resultado = await service.ProcessarMovimentacaoAsync(new RegistrarMovimentacaoDTO
        {
            MaterialId = 1,
            Quantidade = 10,
            Tipo = "Saida"
        });

        Assert.IsFalse(resultado.Success);
        Assert.AreEqual("INSUFFICIENT_STOCK", resultado.Code);
        Assert.AreEqual(3, context.Material.Find(1)!.QuantidadeAtual);
    }

    [TestMethod]
    public async Task ProcessarMovimentacaoAsync_Entrada_DeveAumentarEstoque()
    {
        using var context = ServiceTestFactory.CreateContext();
        var service = new MovimentacaoService(context);

        var resultado = await service.ProcessarMovimentacaoAsync(new RegistrarMovimentacaoDTO
        {
            MaterialId = 1,
            Quantidade = 2,
            Tipo = "Entrada"
        });

        Assert.IsTrue(resultado.Success);
        Assert.AreEqual(5, context.Material.Find(1)!.QuantidadeAtual);
    }
}
