import { BrowserRouter as Router, NavLink, Routes, Route } from 'react-router-dom';
import Materiais from './pages/Materiais';
import Movimentacoes from './pages/Movimentacoes';
import Bancada from './pages/Bancada';

function App() {
    return (
        <Router>
            <div className="app-shell">
                <aside className="app-sidebar">
                    <div className="brand-block">
                        <span className="brand-mark">LT</span>
                        <div>
                            <strong>Lia Tattoo Art</strong>
                            <small>Controle de estoque</small>
                        </div>
                    </div>

                    <nav className="app-nav">
                        <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/">
                            Materiais
                        </NavLink>
                        <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/bancada">
                            Bancada
                        </NavLink>
                        <NavLink className={({ isActive }) => isActive ? 'app-nav-link active' : 'app-nav-link'} to="/movimentacoes">
                            Movimentacoes
                        </NavLink>
                    </nav>
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
