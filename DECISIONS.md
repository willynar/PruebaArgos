# Bitácora de Decisiones Técnicas - PoC Argos

Este documento resume las decisiones arquitectónicas y soluciones implementadas durante el desarrollo de la prueba de concepto para el sistema de gestión de pedidos.

## 1. Arquitectura del Sistema

Se optó por una arquitectura desacoplada que separa claramente la responsabilidad de presentación de la lógica de negocio y persistencia.

### Backend (ASP.NET Core 8 API)
*   **Arquitectura en Capas:** Estructura de capas lógicas (Controllers -> Services/Logic -> Data) para facilitar el mantenimiento.
*   **Inyección de Dependencias (DI):** Uso de interfaces centralizadas en `ConfigurationRepositories.cs` para desacoplar componentes.
*   **Persistencia:** Entity Framework Core con enfoque **Code-First**. 
*   **Principios SOLID:** Controladores delgados que delegan la lógica de negocio a la capa de servicios. No se separaron en bibliotecas de clases externas debido al tamaño actual del proyecto, pero la estructura permite hacerlo fácilmente en el futuro.

### Frontend (Angular 18)
*   **Standalone Components:** Arquitectura moderna de componentes independientes, eliminando módulos innecesarios y simplificando dependencias.
*   **Reactive UI:** Implementación de formularios reactivos y gestión de estado local para controlar la UI (cargas, errores, ediciones) de forma eficiente.

---

## 2. Optimización de Rendimiento (Lecturas de Catálogos)

**Problema:** Los catálogos presentaban una latencia notable al conectar con la base de datos remota, afectando la experiencia de usuario en la carga de formularios.

**Solución Implementada: Server-Side Output Caching**

Se decidió centralizar la estrategia de optimización en el Backend para simplificar el cliente y garantizar la consistencia de los datos.

*   **Tecnología:** Se utilizó el middleware de **Output Caching** nativo de .NET 8.
*   **Estrategia:** Los endpoints de catálogos (`order/status` y `shipping/methods`) están protegidos por políticas de caché con expiración de 3 horas.
*   **Invalidación:** Se implementó un endpoint `POST /api/Catalogs/clear/cache` que permite invalidar el caché bajo demanda mediante tags (`EvictByTagAsync`), útil si los datos maestros llegaran a cambiar.
*   **Resultado:** El Frontend realiza peticiones estándar que, tras la primera carga, son resueltas en milisegundos por el servidor sin tocar la base de datos, eliminando la latencia percibida por el usuario.

---

## 3. Gestión de Concurrencia (Edición Simultánea)

**Problema:** Riesgo de "Lost Update" cuando múltiples agentes editan el mismo pedido.

**Solución: Optimistic Locking**

### Implementación en Backend (EF Core)
*   Se agregó una propiedad `RowVersion` (`byte[]` con `[Timestamp]`) a la entidad `Order`.
*   SQL Server incrementa este valor automáticamente en cada actualización.
*   EF Core detecta si el valor ha cambiado desde la lectura inicial y lanza una `DbUpdateConcurrencyException` si hay un conflicto.

### Gestión de Errores
*   **API:** Retorna un código **HTTP 409 Conflict** ante una colisión de datos. ademas de que limpia todos los errores en el api a lo mas basico para no devolver demaciada data como rutas etc 
*   **Frontend:** El servicio detecta el error 409 y el componente muestra una alerta específica al usuario: *"Conflicto: Otro agente modificó este pedido. Por favor recarga los datos."*, evitando que se sobrescriban cambios de forma silenciosa.

---

## 4. Infraestructura y Despliegue Local

*   **Docker Compose:** Orquestación para levantar la API (.NET) y el Cliente (Angular + Nginx) en un entorno aislado y reproducible.
*   **Base de Datos:** Configurada para conectar a SQL Server remoto, facilitando el inicio rápido sin bases de datos locales.

### Comandos
```bash
docker-compose up --build
```
