# Docker Compose - Multi-container configuration

## Structure
```
PruebaArrgos/
├── docker-compose.yml          # Orquestación de contenedores
├── Angular/
│   ├── Dockerfile              # Build de Angular
│   ├── nginx.conf              # Configuración de Nginx
│   ├── .env                    # Variables de entorno
│   └── ...
└── POCArgos/
    ├── Dockerfile              # Build del backend .NET
    └── ...
```

## Para ejecutar todo:

```bash
# Construir y ejecutar (desde la raíz del proyecto)
docker-compose up -d

# Ver logs
docker-compose logs -f

# Detener
docker-compose down

# Reconstruir
docker-compose up -d --build
```

## Acceso:
- Frontend: http://localhost
- Backend API: http://localhost:8080
- Backend API (desde container): http://backend-api:8080

## Variables de entorno en Docker:

En el `docker-compose.yml`, la sección `frontend-app` pasa automáticamente:
- `ANGULAR_APP_API_URL`: http://backend-api:8080/api (dentro de Docker)
- `ANGULAR_APP_PRODUCTION`: true
- `ANGULAR_APP_ENV`: production

## Networking:

Ambos servicios están en la misma red `app-network`, permitiendo:
- Frontend contactar Backend vía `http://backend-api:8080`
- Nginx hacer proxy a `http://backend-api:8080/api`

## Health Check:

El backend tiene un health check que verifica cada 30s. El frontend espera a que el backend esté sano antes de iniciar.

## Proxy API:

El nginx.conf hace proxy de todas las peticiones `/api` al backend:
```nginx
location /api {
    proxy_pass http://backend-api:8080;
}
```

Esto permite que el frontend acceda a `http://localhost/api/catalogs` 
y se redirija a `http://backend-api:8080/api/catalogs` internamente.
