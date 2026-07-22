using EstoqueLiaTattoo.DTOs;

namespace EstoqueLiaTattoo.Services;

public interface ITintaService
{
    Task<List<TintaResponseDTO>> ListarItensEmUso();
    Task<ServiceResult<TintaResponseDTO>> AbrirFrasco(AbrirTintaDTO dto);
    Task<ServiceResult<TintaResponseDTO>> AtualizarPorcentagem(int id, AtualizarConsumoTintaDTO dto);
    Task<bool> DescartarItem(int id);
}
