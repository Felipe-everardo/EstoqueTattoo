using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Services;

public interface IMaterialService
{
    Task<IEnumerable<MaterialResponseDTO>> ListarTodosAsync();
    Task<MaterialResponseDTO?> ObterPorIdAsync(int id);
    Task<IEnumerable<MaterialResponseDTO>> ListarCriticosAsync();
    Task<Material> CriarAsync(Material material);
    Task<bool> AtualizarAsync(Material material);
    Task<bool> DeletarAsync(int id);
}
