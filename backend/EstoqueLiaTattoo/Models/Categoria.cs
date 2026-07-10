using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EstoqueLiaTattoo.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MaxLength(50)]
    public string Nome { get; set; } = string.Empty;

    // Relacionamento Inverso: Uma categoria tem muitos materiais
    public virtual ICollection<Material>? Materiais { get; set; }
}
 