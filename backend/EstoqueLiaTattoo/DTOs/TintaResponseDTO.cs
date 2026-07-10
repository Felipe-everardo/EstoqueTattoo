namespace EstoqueLiaTattoo.DTOs;

public class TintaResponseDTO
{
    // O que o Front-end recebe para mostrar os Cards
    public int Id { get; set; } // ID da Bancada
    public string TintaNome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int PorcentagemRestante { get; set; }
    public DateTime DataAbertura { get; set; }

    // O que o Front-end envia para abrir um novo frasco
    public class AbrirTintaDTO
    {
        public int TintaId { get; set; } // ID do Estoque
    }

    public class AtualizarConsumoDTO
    {
        public int Id { get; set; } // ID da Bancada
        public int NovaPorcentagem { get; set; }
    }
}
