# Planificación y Requerimientos: Evaluación Final Unidad II (.NET)

## 1. Validación del Proyecto

Tus apuntes iniciales y la estructura planeada están **perfectamente alineados** con las exigencias de la rúbrica para alcanzar la nota máxima (100 puntos / Nota 7.0).

Has capturado los puntos críticos que suelen ser motivo de error, destacando especialmente:
* La restricción estricta de utilizar **MVC puro sin API** para la autenticación y el perfil.
* El mapeo exacto de las rutas HTTP requeridas para el `SolicitudesApiController`.
* Las validaciones específicas en el *front-end* y base de datos (como la longitud de la descripción y el estado "Pendiente").
* La correcta segmentación de reglas de negocio por Roles (Admin, Operador, Usuario).

---

## 2. Transcripción de Requerimientos Oficiales

A partir del documento de evaluación, estos son los requerimientos obligatorios categorizados para el sistema TechHelpWeb:

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
* **AuthController:** Manejo de Login y cierre de sesión. Debe incluir redirección lógica según `RolId`.
* **UsuariosController:** CRUD básico de usuarios y administración general (Exclusivo Administrador).
* **PerfilController:** Visualizar y actualizar datos del usuario autenticado. El campo correo no es editable.
* **Cambio de Contraseña:** Validar la contraseña actual, la nueva y su confirmación.
* **Validaciones:** Campos obligatorios, formato de correo válido y longitud mínima de contraseñas.

### Módulo de Solicitudes de Soporte (Exclusivo API RESTful)
La API RESTful se utilizará exclusivamente para el CRUD del soporte técnico.

**Endpoints obligatorios para `SolicitudesApiController`:**

| Método HTTP | Ruta | Función |
| :--- | :--- | :--- |
| **GET** | `/api/solicitudes` | Listar solicitudes (Todas para Operador, Filtradas por ID para Usuario) |
| **GET** | `/api/solicitudes/{id}` | Buscar una solicitud por ID |
| **POST** | `/api/solicitudes` | Registrar una nueva solicitud (Vista Usuario) |
| **PUT** | `/api/solicitudes/{id}/estado` | Actualizar el estado de una solicitud (Vista Operador) |
| **DELETE** | `/api/solicitudes/{id}` | Eliminar una solicitud (Vista Operador) |

**Campos obligatorios de la solicitud:**
* Nombre completo y Correo (autocompletados desde el perfil, inmutables en la vista).
* Área solicitante y Tipo de problema (Listas desplegables / FK).
* Descripción (Texto largo, mínimo 10 caracteres).
* Prioridad y Fecha de solicitud.
* Estado (Por defecto: Pendiente).

### Interfaz de Usuario y Experiencia (UX)
Elementos visuales requeridos para la vista dinámica de `SolicitudesController`:
* Formulario de registro en un contenedor o tarjeta (`card`).
* Tabla dinámica de historial de solicitudes.
* Botones de acción diferenciados en cada fila.
* Menú superior de navegación simple.
* Mensajes visuales claros (éxito/error).
* Diseño responsivo básico.

---

## 3. Checklist Definitivo de Desarrollo

### Configuración Inicial y Base de Datos (Relacional)
> *Asegúrate de que la estructura relacional esté sólida antes de codificar la lógica.*

- [ ] **Crear el proyecto:** Inicializar un proyecto web en **ASP.NET MVC**.
- [ ] **Diseño del Modelo Entidad-Relación:** Definir correctamente las llaves primarias (PK) y foráneas (FK).
- [ ] **Poblar Tablas Maestras (Catálogos):**
    - [ ] `Roles` (Administrador, Soporte, Usuario).
    - [ ] `AreasSolicitantes` (Administración, Docencia, Laboratorio, Biblioteca, Dirección, Otro).
    - [ ] `TiposProblema` (Problema con computador, internet, software, impresora, cuenta institucional, Otro).
    - [ ] `Prioridades` (Baja, Media, Alta).
    - [ ] `EstadosSolicitud` (Pendiente, En proceso, Resuelto).
- [ ] **Crear Tablas Principales:**
    - [ ] `Usuarios` (Relacionada a Roles; debe incluir campo para estado activo/inactivo).
    - [ ] `SolicitudesSoporte` (Relacionada al Usuario autenticado y a las tablas maestras).

### Módulo de Autenticación y Perfil (Puro MVC + Razor)
> *Restricción estricta: Este módulo se maneja directo en el servidor, NO debe usar endpoints de la API.*

- [ ] **`AuthController`:** Implementar acciones para **Login** y **Cierre de sesión**. Implementar redirección por rol.
- [ ] **`UsuariosController`:** (Vista Admin) Controlar la administración base de usuarios, roles y estados.
- [ ] **`PerfilController`:**
    - [ ] **Vista de Perfil:** Mostrar y permitir modificar los datos básicos (Correo debe ser de solo lectura).
    - [ ] **Cambio de Contraseña:** Crear formulario que valide *Contraseña actual*, *Nueva contraseña* y *Confirmación*.
- [ ] **Validaciones de Modelo (Data Annotations):**
    - [ ] Campos obligatorios asignados.
    - [ ] Formato de correo electrónico válido.
    - [ ] Longitud mínima de contraseña y coincidencia en la confirmación.

### Módulo API RESTful (Exclusivo para Soporte)
> *Controlador de API dentro del mismo proyecto dedicado únicamente al CRUD de solicitudes.*

- [ ] **`SolicitudesApiController`:** Configurar exactamente las siguientes rutas y métodos HTTP:
    - [ ] `GET /api/solicitudes` -> Listar solicitudes (Soporta filtro query param `?usuarioId=X`).
    - [ ] `GET /api/solicitudes/{id}` -> Buscar una solicitud específica por su ID.
    - [ ] `POST /api/solicitudes` -> Registrar una nueva solicitud.
    - [ ] `PUT /api/solicitudes/{id}/estado` -> Actualizar el estado de la solicitud.
    - [ ] `DELETE /api/solicitudes/{id}` -> Eliminar una solicitud del sistema.

### Interfaz de Usuario y Consumo de API
> *Desarrollo de la vista dinámica separada por roles.*

- [ ] **Vista Usuario (`Mis Solicitudes`):**
    - [ ] Renderizar Formulario de Registro consumiendo método `POST`.
    - [ ] Configurar Nombre y Correo para autocompletar (readonly).
    - [ ] Configurar dropdowns (Área, Problema, Prioridad) y validación de texto (min 10 chars).
    - [ ] Renderizar Tabla de Historial (filtrada) consumiendo `GET`. (Sin botones de acción).
- [ ] **Vista Operador (`Dashboard Soporte`):**
    - [ ] Renderizar Tabla Global consumiendo `GET`.
    - [ ] Agregar botones de acción por fila: **Actualizar Estado** (PUT) y **Eliminar** (DELETE).
- [ ] **Envío de Datos:** Programar consumo asíncrono con JavaScript (Fetch API).

### Diseño, Usabilidad y Rúbrica Final
> *Criterios estéticos y de orden del proyecto requeridos para la nota máxima.*

- [ ] **Estructura de la Interfaz:**
    - [ ] Menú superior simple y funcional (Navbar de navegación).
    - [ ] Vistas y formularios en contenedores visuales (`card`).
- [ ] **Experiencia de Usuario (UX):**
    - [ ] Configurar alertas visuales claras de éxito o error.
    - [ ] Asegurar diseño responsivo básico (Mobile Friendly).
- [ ] **Revisión de Arquitectura:** Verificar respeto al patrón MVC (separación de lógica y vistas).