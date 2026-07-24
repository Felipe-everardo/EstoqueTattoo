import { useEffect, useState } from 'react';
import dashboardService from '../services/dashboardService';

const resumoInicial = {
    totalMateriais: 0,
    materiaisCriticos: 0,
    tintasEmUso: 0,
    tintasBaixas: 0,
    materiaisAtivos: [],
    materiaisAbaixoMinimo: [],
    itensBancada: [],
    itensBancadaEmAlerta: []
};

const indicadores = [
    {
        id: 'materiaisAtivos',
        titulo: 'Materiais ativos',
        contador: 'totalMateriais',
        lista: 'materiaisAtivos',
        tipo: 'material',
        descricao: 'Itens disponíveis no cadastro de materiais.'
    },
    {
        id: 'materiaisCriticos',
        titulo: 'Abaixo do mínimo',
        contador: 'materiaisCriticos',
        lista: 'materiaisAbaixoMinimo',
        tipo: 'material',
        descricao: 'Materiais que chegaram ao limite mínimo de estoque.',
        classe: 'danger-card'
    },
    {
        id: 'itensBancada',
        titulo: 'Itens na bancada',
        contador: 'tintasEmUso',
        lista: 'itensBancada',
        tipo: 'tinta',
        descricao: 'Frascos abertos e acompanhados na bancada.'
    },
    {
        id: 'bancadaAlerta',
        titulo: 'Bancada em alerta',
        contador: 'tintasBaixas',
        lista: 'itensBancadaEmAlerta',
        tipo: 'tinta',
        descricao: 'Itens da bancada com 20% ou menos.',
        classe: 'warning-card'
    }
];

const Dashboard = () => {
    const [resumo, setResumo] = useState(resumoInicial);
    const [carregando, setCarregando] = useState(true);
    const [erro, setErro] = useState('');
    const [indicadorAberto, setIndicadorAberto] = useState('');

    const carregarResumo = async () => {
        try {
            setCarregando(true);
            setErro('');
            const dados = await dashboardService.resumo();
            setResumo(dados);
        } catch (error) {
            console.error('Erro ao carregar dashboard:', error);
            setErro('Não foi possível carregar o resumo. Confira se a API está rodando.');
        } finally {
            setCarregando(false);
        }
    };

    useEffect(() => {
        carregarResumo();
    }, []);

    const indicadorSelecionado = indicadores.find((indicador) => indicador.id === indicadorAberto);
    const itensSelecionados = indicadorSelecionado ? resumo[indicadorSelecionado.lista] ?? [] : [];

    const alternarIndicador = (id) => {
        setIndicadorAberto((atual) => atual === id ? '' : id);
    };

    return (
        <div className="page-stack">
            <header className="page-header dashboard-hero">
                <div>
                    <span className="eyebrow">Visão geral</span>
                    <h1>Dashboard do estoque</h1>
                    <p>Acompanhe rapidamente os principais sinais do estoque e da bancada.</p>
                </div>
            </header>

            {erro && <div className="alert alert-danger">{erro}</div>}

            <section className="stats-grid dashboard-stats" aria-label="Indicadores do estoque">
                {indicadores.map((indicador) => {
                    const aberto = indicadorAberto === indicador.id;

                    return (
                        <button
                            key={indicador.id}
                            type="button"
                            className={`stat-card stat-card-button ${indicador.classe ?? ''} ${aberto ? 'is-active' : ''}`}
                            onClick={() => alternarIndicador(indicador.id)}
                            aria-expanded={aberto}
                            aria-controls="dashboard-details"
                            disabled={carregando}
                        >
                            <span>{indicador.titulo}</span>
                            <span className="stat-card-value">
                                <strong>{resumo[indicador.contador]}</strong>
                                <span className="stat-toggle-icon" aria-hidden="true">{aberto ? '−' : '+'}</span>
                            </span>
                        </button>
                    );
                })}
            </section>

            {indicadorSelecionado && (
                <section
                    id="dashboard-details"
                    className="surface-panel dashboard-details"
                    aria-live="polite"
                >
                    <div className="dashboard-details-header">
                        <div>
                            <span className="eyebrow">Itens do indicador</span>
                            <h2>{indicadorSelecionado.titulo}</h2>
                            <p>{indicadorSelecionado.descricao}</p>
                        </div>
                        <span className="details-count">
                            {itensSelecionados.length} {itensSelecionados.length === 1 ? 'item' : 'itens'}
                        </span>
                    </div>

                    {itensSelecionados.length === 0 ? (
                        <p className="dashboard-details-empty">Nenhum item neste estado no momento.</p>
                    ) : (
                        <div className="dashboard-item-list">
                            {itensSelecionados.map((item) => (
                                <div className="dashboard-item-row" key={item.id}>
                                    <div>
                                        <strong>{item.nome}</strong>
                                        <span>{item.categoria}</span>
                                    </div>
                                    {indicadorSelecionado.tipo === 'material' ? (
                                        <span className={item.quantidadeAtual <= item.quantidadeMinima ? 'item-state danger' : 'item-state'}>
                                            Estoque {item.quantidadeAtual} · mínimo {item.quantidadeMinima}
                                        </span>
                                    ) : (
                                        <span className={item.porcentagemRestante <= 20 ? 'item-state danger' : 'item-state'}>
                                            {item.porcentagemRestante}% restante
                                        </span>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}

                    <p className="dashboard-details-note">
                        Para alterar estes dados, use Materiais ou Bancada no menu.
                    </p>
                </section>
            )}
        </div>
    );
};

export default Dashboard;
