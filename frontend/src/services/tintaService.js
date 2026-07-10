import api from '../api/axiosConfig';

const tintaService = {
    listar: async () => {
        const response = await api.get('/tintas');
        return response.data;
    },

    abrir: async (tintaId) => {
        const response = await api.post('/tintas/abrir', { tintaId });
        return response.data;
    },

    atualizarNivel: async (idBancada, novaPorcentagem) => {
        const response = await api.put('/tintas/atualizar', { id: idBancada, novaPorcentagem });
        return response.data;
    },

    descartar: async (idBancada) => {
        const response = await api.delete(`/tintas/${idBancada}`);
        return response.data;
    }
};

export default tintaService;
