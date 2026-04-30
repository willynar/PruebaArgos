const { writeFile } = require('fs');
require('dotenv').config({ path: process.argv.includes('--environment=prod') ? '.env.production' : '.env' });

const isProduction = process.argv.includes('--environment=prod');

// El archivo consolidado como única fuente de verdad
const targetPath = `./src/app/services/api.config.ts`;

const envConfigFile = `// Archivo generado automáticamente por scripts/set-env.js
export interface AppConfig {
  production: boolean;
  baseUrl: string;
  env: string;
}

export const environment: AppConfig = {
  production: ${isProduction},
  baseUrl: '${process.env.ANGULAR_APP_BASE_URL || process.env.ANGULAR_APP_API_URL || (isProduction ? '/api' : 'http://localhost:8080/api')}',
  env: '${process.env.ANGULAR_APP_ENV || (isProduction ? 'production' : 'development')}'
};

// Alias para compatibilidad
export const apiConfig = environment;
`;

console.log(`Consolidando configuración en ${targetPath}...`);

writeFile(targetPath, envConfigFile, function (err) {
   if (err) {
       throw console.error(err);
   } else {
       console.log(`Configuración de la aplicación consolidada correctamente.`);
   }
});
