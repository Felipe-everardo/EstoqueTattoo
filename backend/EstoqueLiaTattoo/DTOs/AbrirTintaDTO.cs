using System.ComponentModel.DataAnnotations;

namespace EstoqueLiaTattoo.DTOs;

public class AbrirTintaDTO
{
    [Required(ErrorMessage = "O material é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O material informado é inválido.")]
    public int MaterialId { get; set; }
}
