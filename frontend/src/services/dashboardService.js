import api from '../api/axiosConfig';

const dashboardService = {
    resumo: async () => {
        const response = await api.get('/dashboard/resumo');
        return response.data;
    }
};

export default dashboardService;
