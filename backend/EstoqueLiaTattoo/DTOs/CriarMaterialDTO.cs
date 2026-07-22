using System.ComponentModel.DataAnnotations;

namespace EstoqueLiaTattoo.DTOs;

public class CriarMaterialDTO
{
    [Required(ErrorMessage = "O nome do material é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A categoria informada é inválida.")]
    public int CategoriaId { get; set; }

    [Range(0, 10000, ErrorMessage = "A quantidade atual não pode ser negativa.")]
    public int QuantidadeAtual { get; set; }

    [Range(0, 1000, ErrorMessage = "A quantidade mínima não pode ser negativa.")]
    public int QuantidadeMinima { get; set; }

    [Range(0, 100000, ErrorMessage = "O preço unitário não pode ser negativo.")]
    public decimal PrecoUnitario { get; set; }
}
