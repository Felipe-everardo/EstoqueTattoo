using System.ComponentModel.DataAnnotations;

namespace EstoqueLiaTattoo.DTOs;

public class RegistrarMovimentacaoDTO
{
    [Required(ErrorMessage = "O material é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O material informado é inválido.")]
    public int MaterialId { get; set; }

    [Required(ErrorMessage = "A quantidade é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int Quantidade { get; set; }

    [Required(ErrorMessage = "O tipo da movimentação é obrigatório.")]
    [RegularExpression("Entrada|Saida", ErrorMessage = "O tipo deve ser Entrada ou Saida.")]
    public string Tipo { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "A observação deve ter no máximo 255 caracteres.")]
    public string? Observacao { get; set; }
}
