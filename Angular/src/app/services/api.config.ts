import { environment as envConfig } from '../../environments/environment';

// Re-exportar environment para compatibilidad con servicios existentes
export const environment = envConfig;

// Exportar como apiConfig también
export const apiConfig = {
  baseUrl: environment.apiUrl,
  production: environment.production,
  env: environment.env
};
