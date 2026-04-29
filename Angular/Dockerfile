# Stage 1: Build
FROM node:20-alpine AS build

WORKDIR /app

# Copiar package.json y package-lock.json
COPY package*.json ./

# Instalar dependencias
RUN npm ci

# Copiar código fuente
COPY . .

# Build de producción
ARG ANGULAR_APP_API_URL=https://localhost:44380/api
ARG ANGULAR_APP_PRODUCTION=true
ARG ANGULAR_APP_ENV=production

ENV ANGULAR_APP_API_URL=$ANGULAR_APP_API_URL
ENV ANGULAR_APP_PRODUCTION=$ANGULAR_APP_PRODUCTION
ENV ANGULAR_APP_ENV=$ANGULAR_APP_ENV

RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copiar configuración de nginx
COPY nginx.conf /etc/nginx/nginx.conf

# Copiar archivos compilados de Angular
COPY --from=build /app/dist/omni-erp-frontend/browser /usr/share/nginx/html

# Exponer puerto
EXPOSE 80

# Comando de inicio
CMD ["nginx", "-g", "daemon off;"]
