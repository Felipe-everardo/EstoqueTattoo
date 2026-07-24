namespace EstoqueLiaTattoo.DTOs;

public class DashboardResumoDTO
{
    public int TotalMateriais { get; set; }
    public int MateriaisCriticos { get; set; }
    public int TintasEmUso { get; set; }
    public int TintasBaixas { get; set; }
    public IEnumerable<DashboardMaterialDTO> MateriaisAtivos { get; set; } = [];
    public IEnumerable<DashboardMaterialDTO> MateriaisAbaixoMinimo { get; set; } = [];
    public IEnumerable<DashboardTintaDTO> ItensBancada { get; set; } = [];
    public IEnumerable<DashboardTintaDTO> ItensBancadaEmAlerta { get; set; } = [];
}
