using EstoqueLiaTattoo.DTOs;

namespace EstoqueLiaTattoo.Services;

public interface IMovimentacaoServico
{
    Task<ServiceResult<MovimentacaoResponseDTO>> ProcessarMovimentacaoAsync(RegistrarMovimentacaoDTO dto);
    Task<IEnumerable<MovimentacaoResponseDTO>> ListarHistoricoAsync();
    Task<MovimentacaoResponseDTO?> ObterPorIdAsync(int id);
}
