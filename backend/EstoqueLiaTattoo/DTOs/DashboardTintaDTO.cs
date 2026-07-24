namespace EstoqueLiaTattoo.DTOs;

public class DashboardTintaDTO
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int PorcentagemRestante { get; set; }
}
