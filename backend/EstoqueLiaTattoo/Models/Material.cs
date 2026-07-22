using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstoqueLiaTattoo.Models;

public class Material
{
    [Key] 
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do material é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 10000, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int QuantidadeAtual { get; set; }

    [Range(0, 1000, ErrorMessage = "A quantidade mínima deve ser um valor positivo.")]
    public int QuantidadeMinima { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoUnitario { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    public bool IsAtivo { get; set; } = true;

    [ForeignKey("CategoriaId")]
    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
}
