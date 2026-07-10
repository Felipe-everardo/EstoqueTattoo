import { useEffect, useState } from 'react';
import materialService from '../services/materialService';
import movimentacaoService from '../services/movimentacaoService';

const materialInicial = {
    nome: '',
    categoriaId: '',
    quantidadeAtual: 0,
    quantidadeMinima: 0
};

const Materiais = () => {
    const [materiais, setMateriais] = useState([]);
    const [categorias, setCategorias] = useState([]);
    const [busca, setBusca] = useState('');
    const [mostrarModal, setMostrarModal] = useState(false);
    const [carregando, setCarregando] = useState(true);
    const [salvando, setSalvando] = useState(false);
    const [salvandoMovimentacao, setSalvandoMovimentacao] = useState(false);
    const [removendo, setRemovendo] = useState(false);
    const [erro, setErro] = useState('');
    const [sucesso, setSucesso] = useState('');
    const [novoMaterial, setNovoMaterial] = useState(materialInicial);
    const [materialParaRemover, setMaterialParaRemover] = useState(null);
    const [movimentacaoEstoque, setMovimentacaoEstoque] = useState(null);

    const carregarDados = async () => {
        try {
            setErro('');
            const [dadosMateriais, dadosCategorias] = await Promise.all([
                materialService.listar(),
                materialService.listarCategorias()
            ]);

            setMateriais(dadosMateriais);
            setCategorias(dadosCategorias);
        } catch (error) {
            console.error('Erro ao carregar materiais:', error);
            setErro('Nao foi possivel carregar os dados. Confira se a API esta rodando.');
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

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        const deveConverterParaNumero = name === 'categoriaId' || name.includes('quantidade');

        setNovoMaterial((prev) => ({
            ...prev,
            [name]: deveConverterParaNumero ? Number(value) : value
        }));
    };

    const handleSalvar = async (event) => {
        event.preventDefault();
        setSalvando(true);
        setErro('');
        setSucesso('');

        try {
            await materialService.criar({
                ...novoMaterial,
                categoriaId: Number(novoMaterial.categoriaId),
                quantidadeAtual: Number(novoMaterial.quantidadeAtual),
                quantidadeMinima: Number(novoMaterial.quantidadeMinima)
            });

            setMostrarModal(false);
            setNovoMaterial(materialInicial);
            setSucesso('Material cadastrado com sucesso.');
            await carregarDados();
        } catch (error) {
            console.error('Erro ao salvar:', error);
            setErro('Erro ao salvar material. Verifique os dados e tente novamente.');
        } finally {
            setSalvando(false);
        }
    };

    const handleRemover = async () => {
        if (!materialParaRemover) return;

        setRemovendo(true);
        setErro('');
        setSucesso('');

        try {
            await materialService.remover(materialParaRemover.id);
            setSucesso('Material removido com sucesso.');
            setMaterialParaRemover(null);
            await carregarDados();
        } catch (error) {
            console.error('Erro ao remover material:', error);
            setErro('Nao foi possivel remover o material. Tente novamente em alguns instantes.');
        } finally {
            setRemovendo(false);
        }
    };

    const abrirMovimentacaoEstoque = (material, tipo) => {
        setErro('');
        setSucesso('');
        setMovimentacaoEstoque({
            material,
            tipo,
            quantidade: 1,
            observacao: tipo === 'Entrada' ? 'Reposicao feita em materiais' : 'Baixa feita em materiais'
        });
    };

    const atualizarMovimentacaoEstoque = (campo, valor) => {
        setMovimentacaoEstoque((prev) => ({
            ...prev,
            [campo]: campo === 'quantidade' ? Number(valor) : valor
        }));
    };

    const handleSalvarMovimentacao = async (event) => {
        event.preventDefault();
        if (!movimentacaoEstoque) return;

        setSalvandoMovimentacao(true);
        setErro('');
        setSucesso('');

        try {
            await movimentacaoService.registrar({
                materialId: movimentacaoEstoque.material.id,
                quantidade: Number(movimentacaoEstoque.quantidade),
                tipo: movimentacaoEstoque.tipo,
                data: new Date().toISOString(),
                observacao: movimentacaoEstoque.observacao
            });

            setSucesso(`${movimentacaoEstoque.tipo} registrada automaticamente no historico.`);
            setMovimentacaoEstoque(null);
            await carregarDados();
        } catch (error) {
            console.error('Erro ao movimentar estoque:', error);
            setErro('Nao foi possivel movimentar o estoque. Verifique a quantidade disponivel e tente novamente.');
        } finally {
            setSalvandoMovimentacao(false);
        }
    };

    const materiaisFiltrados = materiais.filter((material) =>
        material.nome.toLowerCase().includes(busca.toLowerCase())
    );

    const itensBaixoEstoque = materiais.filter(
        (material) => material.quantidadeAtual <= material.quantidadeMinima
    ).length;

    return (
        <div className="page-stack">
            <header className="page-header">
                <div>
                    <span className="eyebrow">Inventario</span>
                    <h1>Materiais</h1>
                    <p>Consulte o saldo dos itens do estudio e cadastre novos materiais com categoria e estoque minimo.</p>
                </div>
                <div className="header-actions">
                    <input
                        type="text"
                        className="form-control control-input"
                        placeholder="Pesquisar material"
                        value={busca}
                        onChange={(event) => setBusca(event.target.value)}
                    />
                    <button className="btn btn-primary" onClick={() => setMostrarModal(true)}>
                        Novo material
                    </button>
                </div>
            </header>

            {erro && <div className="alert alert-danger">{erro}</div>}
            {sucesso && <div className="alert alert-success">{sucesso}</div>}

            <section className="stats-grid compact">
                <div className="stat-card">
                    <span>Total de itens</span>
                    <strong>{materiais.length}</strong>
                </div>
                <div className="stat-card">
                    <span>Abaixo do minimo</span>
                    <strong>{itensBaixoEstoque}</strong>
                </div>
            </section>

            <section className="surface-panel">
                <div className="panel-header">
                    <div>
                        <h2>Estoque atual</h2>
                        <p>Itens em vermelho precisam de reposicao ou revisao do minimo configurado.</p>
                    </div>
                </div>

                <div className="table-responsive">
                    <table className="table app-table mb-0">
                        <thead>
                            <tr>
                                <th>Material</th>
                                <th>Categoria</th>
                                <th>Qtd atual</th>
                                <th>Qtd minima</th>
                                <th>Status</th>
                                <th>Acoes</th>
                            </tr>
                        </thead>
                        <tbody>
                            {carregando && (
                                <tr>
                                    <td colSpan="6" className="text-center text-muted py-4">
                                        Carregando materiais...
                                    </td>
                                </tr>
                            )}

                            {!carregando && materiaisFiltrados.length === 0 && (
                                <tr>
                                    <td colSpan="6" className="text-center text-muted py-4">
                                        Nenhum material encontrado.
                                    </td>
                                </tr>
                            )}

                            {materiaisFiltrados.map((material) => {
                                const precisaRepor = material.quantidadeAtual <= material.quantidadeMinima;

                                return (
                                    <tr key={material.id}>
                                        <td><strong>{material.nome}</strong></td>
                                        <td>{material.nomeCategoria}</td>
                                        <td>{material.quantidadeAtual}</td>
                                        <td>{material.quantidadeMinima}</td>
                                        <td>
                                            <span className={`status-pill ${precisaRepor ? 'danger' : 'success'}`}>
                                                {precisaRepor ? 'Repor' : 'OK'}
                                            </span>
                                        </td>
                                        <td>
                                            <div className="d-flex gap-2 flex-wrap">
                                                <button
                                                    type="button"
                                                    className="btn btn-outline-success btn-sm"
                                                    onClick={() => abrirMovimentacaoEstoque(material, 'Entrada')}
                                                >
                                                    Entrada
                                                </button>
                                                <button
                                                    type="button"
                                                    className="btn btn-outline-dark btn-sm"
                                                    onClick={() => abrirMovimentacaoEstoque(material, 'Saida')}
                                                >
                                                    Saida
                                                </button>
                                                <button
                                                    type="button"
                                                    className="btn btn-outline-danger btn-sm"
                                                    onClick={() => setMaterialParaRemover(material)}
                                                >
                                                    Remover
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            </section>

            {mostrarModal && (
                <div className="modal-backdrop-custom">
                    <div className="surface-panel modal-panel">
                        <div className="panel-header">
                            <div>
                                <h2>Cadastrar material</h2>
                                <p>Preencha os dados principais usados pelo estoque.</p>
                            </div>
                            <button type="button" className="btn btn-outline-secondary btn-sm" onClick={() => setMostrarModal(false)}>
                                Fechar
                            </button>
                        </div>

                        <form onSubmit={handleSalvar}>
                            <div className="mb-3">
                                <label className="form-label">Nome do material</label>
                                <input
                                    type="text"
                                    name="nome"
                                    required
                                    className="form-control control-input"
                                    value={novoMaterial.nome}
                                    onChange={handleInputChange}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Categoria</label>
                                <select
                                    name="categoriaId"
                                    required
                                    className="form-select control-input"
                                    value={novoMaterial.categoriaId}
                                    onChange={handleInputChange}
                                >
                                    <option value="" disabled>Selecione uma categoria</option>
                                    {categorias.map((categoria) => (
                                        <option key={categoria.id} value={categoria.id}>
                                            {categoria.nome}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div className="row">
                                <div className="col-6 mb-3">
                                    <label className="form-label">Qtd atual</label>
                                    <input
                                        type="number"
                                        name="quantidadeAtual"
                                        required
                                        min="0"
                                        className="form-control control-input"
                                        value={novoMaterial.quantidadeAtual}
                                        onChange={handleInputChange}
                                    />
                                </div>
                                <div className="col-6 mb-3">
                                    <label className="form-label">Qtd minima</label>
                                    <input
                                        type="number"
                                        name="quantidadeMinima"
                                        required
                                        min="0"
                                        className="form-control control-input"
                                        value={novoMaterial.quantidadeMinima}
                                        onChange={handleInputChange}
                                    />
                                </div>
                            </div>

                            <div className="d-flex justify-content-end gap-2 mt-3">
                                <button type="button" className="btn btn-outline-secondary" onClick={() => setMostrarModal(false)}>
                                    Cancelar
                                </button>
                                <button type="submit" className="btn btn-primary" disabled={salvando}>
                                    {salvando ? 'Salvando...' : 'Salvar'}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {movimentacaoEstoque && (
                <div className="modal-backdrop-custom">
                    <div className="surface-panel modal-panel modal-panel-sm">
                        <div className="panel-header">
                            <div>
                                <h2>{movimentacaoEstoque.tipo} de estoque</h2>
                                <p>Essa acao atualiza o saldo e gera uma movimentacao automaticamente.</p>
                            </div>
                        </div>

                        <form onSubmit={handleSalvarMovimentacao}>
                            <div className="delete-summary mb-3">
                                <strong>{movimentacaoEstoque.material.nome}</strong>
                                <span>Saldo atual: {movimentacaoEstoque.material.quantidadeAtual}</span>
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Quantidade</label>
                                <input
                                    type="number"
                                    min="1"
                                    max={movimentacaoEstoque.tipo === 'Saida' ? movimentacaoEstoque.material.quantidadeAtual : undefined}
                                    required
                                    className="form-control control-input"
                                    value={movimentacaoEstoque.quantidade}
                                    onChange={(event) => atualizarMovimentacaoEstoque('quantidade', event.target.value)}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Observacao</label>
                                <input
                                    type="text"
                                    className="form-control control-input"
                                    value={movimentacaoEstoque.observacao}
                                    onChange={(event) => atualizarMovimentacaoEstoque('observacao', event.target.value)}
                                />
                            </div>

                            <div className="d-flex justify-content-end gap-2 mt-3">
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary"
                                    onClick={() => setMovimentacaoEstoque(null)}
                                    disabled={salvandoMovimentacao}
                                >
                                    Cancelar
                                </button>
                                <button type="submit" className="btn btn-primary" disabled={salvandoMovimentacao}>
                                    {salvandoMovimentacao ? 'Salvando...' : 'Confirmar'}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {materialParaRemover && (
                <div className="modal-backdrop-custom">
                    <div className="surface-panel modal-panel modal-panel-sm">
                        <div className="panel-header">
                            <div>
                                <h2>Remover material</h2>
                                <p>Essa acao remove o item da lista ativa, mas preserva o historico de movimentacoes.</p>
                            </div>
                        </div>

                        <div className="delete-summary">
                            <strong>{materialParaRemover.nome}</strong>
                            <span>{materialParaRemover.nomeCategoria}</span>
                        </div>

                        <div className="d-flex justify-content-end gap-2 mt-3">
                            <button
                                type="button"
                                className="btn btn-outline-secondary"
                                onClick={() => setMaterialParaRemover(null)}
                                disabled={removendo}
                            >
                                Cancelar
                            </button>
                            <button type="button" className="btn btn-danger" onClick={handleRemover} disabled={removendo}>
                                {removendo ? 'Removendo...' : 'Remover'}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Materiais;
