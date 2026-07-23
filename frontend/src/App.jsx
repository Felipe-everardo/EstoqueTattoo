import { BrowserRouter as Router, NavLink, Routes, Route } from 'react-router-dom';
import Materiais from './pages/Materiais';
import Movimentacoes from './pages/Movimentacoes';
import Bancada from './pages/Bancada';
import Dashboard from './pages/Dashboard';
import Logo from "./assets/LiaGato.png";

function App() {
    return (
        <Router>
            <div className="app-shell">
                <aside className="app-sidebar">
                    <div>
                        <div className="brand-block">
                            <img
                                className="brand-mark"
                                src={Logo}
                                alt="Logo Lia Tattoo Art"
                            />
                            <div>
                                <strong className="eyebrow">Lia TattooArt</strong>
                                <small>Studio inventory</small>
                            </div>
                        </div>

                        <nav className="app-nav" aria-label="Navegação principal">
                            <span className="nav-section-label">Estoque</span>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/">
                                <span className="nav-indicator" />
                                Dashboard
                            </NavLink>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/materiais">
                                <span className="nav-indicator" />
                                Materiais
                            </NavLink>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/bancada">
                                <span className="nav-indicator" />
                                Bancada
                            </NavLink>
                            <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/movimentacoes">
                                <span className="nav-indicator" />
                                Movimentações
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
                        <Route path="/" element={<Dashboard />} />
                        <Route path="/materiais" element={<Materiais />} />
                        <Route path="/bancada" element={<Bancada />} />
                        <Route path="/movimentacoes" element={<Movimentacoes />} />
                    </Routes>
                </main>
            </div>
        </Router>
    );
}

export default App;
