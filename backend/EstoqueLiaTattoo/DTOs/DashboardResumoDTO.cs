namespace EstoqueLiaTattoo.DTOs;

public class DashboardResumoDTO
{
    public int TotalMateriais { get; set; }
    public int MateriaisCriticos { get; set; }
    public int TintasEmUso { get; set; }
    public int TintasBaixas { get; set; }
    public int TotalMovimentacoes { get; set; }
    public IEnumerable<MovimentacaoResponseDTO> UltimasMovimentacoes { get; set; } = [];
}
