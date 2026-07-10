using EstoqueLiaTattoo.DTOs;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Services;

public interface IMovimentacaoServico
{
    Task<Movimentacao?> ProcessarMovimentacaoAsync(Movimentacao movimentacao);
    Task<IEnumerable<MovimentacaoResponseDTO>> ListarHistoricoAsync();
    Task<MovimentacaoResponseDTO?> ObterPorIdAsync(int id);
}
