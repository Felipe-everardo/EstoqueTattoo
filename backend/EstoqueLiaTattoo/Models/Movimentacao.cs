using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstoqueLiaTattoo.Models;

public class Movimentacao
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MaterialId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
    public int Quantidade { get; set; }

    [Required]
    [MaxLength(10)] // "Entrada" ou "Saida"
    public string Tipo { get; set; } = string.Empty;

    [Required]
    public DateTime Data { get; set; } = DateTime.Now;

    [MaxLength(255)]
    public string? Observacao { get; set; }

    [ForeignKey("MaterialId")]
    public virtual Material? Material { get; set; }
}
