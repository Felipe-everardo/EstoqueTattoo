using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueLiaTattoo.Models;

public class Tinta
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O material é obrigatório.")]
    public int MaterialId { get; set; }

    [ForeignKey("MaterialId")]
    public virtual Material Material { get; set; } = null!;

    [Range(0, 100, ErrorMessage = "A porcentagem deve estar entre 0 e 100.")]
    public int PorcentagemRestante { get; set; } = 100;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DataAbertura { get; set; } = DateTime.Now; 
}
