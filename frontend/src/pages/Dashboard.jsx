import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import dashboardService from '../services/dashboardService';

const resumoInicial = {
    totalMateriais: 0,
    materiaisCriticos: 0,
    tintasEmUso: 0,
    tintasBaixas: 0,
    totalMovimentacoes: 0,
    ultimasMovimentacoes: []
};

const Dashboard = () => {
    const [resumo, setResumo] = useState(resumoInicial);
    const [carregando, setCarregando] = useState(true);
    const [erro, setErro] = useState('');

    const carregarResumo = async () => {
        try {
            setCarregando(true);
            setErro('');
            const dados = await dashboardService.resumo();
            setResumo(dados);
        } catch (error) {
            console.error('Erro ao carregar dashboard:', error);
            setErro('Nao foi possivel carregar o resumo. Confira se a API esta rodando.');
        } finally {
            setCarregando(false);
        }
    };

    useEffect(() => {
        carregarResumo();
    }, []);

    return (
        <div className="page-stack">
            <header className="page-header dashboard-hero">
                <div>
                    <span className="eyebrow">Visao geral</span>
                    <h1>Dashboard do estoque</h1>
                    <p>Acompanhe rapidamente os principais sinais operacionais do estudio: saldo, alertas, bancada e historico recente.</p>
                </div>
                <div className="header-actions compact-actions">
                    <button type="button" className="btn btn-outline-dark" onClick={carregarResumo} disabled={carregando}>
                        {carregando ? 'Atualizando...' : 'Atualizar'}
                    </button>
                    <Link className="btn btn-primary" to="/materiais">Ver estoque</Link>
                </div>
            </header>

            {erro && <div className="alert alert-danger">{erro}</div>}

            <section className="stats-grid dashboard-stats">
                <div className="stat-card">
                    <span>Materiais ativos</span>
                    <strong>{resumo.totalMateriais}</strong>
                </div>
                <div className="stat-card danger-card">
                    <span>Abaixo do minimo</span>
                    <strong>{resumo.materiaisCriticos}</strong>
                </div>
                <div className="stat-card">
                    <span>Itens na bancada</span>
                    <strong>{resumo.tintasEmUso}</strong>
                </div>
                <div className="stat-card warning-card">
                    <span>Bancada em alerta</span>
                    <strong>{resumo.tintasBaixas}</strong>
                </div>
                <div className="stat-card">
                    <span>Movimentacoes</span>
                    <strong>{resumo.totalMovimentacoes}</strong>
                </div>
            </section>

            <section className="surface-panel">
                <div className="panel-header">
                    <div>
                        <h2>Ultimas movimentacoes</h2>
                        <p>Resumo das operacoes mais recentes feitas no estoque e na bancada.</p>
                    </div>
                    <Link className="btn btn-outline-dark btn-sm" to="/movimentacoes">Historico completo</Link>
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
                                        Carregando resumo...
                                    </td>
                                </tr>
                            )}

                            {!carregando && resumo.ultimasMovimentacoes.length === 0 && (
                                <tr>
                                    <td colSpan="5" className="text-center text-muted py-4">
                                        Nenhuma movimentacao registrada.
                                    </td>
                                </tr>
                            )}

                            {!carregando && resumo.ultimasMovimentacoes.map((item) => (
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

export default Dashboard;
