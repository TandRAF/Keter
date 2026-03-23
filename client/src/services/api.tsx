import axios from 'axios';

export const api = axios.create({
  baseURL: 'http://127.0.0.1:5001/api', 
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
  
    if (error.response && error.response.status === 401) {
      console.warn("Session expired or user deleted. Kicking to login...");
      
      localStorage.removeItem('user');
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);