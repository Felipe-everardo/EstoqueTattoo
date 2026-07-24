import { useEffect, useState } from 'react';
import movimentacaoService from '../services/movimentacaoService';

const formatadorDia = new Intl.DateTimeFormat('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: '2-digit'
});

const formatadorHora = new Intl.DateTimeFormat('pt-BR', {
    hour: '2-digit',
    minute: '2-digit'
});

const formatarData = (valor) => {
    const data = new Date(valor);

    return {
        dia: formatadorDia.format(data),
        hora: formatadorHora.format(data)
    };
};

const Movimentacoes = () => {
    const [historico, setHistorico] = useState([]);
    const [carregando, setCarregando] = useState(true);
    const [erro, setErro] = useState('');

    const carregarHistorico = async () => {
        try {
            setCarregando(true);
            setErro('');
            const dadosHistorico = await movimentacaoService.listar();
            setHistorico(dadosHistorico);
        } catch (error) {
            console.error('Erro ao carregar movimentacoes:', error);
            setErro('Não foi possível carregar as movimentações. Confira se a API está rodando.');
        } finally {
            setCarregando(false);
        }
    };

    useEffect(() => {
        carregarHistorico();
    }, []);

    const entradas = historico.filter((item) => item.tipo === 'Entrada').length;
    const saidas = historico.filter((item) => item.tipo === 'Saida').length;

    return (
        <div className="page-stack">
            <header className="page-header">
                <div>
                    <span className="eyebrow">Fluxo de estoque</span>
                    <h1>Movimentações</h1>
                    <p>Histórico automático gerado por entradas no estoque e envios para a bancada.</p>
                </div>

                <button type="button" className="btn btn-primary" onClick={carregarHistorico} disabled={carregando}>
                    {carregando ? 'Atualizando...' : 'Atualizar'}
                </button>
            </header>

            {erro && <div className="alert alert-danger">{erro}</div>}

            <section className="stats-grid compact">
                <div className="stat-card">
                    <span>Registros</span>
                    <strong>{historico.length}</strong>
                </div>
                <div className="stat-card">
                    <span>Entradas</span>
                    <strong>{entradas}</strong>
                </div>
                <div className="stat-card">
                    <span>Saídas</span>
                    <strong>{saidas}</strong>
                </div>
            </section>

            <section className="surface-panel">
                <div className="panel-header">
                    <div>
                        <h2>Histórico recente</h2>
                        <p>Cada registro nasce de uma ação feita em materiais ou bancada.</p>
                    </div>
                </div>

                <div className="table-responsive table-scroll">
                    <table className="table app-table mb-0">
                        <thead>
                            <tr>
                                <th>Data</th>
                                <th>Material</th>
                                <th>Tipo</th>
                                <th>Qtd</th>
                                <th>Origem</th>
                            </tr>
                        </thead>
                        <tbody>
                            {carregando && (
                                <tr>
                                    <td colSpan="5" className="text-center text-muted py-4">
                                        Carregando movimentações...
                                    </td>
                                </tr>
                            )}

                            {!carregando && historico.length === 0 && (
                                <tr>
                                    <td colSpan="5" className="text-center text-muted py-4">
                                        Nenhuma movimentação registrada.
                                    </td>
                                </tr>
                            )}

                            {historico.map((item) => {
                                const dataFormatada = formatarData(item.data);

                                return (
                                    <tr key={item.id}>
                                        <td data-label="Data">
                                            <time className="movement-date" dateTime={item.data}>
                                                <span>{dataFormatada.dia}</span>
                                                <span>{dataFormatada.hora}</span>
                                            </time>
                                        </td>
                                        <td data-label="Material"><strong>{item.nomeMaterial}</strong></td>
                                        <td data-label="Tipo">
                                            <span className={`status-pill ${item.tipo === 'Saida' ? 'danger' : 'success'}`}>
                                                {item.tipo === 'Saida' ? 'Saída' : item.tipo}
                                            </span>
                                        </td>
                                        <td data-label="Quantidade">{item.quantidade}</td>
                                        <td data-label="Origem" className="movement-origin">{item.observacao || '-'}</td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            </section>
        </div>
    );
};

export default Movimentacoes;
