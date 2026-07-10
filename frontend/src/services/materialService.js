import api from '../api/axiosConfig';

const materialService = {
    listar: async () => {
        const response = await api.get('/materiais');
        return response.data;
    },

    listarCategorias: async () => {
        const response = await api.get('/categorias');
        return response.data;
    },

    criar: async (material) => {
        const response = await api.post('/materiais', material);
        return response.data;
    },

    remover: async (id) => {
        const response = await api.delete(`/materiais/${id}`);
        return response.data;
    }
};

export default materialService;
