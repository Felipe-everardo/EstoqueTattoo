using EstoqueLiaTattoo.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLiaTattoo.Data;

public static class DemoDataSeeder
{
    private const string DemoMarker = "[DEMO]";

    public static async Task SeedAsync(EstoqueLiaTattooContext context)
    {
        var tintaPreta = await context.Material.FirstOrDefaultAsync(m => m.Id == 3 && m.IsAtivo);
        var tintaVermelha = await context.Material.FirstOrDefaultAsync(m => m.Id == 4 && m.IsAtivo);
        var plasticoFilme = await context.Material.FirstOrDefaultAsync(m => m.Id == 6 && m.IsAtivo);
        var tintaVerde = await GetOrCreateMaterialAsync(context, "Tinta Verde Electric 120ml", quantidadeAtual: 4, quantidadeMinima: 1, precoUnitario: 135.00m);
        var tintaAzul = await GetOrCreateMaterialAsync(context, "Tinta Azul Royal 120ml", quantidadeAtual: 3, quantidadeMinima: 1, precoUnitario: 135.00m);

        if (tintaPreta == null || tintaVermelha == null || plasticoFilme == null)
        {
            return;
        }

        var agora = DateTime.Now;

        await AddTintaEmUsoAsync(context, tintaPreta, porcentagemRestante: 65, dataAbertura: agora.AddDays(-2), "Material enviado para a bancada");
        await AddTintaEmUsoAsync(context, tintaVermelha, porcentagemRestante: 18, dataAbertura: agora.AddDays(-5), "Consumo em sessão");
        await AddTintaEmUsoAsync(context, tintaVerde, porcentagemRestante: 82, dataAbertura: agora.AddDays(-1), "Abertura para sessão colorida");
        await AddTintaEmUsoAsync(context, tintaAzul, porcentagemRestante: 42, dataAbertura: agora.AddHours(-14), "Abertura para trabalho sombreado");

        plasticoFilme.QuantidadeAtual = Math.Max(0, plasticoFilme.QuantidadeMinima - 1);

        await AddMovimentacaoIfMissingAsync(
            context,
            tintaPreta,
            quantidade: 3,
            tipo: "Entrada",
            data: agora.AddDays(-6),
            descricao: "Reposição inicial de tintas");

        await AddMovimentacaoIfMissingAsync(
            context,
            plasticoFilme,
            quantidade: 1,
            tipo: "Saida",
            data: agora.AddHours(-8),
            descricao: "Item próximo do estoque mínimo");

        await context.SaveChangesAsync();
    }

    private static async Task<Material> GetOrCreateMaterialAsync(
        EstoqueLiaTattooContext context,
        string nome,
        int quantidadeAtual,
        int quantidadeMinima,
        decimal precoUnitario)
    {
        var material = await context.Material.FirstOrDefaultAsync(m => m.Nome == nome && m.IsAtivo);
        if (material != null)
        {
            return material;
        }

        material = new Material
        {
            Nome = nome,
            CategoriaId = 2,
            QuantidadeAtual = quantidadeAtual,
            QuantidadeMinima = quantidadeMinima,
            PrecoUnitario = precoUnitario,
            IsAtivo = true
        };

        context.Material.Add(material);
        await context.SaveChangesAsync();
        return material;
    }

    private static async Task AddTintaEmUsoAsync(
        EstoqueLiaTattooContext context,
        Material material,
        int porcentagemRestante,
        DateTime dataAbertura,
        string descricaoMovimentacao)
    {
        var jaEstaNaBancada = await context.Tinta.AnyAsync(t => t.MaterialId == material.Id);
        if (jaEstaNaBancada)
        {
            return;
        }

        material.QuantidadeAtual = Math.Max(0, material.QuantidadeAtual - 1);

        context.Tinta.Add(new Tinta
        {
            MaterialId = material.Id,
            PorcentagemRestante = porcentagemRestante,
            DataAbertura = dataAbertura
        });

        await AddMovimentacaoIfMissingAsync(
            context,
            material,
            quantidade: 1,
            tipo: "Saida",
            data: dataAbertura,
            descricao: descricaoMovimentacao);
    }

    private static async Task AddMovimentacaoIfMissingAsync(
        EstoqueLiaTattooContext context,
        Material material,
        int quantidade,
        string tipo,
        DateTime data,
        string descricao)
    {
        var observacao = $"{DemoMarker} {descricao}";
        var jaExiste = await context.Movimentacao.AnyAsync(m =>
            m.MaterialId == material.Id &&
            m.Tipo == tipo &&
            m.Observacao == observacao);

        if (jaExiste)
        {
            return;
        }

        context.Movimentacao.Add(new Movimentacao
        {
            MaterialId = material.Id,
            Quantidade = quantidade,
            Tipo = tipo,
            Data = data,
            Observacao = observacao
        });
    }
}
