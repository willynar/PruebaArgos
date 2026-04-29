## Arquitectura del Backend

Para estructurar la solución, opté por una **arquitectura en capas lógicas** utilizando **ASP.NET Core 8 API**. 

* **Desacoplamiento:** Decidí usar interfaces (`IOrderService`, `ICatalogsService`) y un archivo centralizado para la inyección de dependencias (`ConfigurationRepositories.cs`). Esto me permitió separar completamente la lógica de negocio de los controladores HTTP.
* **Persistencia:** Implementé **Entity Framework Core (Code-First)** con SQL Server. Creé las entidades (`Orders`, `OrderStatuses`, `ShippingMethods`) en inglés, ya que considero que es una buena práctica y el estándar de la industria mantener el código en el mismo idioma que el framework.
* **Principios SOLID:** Me aseguré de aislar el acceso a datos en la capa `Logic`. De esta forma, mis controladores quedaron súper limpios y con una única responsabilidad: recibir peticiones y devolver respuestas.

---

## Solución al Problema A: Cuello de Botella en Lecturas Frecuentes

**El reto:** Los endpoints que traen los catálogos (como los estados y métodos de envío) simulaban tener latencia y saturaban la red por las lecturas constantes desde el frontend.

**Objetivo:** Primera carga ~2000ms, subsecuentes < 50ms

### Backend - ASP.NET Core 8 (Output Caching)
**Mi enfoque:**
Me di cuenta de que estos catálogos son datos que rara vez cambian, así que implementé **Output Caching**, que es una característica nativa y muy potente de .NET 8. de la cual habia echo uso en el pasado en otros proyectos, me parecio la mejor solucion para este problema, ya que permite cachear respuestas HTTP basandose en parametros y headers, lo que lo hace ideal para este caso.

* **Por qué lo elegí:** A diferencia del caché en memoria tradicional que requiere lógica manual dentro del servicio, el OutputCache intercepta la petición a nivel del middleware HTTP. Si la data está cacheada, el servidor responde en milisegundos sin siquiera tocar el controlador ni la base de datos, lo que le quita una carga gigante a la infraestructura, ademas de que es una solucion muy limpia y facil de mantener.
* **Estrategia de limpieza:** Le asigné etiquetas (`Tags`) a estas políticas de caché. Además, dejé preparado un endpoint (`POST /api/Catalogs/clear-cache`) que usa `EvictByTagAsync` para invalidar la caché bajo demanda. Así, si alguna vez actualizamos un estado en la base de datos, podemos limpiar la caché instantáneamente.

### Frontend - Angular 18 (In-Memory Service Caching)
**Mi enfoque complementario:**
Implementé un **caché en memoria con versionado temporal** en el servicio `CatalogoService` para garantizar rendimiento óptimo desde el cliente:

**Técnica utilizada:**
- **`shareReplay({ bufferSize: 1, refCount: false })`** - Persiste el observable cacheado incluso sin subscribers activos, asegurando reutilización de la respuesta
- **`tap()`** - Registra timestamp para saber exactamente cuándo se cacheó cada consulta
- **Validación de edad del caché** - Si el caché tiene más de 60 segundos, se invalida automáticamente (evita datos obsoletos)
- **Control manual** - Botón "Actualizar" permite al usuario forzar una recarga limpiando el caché bajo demanda

**Flujo de ejecución:**
```
Primera petición -> HTTP + 2000ms delay -> shareReplay guarda resultado
Segunda petición -> Verifica timestamp -> Si < 60s devuelve caché (< 50ms) -> Sin HTTP
Después de 60s -> Cache expirado -> Siguiente petición hace HTTP nuevamente
Usuario hace click en "Actualizar" -> Cache limpiado manualmente -> Próxima petición hace HTTP
```

**Resultado:**
El usuario ve las opciones de dropdown en milisegundos en subsecuentes visitas, y el servidor no recibe peticiones innecesarias para datos que no cambian. La combinación de Output Caching backend + Service Caching frontend garantiza máximo rendimiento.

---

## Solución al Problema B: Pérdida de Datos por Colisión (Concurrencia)

**El reto:** Evitar que dos agentes que están editando un mismo pedido al mismo tiempo se sobrescriban la información mutuamente sin darse cuenta.

**Objetivo:** Detectar conflictos y mostrar error `409 Conflict` sin perder datos

### Backend - Entity Framework Optimistic Locking
**Mi enfoque:**
Para este escenario, me incliné por usar el patrón de **Bloqueo Optimista (Optimistic Locking)**.

* **Por qué lo elegí:** A diferencia del bloqueo pesimista (que cierra registros), el optimista es más eficiente. EF Core maneja automáticamente la concurrencia a través de versionado. me  fui por una solucion que me sugirieron varias ia y me parecio bastante interesando con la integracion de entity framework  el como maneja RowVersion pra versionar los datos algo genial nunca lo habia pensado de esa forma

* **Cómo lo implementé:** 
  - Agregué columna `RowVersion` (tipo `byte[]`, decorada con `[Timestamp]`) a la tabla `Orders`
  - EF Core incrementa automáticamente esta versión con cada UPDATE
  - En el flujo de edición: Cliente obtiene pedido con su versión, intenta guardar enviando esa versión
  - En el servidor: Valido que la versión coincida ANTES de actualizar
  - Si no coincide → Alguien más modificó el registro → Excepción `DbUpdateConcurrencyException` → HTTP 409

* **Experiencia de usuario:** 
  - Si detecto una colisión, atrapo la excepción `DbUpdateConcurrencyException` y devuelvo código HTTP `409 Conflict`
  - El Frontend captura el 409 y muestra una alerta clara indicando: *"Otro usuario modificó este pedido. Recarga los cambios y vuelve a intentar"*
  - Esto obliga al usuario a recargar los datos actualizados antes de perder su trabajo

### Frontend - Angular Error Handling
**Flujo en el componente:**
```
Usuario intenta guardar pedido
-> Envía orden con rowVersion actual
-> Backend rechaza: 409 Conflict
-> Frontend captura error
-> Muestra: "Error: Conflicto de concurrencia. Recargando datos..."
-> Auto-recarga la lista de pedidos
-> Usuario ve los cambios del otro agente
-> Puede intentar de nuevo con datos frescos
```

---

## Ejecución Local

Para revisar y levantar el proyecto de forma local, preparé todo para que sea muy sencillo. (La cadena de conexión ya está apuntando a la base de datos SQL remota en Somee.com, así que no necesitas instalar ni configurar un servidor de base de datos local).

### Opción 1: Docker Compose (Recomendada)
Si prefieres usar Docker, solo ejecuta esto en la terminal desde la raíz del proyecto:
```bash
docker-compose up --build
```
- Backend API: `http://localhost:8080`
- Frontend: `http://localhost`
- Swagger: `http://localhost:8080/swagger`

### Opción 2: SDK nativo
#### Backend (.NET 8)
```bash
cd POCArgos
dotnet restore
dotnet run
```
Acceder a Swagger en `https://localhost:5001/swagger`

#### Frontend (Angular 18)
```bash
cd Angular
npm install
npm start
```
Acceder a `http://localhost:4200`

---

## Decisiones clave

| Aspecto | Decisión | Justificación |
|---------|----------|--------------|
| **Backend Performance** | Output Caching (ASP.NET) | Nativo, eficiente, fácil mantenimiento |
| **Frontend Caching** | RxJS shareReplay + timestamp | Persistencia de caché + validación de edad |
| **Concurrencia** | Optimistic Locking (RowVersion) | Eficiente, automático en EF, escalable |
| **Arquitectura** | Capas + Interfaces + DI | SOLID, testeable, mantenible |
| **Persistencia** | EF Core Code-First | Type-safe, migrations automáticas |
| **Infraestructura** | Docker + docker-compose | Reproducible, aislado, fácil deploy |
