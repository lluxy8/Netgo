import axios from 'axios';

let activeRequests = 0;
let setLoadingGlobal: ((state: boolean) => void) | null = null;

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE
});

export const registerLoadingHandler = (handler: (state: boolean) => void) => {
  setLoadingGlobal = handler;
};

api.interceptors.request.use(
  (config) => {
    activeRequests++;
    setLoadingGlobal?.(true);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    activeRequests--;
    if (activeRequests === 0) setLoadingGlobal?.(false);
    return response;
  },
  (error) => {
    activeRequests--;
    if (activeRequests === 0) setLoadingGlobal?.(false);
    return Promise.reject(error);
  }
);