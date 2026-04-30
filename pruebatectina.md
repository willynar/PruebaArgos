📝 TEXTO (resumen fiel)
Prueba Técnica Core Transaccional

Angular + .NET / Node.js

1. Contexto del desafío

La empresa construye OmniERP, un sistema crítico.
Existe un Módulo de Edición de Pedidos con dos problemas graves en producción.

Tu misión: crear una Prueba de Concepto (PoC) que resuelva ambos problemas definiendo:

arquitectura
patrones
estrategias técnicas
Problema A: Cuello de botella en lecturas frecuentes

Dolor:

Para llenar dropdowns (Estados, Métodos de envío, etc.)
El frontend consulta constantemente a la base de datos
Estos datos cambian poco → se está saturando la red

Reto:

Simular que la consulta tarda ~2s
Diseñar solución para que luego responda en < 50ms
Sin castigar la infraestructura
Problema B: Pérdida de datos por colisión

Dolor:

Dos agentes editan el mismo pedido
Uno sobrescribe cambios del otro sin darse cuenta

Reto:

Implementar API + frontend
Detectar conflictos de edición
Evitar pérdida de datos
Dar feedback claro al usuario
2. Requerimientos técnicos
Backend
Elegir arquitectura (Clean, Hexagonal, MVC, etc.)
Elegir persistencia (SQL, NoSQL, memoria)
Se evalúa desacoplamiento y principios SOLID
Frontend
Angular 18+
Manejo de estado y arquitectura de componentes
Buen rendimiento
UI/UX
No importa CSS complejo
Sí importa comportamiento ante problemas
3. Entregables
Código en GitHub/GitLab
Ejecución local (docker-compose o README claro)
Archivo DECISIONS.md explicando:
Cómo resolviste Problema A
Cómo resolviste Problema B
Por qué elegiste esas soluciones
4. Sustentación técnica

Tendrás una entrevista donde explicarás:

Arquitectura
Decisiones técnicas
Alternativas y trade-offs
Adaptación a cambios
Conceptos de escalabilidad
🧠 ¿QUÉ TE ESTÁN PIDIENDO REALMENTE?

No es solo código. Están evaluando cómo piensas como ingeniero.

🔹 1. Resolver performance (Problema A)

👉 Lo que esperan:

No consultar la DB cada vez
Implementar algo como:
cache en memoria (ej: Redis o in-memory)
cache HTTP
singleton/shared data
lazy loading

👉 Idea clave:

“Datos que casi no cambian NO deben consultarse siempre”

🔹 2. Resolver concurrencia (Problema B)

👉 Lo que esperan:

Evitar sobrescrituras silenciosas

👉 Soluciones típicas:

Optimistic Locking (MUY probable que esperen esto)
campo version o updatedAt
Detectar conflicto en backend
Mostrar error en frontend

👉 Idea clave:

“Si alguien modificó el dato antes que tú, debes saberlo”

🔹 3. Diseñar arquitectura (MUY IMPORTANTE)

No quieren un CRUD simple.

Quieren ver:

separación de capas
código mantenible
decisiones claras

Ejemplo:

Controllers
Services
Repositories
Domain models
🔹 4. Pensar como sistema real

Te están evaluando en:

Tema	Qué buscan
Performance	cache inteligente
Concurrencia	control de versiones
Escalabilidad	decisiones razonadas
Diseño	código limpio
UX	manejo de errores
🧩 ¿Qué debería tener tu solución?
Backend
Endpoint de catálogos con cache
Endpoint de pedidos con control de versión
Simulación de latencia (2s)
Frontend
Formulario de pedido
Dropdowns optimizados
Manejo de error de concurrencia
Infra (opcional pero suma puntos)
Docker
Redis (🔥 bonus fuerte)
Documento
DECISIONS.md explicando TODO


base de datos:

Database name:	PruebaArgos
Full domain name:	PruebaArgos.mssql.somee.com
SQL Version:	MS SQL 2022 Exprless
Subscription:	Free hosting package (SPID1380285)
Service ID:	MPID5054801
Invoices:	INVID1495347
pasword: ixvb68hdz2
usuario: PruebaArgos_db
workstation id=PruebaArgos.mssql.somee.com;packet size=4096;user id=wnaranjoa_SQLLogin_1;pwd=ixvb68hdz2;data source=PruebaArgos.mssql.somee.com;persist security info=False;initial catalog=PruebaArgos;TrustServerCertificate=True