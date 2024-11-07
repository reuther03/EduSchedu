import axios from 'axios'
import tokenService from '../../../../EduSchedu/client/src/services/tokenService'

const axiosService = axios.create({
  baseURL: 'http://localhost:5000/',
});

axiosService.interceptors.request.use(
  config => {
    const token = tokenService.getToken()
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

export default axiosService;
