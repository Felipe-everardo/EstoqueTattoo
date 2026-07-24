using EstoqueLiaTattoo.Controllers;
using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLiaTattoo.Tests;

[TestClass]
public class DashboardControllerTests
{
    [TestMethod]
    public async Task GetResumo_DeveRetornarContadoresEListasCoerentes()
    {
        await using var context = ServiceTestFactory.CreateContext();
        context.Tinta.AddRange(
            new Tinta
            {
                Id = 1,
                MaterialId = 1,
                PorcentagemRestante = 75,
                DataAbertura = DateTime.Now
            },
            new Tinta
            {
                Id = 2,
                MaterialId = 2,
                PorcentagemRestante = 20,
                DataAbertura = DateTime.Now
            });
        await context.SaveChangesAsync();

        var controller = new DashboardController(context);

        var response = await controller.GetResumo();

        var okResult = response.Result as OkObjectResult;
        Assert.IsNotNull(okResult);

        var resumo = okResult.Value as DashboardResumoDTO;
        Assert.IsNotNull(resumo);
        Assert.AreEqual(2, resumo.TotalMateriais);
        Assert.AreEqual(1, resumo.MateriaisCriticos);
        Assert.AreEqual(2, resumo.TintasEmUso);
        Assert.AreEqual(1, resumo.TintasBaixas);
        Assert.AreEqual(2, resumo.MateriaisAtivos.Count());
        Assert.AreEqual("Agulha 3RL", resumo.MateriaisAbaixoMinimo.Single().Nome);
        Assert.AreEqual(2, resumo.ItensBancada.Count());
        Assert.AreEqual("Agulha 3RL", resumo.ItensBancadaEmAlerta.Single().Nome);
    }
}
