using System.ComponentModel.DataAnnotations;

namespace EstoqueLiaTattoo.DTOs;

public class AtualizarConsumoTintaDTO
{
    [Required(ErrorMessage = "A porcentagem é obrigatória.")]
    [Range(0, 100, ErrorMessage = "A porcentagem deve estar entre 0 e 100.")]
    public int NovaPorcentagem { get; set; }
}
