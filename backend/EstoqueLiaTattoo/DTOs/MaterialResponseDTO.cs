namespace EstoqueLiaTattoo.DTOs;

public class MaterialResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string NomeCategoria { get; set; } = string.Empty;
}
