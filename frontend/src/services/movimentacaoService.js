import api from '../api/axiosConfig';

const movimentacaoService = {
    listar: async () => {
        const response = await api.get('/movimentacoes');
        return response.data;
    },

    registrar: async (movimentacao) => {
        const response = await api.post('/movimentacoes', movimentacao);
        return response.data;
    }
};

export default movimentacaoService;
