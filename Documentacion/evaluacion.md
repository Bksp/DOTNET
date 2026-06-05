# Planificación y Requerimientos: Evaluación Final Unidad II (.NET)

## 1. Validación del Proyecto
Los requerimientos y la estructura planeada están perfectamente alineados con las exigencias de la rúbrica para alcanzar la nota máxima (100 puntos / Nota 7.0). Puntos críticos a destacar:
* **Restricción estricta:** Utilizar MVC puro sin API para la autenticación y el perfil.
* **Mapeo exacto:** Rutas HTTP requeridas para el `SolicitudesApiController`.
* **Validaciones específicas:** Front-end y base de datos (longitud de descripción, estado "Pendiente").
* **Reglas de negocio por roles:** Correcta segmentación para Administrador, Operador y Usuario.

---

## 2. Transcripción de Requerimientos Oficiales (TechHelpWeb)

### Arquitectura y Tecnologías Base
* El sistema completo debe desarrollarse utilizando el framework ASP.NET MVC.
* Las vistas deben ser dinámicas y estar construidas con Razor.
* El proyecto debe incluir una API RESTful alojada dentro de la misma solución.
* El acceso a datos debe realizarse mediante una base de datos relacional.

### Base de Datos Relacional
La base de datos debe contener las siguientes tablas con sus respectivas llaves primarias y foráneas:
* **Roles:** Administrador, Soporte, Usuario.
* **Usuarios:** Relacionado con Roles; debe incluir un estado activo/inactivo.
* **AreasSolicitantes:** Administración, Docencia, Laboratorio, Biblioteca, Dirección, Otro.
* **TiposProblema:** Computador, internet, software, impresora, cuenta institucional, Otro.
* **Prioridades:** Baja, Media, Alta.
* **EstadosSolicitud:** Pendiente, En proceso, Resuelto.
* **SolicitudesSoporte:** Relacionada al Usuario y a todas las tablas maestras anteriores.

### Módulo de Autenticación y Perfil (MVC Puro)
Este módulo **no debe utilizar la API RESTful**, debe implementarse con controladores MVC, modelos y vistas Razor.
* **AuthController:** Manejo de Login y cierre de sesión. Lógica de redirección por rol.
* **UsuariosController:** CRUD básico de usuarios y administración general (Solo Administrador).
* **PerfilController:** Visualizar y actualizar datos del usuario autenticado. El correo es un dato estático/inmutable.
* **Cambio de Contraseña:** Validar la contraseña actual, la nueva y su confirmación.
* **Validaciones:** Campos obligatorios, formato de correo válido y longitud mínima de contraseñas.

### Módulo de Solicitudes de Soporte (Exclusivo API RESTful)
La API RESTful se utilizará exclusivamente para el CRUD del soporte técnico.

**Endpoints obligatorios para `SolicitudesApiController`**:

| Método HTTP | Ruta | Función |
| :--- | :--- | :--- |
| **GET** | `/api/solicitudes` | Listar todas las solicitudes (Filtro por ID si es Usuario normal). |
| **GET** | `/api/solicitudes/{id}` | Buscar una solicitud por ID. |
| **POST** | `/api/solicitudes` | Registrar una nueva solicitud (Solo Usuario normal). |
| **PUT** | `/api/solicitudes/{id}/estado` | Actualizar el estado de una solicitud (Solo Operador). |
| **DELETE** | `/api/solicitudes/{id}` | Eliminar una solicitud (Solo Operador). |

**Campos obligatorios de la solicitud**:
* Nombre completo y Correo (autocompletados desde el perfil, inmutables).
* Área solicitante y Tipo de problema (Listas desplegables / FK).
* Descripción (Texto largo, mínimo 10 caracteres).
* Prioridad y Fecha de solicitud.
* Estado (Por defecto: Pendiente).

### Interfaz de Usuario y Experiencia (UX)
Elementos visuales requeridos para la vista dinámica de `SolicitudesController`:
* Formulario de registro en un contenedor o tarjeta (card).
* Tabla dinámica de historial de solicitudes.
* Botones de acción diferenciados en cada fila.
* Menú superior de navegación simple.
* Mensajes visuales claros (éxito/error).
* Diseño responsivo básico.

---

## 3. Checklist Definitivo de Desarrollo

### Configuración Inicial y Base de Datos (Relacional)
*Asegúrate de que la estructura relacional esté sólida antes de codificar la lógica.*
* [ ] Crear el proyecto: Inicializar un proyecto web en ASP.NET MVC.
* [ ] Diseño del Modelo Entidad-Relación: Definir correctamente las llaves primarias (PK) y foráneas (FK).
* [ ] Tablas Maestras (Catálogos): Roles, AreasSolicitantes, TiposProblema, Prioridades, EstadosSolicitud.
* [ ] Tabla Usuarios: Relacionada a Roles; debe incluir campo para estado activo/inactivo.
* [ ] Tabla SolicitudesSoporte: Relacionada al Usuario autenticado y a las tablas maestras.

### Módulo de Autenticación y Perfil (Puro MVC + Razor)
*Restricción estricta: Este módulo se maneja directo en el servidor, NO debe usar endpoints de la API.*
* [ ] `AuthController`: Login, cierre de sesión y redirección según RolId.
* [ ] `UsuariosController`: Panel exclusivo del Administrador para CRUD de cuentas de usuario.
* [ ] Vista de Perfil: Mostrar y permitir modificar los datos básicos (bloqueando el campo correo como `readonly`).
* [ ] Cambio de Contraseña: Validar Contraseña actual, Nueva contraseña y Confirmación.
* [ ] Validaciones de Modelo (Data Annotations): Campos obligatorios, formato email, longitud mínima.

### Módulo API RESTful (Exclusivo para Soporte)
*Controlador de API dentro del mismo proyecto dedicado únicamente al CRUD de solicitudes.*
* [ ] `GET /api/solicitudes` -> Listar solicitudes (todas para operador, filtradas para usuario).
* [ ] `GET /api/solicitudes/{id}` -> Buscar una solicitud específica por su ID.
* [ ] `POST /api/solicitudes` -> Registrar una nueva solicitud.
* [ ] `PUT /api/solicitudes/{id}/estado` -> Actualizar el estado de la solicitud.
* [ ] `DELETE /api/solicitudes/{id}` -> Eliminar una solicitud del sistema.

### Interfaz de Usuario y Consumo de API (Por Rol)
*Desarrollo de las vistas dinámicas donde los usuarios interactúan con el sistema.*
* [ ] **Vista Operador:** Renderizar tabla global (GET) con botones de Actualizar (PUT) y Eliminar (DELETE).
* [ ] **Vista Usuario:** Renderizar formulario (POST) y tabla personal (GET filtrado) sin botones de borrado.
* [ ] Formulario de Registro: Autocompletar datos del usuario, dropdowns requeridos, descripción min. 10 caracteres.
* [ ] Envío de Datos (JavaScript / Fetch): Consumir los métodos de la API de forma asíncrona.

### Diseño, Usabilidad y Rúbrica Final
*Criterios estéticos y de orden del proyecto requeridos para la nota máxima.*
* [ ] Estructura de la Interfaz: Menú de navegación, tarjetas (card) y tablas claras.
* [ ] Experiencia de Usuario (UX): Alertas visuales y diseño responsivo básico.
* [ ] Revisión de Arquitectura: Respetar limpiamente el patrón MVC, manteniendo la lógica separada de las vistas.