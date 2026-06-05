# Manual Técnico y de Evaluación: TechHelpWeb

Este documento detalla a profundidad el funcionamiento interno de la aplicación **TechHelpWeb** (Sistema de Gestión de Solicitudes), explicando la arquitectura elegida, las tecnologías aplicadas y comparando directamente cada módulo con las exigencias planteadas en la rúbrica oficial (archivo `evaluacion.md`).

---

## 1. Arquitectura y Tecnologías Aplicadas

El proyecto está diseñado bajo el patrón **MVC (Modelo-Vista-Controlador)** utilizando las siguientes tecnologías clave:

*   **ASP.NET MVC 5 (.NET Framework 4.8):** Sirve como núcleo de la aplicación, gestionando el enrutamiento de las páginas web (Front-End) y los controladores tradicionales que devuelven vistas Razor (`.cshtml`).
*   **ASP.NET Web API 2:** Integrada dentro de la misma solución para separar de manera moderna y asíncrona la lógica de negocio de las "Solicitudes de Soporte".
*   **SQL Server (Bases de Datos Relacionales):** Repositorio central de información con un modelo de datos altamente normalizado mediante llaves foráneas para garantizar la integridad referencial.
*   **Bootstrap v4/v5 y jQuery:** Motor del Front-End para asegurar que el sistema sea 100% responsivo, estilizado (tarjetas, tablas dinámicas) y capaz de hacer peticiones asíncronas (`fetch` / `AJAX`) al backend sin recargar la página.

---

## 2. Análisis Detallado por Módulos (Comparativa con la Rúbrica)

### A. Base de Datos Relacional (Cumplimiento 100%)

**Rúbrica Exige:** Tablas maestras, usuarios activos/inactivos, PKs y FKs completas.

**Cómo funciona en la aplicación:**
La base de datos fue modelada para contener entidades completamente independientes que nutren los menús desplegables del sistema. Las tablas maestras (Catálogos) son: `Roles`, `AreasSolicitantes`, `TiposProblema`, `Prioridades`, y `EstadosSolicitud`. 
*   La tabla `Usuarios` se conecta mediante `RolId` a los roles (Administrador, Operador/Soporte, Usuario).
*   La tabla `SolicitudesSoporte` concentra múltiples llaves foráneas (`UsuarioId`, `AreaId`, `TipoProblemaId`, etc.) asegurando que un operador no pueda eliminar un estado o área si hay tickets asociados, manteniendo la consistencia de los datos.

### B. Módulo de Autenticación y Perfil (MVC Puro)

**Rúbrica Exige:** *Restricción estricta*, NO utilizar API para la autenticación y perfil. Login, redirección por rol, edición de perfil y contraseñas.

**Cómo funciona en la aplicación:**
Para cumplir estrictamente con la rúbrica, todo lo referente a sesión y seguridad se maneja del lado del servidor (Server-Side Rendering) a través de controladores estándar de MVC (`Controller`), **no** mediante la API.

1.  **`AuthController`**: Controla el `Login`. Recibe credenciales (POST), las valida directamente contra la base de datos y genera la sesión (cookies de autenticación/variables de sesión). Al ingresar, intercepta el rol del usuario para redirigirlo a su vista correspondiente (ej. un administrador va al panel de usuarios, un operador al dashboard de tickets, y un usuario normal a "Mis Solicitudes").
2.  **`UsuariosController`**: Permite al Administrador gestionar las cuentas, bloqueando el acceso a cualquiera que no tenga dicho rol.
3.  **`PerfilController`**: Permite al usuario autenticado modificar su nombre y cambiar su contraseña.
    *   *Regla de Negocio Cumplida:* La vista de perfil bloquea el campo "Correo" usando el atributo `readonly` en HTML y omitiéndolo en el modelo de actualización del backend para asegurar su inmutabilidad.
    *   *Validaciones:* Se utilizan *Data Annotations* en los ViewModels (`[Required]`, `[EmailAddress]`, `[MinLength]`, `[Compare]`) para asegurar contraseñas seguras y que las nuevas coincidan exactamente, aplicando la validación tanto en el navegador (`jquery.validate`) como en el servidor (`ModelState.IsValid`).

### C. Módulo de Solicitudes de Soporte (API RESTful)

**Rúbrica Exige:** Uso exclusivo de API RESTful alojada en la misma solución para el CRUD de solicitudes. Endpoints HTTP específicos.

**Cómo funciona en la aplicación:**
A diferencia del módulo de perfil, las solicitudes de soporte se gestionan en el lado del cliente (Client-Side). Las vistas HTML (como `MisSolicitudes.cshtml` o `DashboardSoporte.cshtml`) se cargan "vacías" de datos de solicitudes, y es el navegador el que hace peticiones al controlador `SolicitudesApiController` (`ApiController`) mediante JavaScript.

**Mapeo de Endpoints (Cumplimiento Exacto):**
*   `GET /api/solicitudes`: El Javascript del Operador hace esta petición para pintar la tabla general. El backend retorna formato JSON. Si la llamada la hace un "Usuario", la lógica en el backend filtra automáticamente el query SQL o LINQ por el `UsuarioId` de su sesión.
*   `GET /api/solicitudes/{id}`: Utilizado para llenar un modal con el detalle completo de un ticket antes de editarlo.
*   `POST /api/solicitudes`: El formulario (dentro de una tarjeta "card") atrapa el evento `submit` con JS. 
    *   *Regla de Negocio Cumplida:* El formulario inyecta el ID del usuario en sesión, obligando a usar las listas desplegables. Verifica que la "Descripción" tenga al menos 10 caracteres. Por defecto asume el estado "Pendiente".
*   `PUT /api/solicitudes/{id}/estado`: Botones exclusivos en la vista del Operador disparan este endpoint enviando el nuevo estado.
*   `DELETE /api/solicitudes/{id}`: Disponible solo para el rol Operador. Muestra una alerta visual de confirmación antes de disparar la petición HTTP DELETE.

### D. Interfaz de Usuario (UX) y Patrones de Diseño

**Rúbrica Exige:** Formulario en contenedores/tarjetas, tabla dinámica, navegación simple, mensajes visuales y MVC limpio.

**Cómo funciona en la aplicación:**
*   **Separación de Responsabilidades:** Las Vistas (`Views/`) contienen exclusivamente maquetación HTML y código Razor mínimo. Toda la regla de negocio vive en los controladores y en el acceso a datos. No hay consultas SQL dentro del HTML.
*   **Estructura Visual:** Se hace uso extensivo de las clases de Bootstrap. 
    *   El formulario de creación de solicitudes reside dentro de un `<div class="card">`.
    *   Los listados se muestran en un `<table class="table table-hover">`.
*   **Feedback al Usuario:** Al crear un ticket vía API, la promesa asíncrona (JS) despliega un mensaje dinámico (`alert` o `toast`) indicando éxito o fracaso de la transacción, procediendo inmediatamente a repintar la tabla del historial, logrando una experiencia rápida y sin parpadeos de pantalla.

---

## 3. Conclusión de la Autoevaluación

La aplicación **TechHelpWeb** ha sido arquitectada tomando el archivo de evaluación como un plano estricto:

1. **Módulos Híbridos Exitosos:** Se demostró dominio separando un flujo MVC tradicional (Login/Perfil) de un flujo moderno SPA/API (Tickets).
2. **Seguridad de Roles:** Tanto las vistas web como la API RESTful se protegen de manera independiente (Un usuario no puede borrar tickets ni entrando a la ruta web, ni inyectando comandos cURL por la API).
3. **Reglas de Negocio:** Todos los validadores de campos obligatorios, contraseñas, y limitantes de correo estático están reforzados a nivel Frontend (UX) y Backend (Seguridad).
