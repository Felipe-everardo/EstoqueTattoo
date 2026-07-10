import { useEffect, useState } from 'react';
import movimentacaoService from '../services/movimentacaoService';

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
            setErro('Nao foi possivel carregar as movimentacoes. Confira se a API esta rodando.');
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
                    <h1>Movimentacoes</h1>
                    <p>Historico automatico gerado por entradas no estoque e envios para a bancada.</p>
                </div>

                <button type="button" className="btn btn-outline-dark" onClick={carregarHistorico} disabled={carregando}>
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
                    <span>Saidas</span>
                    <strong>{saidas}</strong>
                </div>
            </section>

            <section className="surface-panel">
                <div className="panel-header">
                    <div>
                        <h2>Historico recente</h2>
                        <p>Cada registro nasce de uma acao feita em materiais ou bancada.</p>
                    </div>
                </div>

                <div className="table-responsive">
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
                                        Carregando movimentacoes...
                                    </td>
                                </tr>
                            )}

                            {!carregando && historico.length === 0 && (
                                <tr>
                                    <td colSpan="5" className="text-center text-muted py-4">
                                        Nenhuma movimentacao registrada.
                                    </td>
                                </tr>
                            )}

                            {historico.map((item) => (
                                <tr key={item.id}>
                                    <td>{new Date(item.data).toLocaleString()}</td>
                                    <td>{item.nomeMaterial}</td>
                                    <td>
                                        <span className={`status-pill ${item.tipo === 'Saida' ? 'danger' : 'success'}`}>
                                            {item.tipo}
                                        </span>
                                    </td>
                                    <td>{item.quantidade}</td>
                                    <td>{item.observacao || '-'}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </section>
        </div>
    );
};

export default Movimentacoes;
