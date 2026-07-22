namespace EstoqueLiaTattoo.DTOs;

public class TintaResponseDTO
{
    public int Id { get; set; }
    public string TintaNome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int PorcentagemRestante { get; set; }
    public DateTime DataAbertura { get; set; }
}
