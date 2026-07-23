import { useEffect, useState } from 'react';
import materialService from '../services/materialService';
import tintaService from '../services/tintaService';

const Bancada = () => {
    const [tintasEmUso, setTintasEmUso] = useState([]);
    const [materiaisEstoque, setMateriaisEstoque] = useState([]);
    const [materialSelecionado, setMaterialSelecionado] = useState('');
    const [carregando, setCarregando] = useState(true);
    const [salvando, setSalvando] = useState(false);
    const [erro, setErro] = useState('');
    const [sucesso, setSucesso] = useState('');

    const carregarDados = async () => {
        try {
            setErro('');
            const [dadosTintas, dadosMateriais] = await Promise.all([
                tintaService.listar(),
                materialService.listar()
            ]);

            setTintasEmUso(dadosTintas);
            setMateriaisEstoque(dadosMateriais);
        } catch (error) {
            console.error('Erro ao carregar bancada:', error);
            setErro('Não foi possível carregar a bancada. Confira se a API está rodando.');
        } finally {
            setCarregando(false);
        }
    };

    useEffect(() => {
        const carregarInicial = async () => {
            await carregarDados();
        };

        carregarInicial();
    }, []);

    const handleAbrirTinta = async () => {
        if (!materialSelecionado) {
            setErro('Selecione um item do estoque antes de abrir um frasco.');
            return;
        }

        try {
            setSalvando(true);
            setErro('');
            setSucesso('');
            await tintaService.abrir(Number(materialSelecionado));
            setMaterialSelecionado('');
            setSucesso('Tinta aberta e estoque atualizado.');
            await carregarDados();
        } catch (error) {
            console.error('Erro ao abrir tinta:', error);
            setErro('Não foi possível abrir a tinta. Verifique se existe estoque disponível.');
        } finally {
            setSalvando(false);
        }
    };

    const handleAtualizarNivel = (id, novoValor) => {
        setTintasEmUso((prev) => prev.map((tinta) =>
            tinta.id === id ? { ...tinta, porcentagemRestante: Number(novoValor) } : tinta
        ));
    };

    const handleSalvarNivel = async (id, valorFinal) => {
        try {
            setErro('');
            await tintaService.atualizarNivel(id, Number(valorFinal));
            setSucesso('Nível da tinta atualizado.');
        } catch (error) {
            console.error('Erro ao salvar nivel:', error);
            setErro('Não foi possível salvar o nível da tinta.');
            await carregarDados();
        }
    };

    const handleDescartar = async (id) => {
        try {
            setErro('');
            setSucesso('');
            await tintaService.descartar(id);
            setSucesso('Tinta removida da bancada.');
            await carregarDados();
        } catch (error) {
            console.error('Erro ao descartar tinta:', error);
            setErro('Não foi possível remover a tinta da bancada.');
        }
    };

    const getStatusClass = (valor) => {
        if (valor <= 20) return 'danger';
        if (valor <= 50) return 'warning';
        return 'success';
    };

    const tintasBaixas = tintasEmUso.filter((tinta) => tinta.porcentagemRestante <= 20).length;

    return (
        <div className="page-stack">
            <header className="page-header">
                <div>
                    <span className="eyebrow">Operação do estúdio</span>
                    <h1>Bancada de tintas e agulhas</h1>
                    <p>Controle as tintas e agulhas abertas, acompanhe consumo e retire frascos e agulhas que acabaram.</p>
                </div>
                <div className="header-actions">
                    <select
                        className="form-select control-input"
                        value={materialSelecionado}
                        onChange={(event) => setMaterialSelecionado(event.target.value)}
                    >
                        <option value="">Selecione um item do estoque</option>
                        {materiaisEstoque.map((material) => (
                            <option key={material.id} value={material.id}>
                                {material.nome} - saldo {material.quantidadeAtual}
                            </option>
                        ))}
                    </select>
                    <button className="btn btn-primary" onClick={handleAbrirTinta} disabled={salvando || carregando}>
                        {salvando ? 'Abrindo...' : 'Abrir frasco'}
                    </button>
                </div>
            </header>

            {erro && <div className="alert alert-danger">{erro}</div>}
            {sucesso && <div className="alert alert-success">{sucesso}</div>}

            <section className="stats-grid compact">
                <div className="stat-card">
                    <span>Tintas em uso</span>
                    <strong>{tintasEmUso.length}</strong>
                </div>
                <div className="stat-card">
                    <span>Precisam de atenção</span>
                    <strong>{tintasBaixas}</strong>
                </div>
            </section>

            {carregando && <div className="empty-state">Carregando bancada...</div>}

            {!carregando && tintasEmUso.length === 0 && (
                <div className="empty-state">
                    <h2>Bancada vazia</h2>
                    <p>Abra uma tinta do estoque para iniciar o controle de consumo.</p>
                </div>
            )}

            <section className="ink-grid">
                {tintasEmUso.map((tinta) => {
                    const status = getStatusClass(tinta.porcentagemRestante);

                    return (
                        <article key={tinta.id} className="ink-card">
                            <div className="ink-card-header">
                                <div>
                                    <h2>{tinta.tintaNome}</h2>
                                    <span>{tinta.categoria}</span>
                                </div>
                                <button className="btn btn-outline-danger btn-sm" onClick={() => handleDescartar(tinta.id)}>
                                    Descartar
                                </button>
                            </div>

                            <div className="meter">
                                <div className={`meter-fill meter-${status}`} style={{ width: `${tinta.porcentagemRestante}%` }}>
                                    {tinta.porcentagemRestante}%
                                </div>
                            </div>

                            <label className="form-label subtle-label">Nível restante</label>
                            <input
                                type="range"
                                className="form-range"
                                min="0"
                                max="100"
                                step="5"
                                value={tinta.porcentagemRestante}
                                onChange={(event) => handleAtualizarNivel(tinta.id, event.target.value)}
                                onMouseUp={(event) => handleSalvarNivel(tinta.id, event.target.value)}
                                onTouchEnd={(event) => handleSalvarNivel(tinta.id, event.target.value)}
                            />

                            <div className="ink-card-footer">
                                <span>Aberta em {new Date(tinta.dataAbertura).toLocaleDateString()}</span>
                                {tinta.porcentagemRestante <= 20 && <strong>Reposição em breve</strong>}
                            </div>
                        </article>
                    );
                })}
            </section>
        </div>
    );
};

export default Bancada;

