// Archivo generado automáticamente por scripts/set-env.js
export interface AppConfig {
  production: boolean;
  baseUrl: string;
  env: string;
}

export const environment: AppConfig = {
  production: false,
  baseUrl: 'https://localhost:44380/api',
  env: 'development'
};

// Alias para compatibilidad
export const apiConfig = environment;
