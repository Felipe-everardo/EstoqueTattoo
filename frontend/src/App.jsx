import { BrowserRouter as Router, NavLink, Routes, Route } from 'react-router-dom';
import Materiais from './pages/Materiais';
import Movimentacoes from './pages/Movimentacoes';
import Bancada from './pages/Bancada';

function App() {
    return (
        <Router>
            <div className="app-shell">
                <aside className="app-sidebar">
                    <div>
                        <div className="brand-block">
                            <span className="brand-mark">LT</span>
                            <div>
                                <strong>Lia Tattoo Art</strong>
                                <small>Studio inventory</small>
                            </div>
                        </div>

                        <nav className="app-nav" aria-label="Navegacao principal">
                            <span className="nav-section-label">Estoque</span>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/">
                                <span className="nav-indicator" />
                                Materiais
                            </NavLink>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/bancada">
                                <span className="nav-indicator" />
                                Bancada
                            </NavLink>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/movimentacoes">
                                <span className="nav-indicator" />
                                Movimentacoes
                            </NavLink>
                        </nav>
                    </div>

                    <div className="sidebar-footnote">
                        <span>Ambiente local</span>
                        <strong>API + SQLite</strong>
                    </div>
                </aside>

                <main className="app-main">
                    <Routes>
                        <Route path="/" element={<Materiais />} />
                        <Route path="/bancada" element={<Bancada />} />
                        <Route path="/movimentacoes" element={<Movimentacoes />} />
                    </Routes>
                </main>
            </div>
        </Router>
    );
}

export default App;
