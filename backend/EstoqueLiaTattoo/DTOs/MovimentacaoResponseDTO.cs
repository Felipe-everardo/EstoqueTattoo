namespace EstoqueLiaTattoo.DTOs;

public class MovimentacaoResponseDTO
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string NomeMaterial { get; set; } = string.Empty; // Facilitador para o Front-end
    public int Quantidade { get; set; }
    public string Tipo { get; set; } = string.Empty; // Entrada ou Saída
    public DateTime Data { get; set; }
    public string? Observacao { get; set; }
}

