# TechHelp - Sistema de Gestión de Solicitudes (Entrega 3)

Este proyecto es una aplicación web desarrollada en **ASP.NET MVC (.NET Framework 4.8)** para la gestión de usuarios y solicitudes de soporte técnico.

## Características Principales

* **Gestión de Usuarios:** Creación, edición y listado de usuarios.
* **Autenticación:** Sistema de inicio de sesión (Login) y gestión de perfiles.
* **Módulo de Solicitudes:** Creación y seguimiento de solicitudes de soporte.
* **Dashboard de Soporte:** Panel para la administración de los tickets de ayuda.
* **API REST:** Controladores de API (`SolicitudesApiController`) para integración de datos.

## Tecnologías Utilizadas

* **Backend:** C# con ASP.NET MVC 5
* **Frontend:** HTML5, CSS3, Bootstrap (v5/v4), jQuery y Razor Views (`.cshtml`)
* **Base de Datos:** SQL Server (Entity Framework / ADO.NET)
* **Entorno de Desarrollo:** Visual Studio

## Estructura del Proyecto

* `Controllers/`: Contiene la lógica de enrutamiento y manejo de peticiones (`AuthController`, `UsuariosController`, `SolicitudesController`, etc.).
* `Views/`: Vistas de la aplicación generadas con el motor Razor (Usuarios, Solicitudes, Perfil, etc.).
* `Models/`: Definición de entidades, ViewModels y el contexto de base de datos (`TechHelpDbContext`).
* `Scripts/` y `Content/`: Archivos estáticos como JavaScript (jQuery, Bootstrap) y CSS.

## Requisitos Previos

* Visual Studio 2019 o 2022 (con la carga de trabajo "Desarrollo de ASP.NET y web").
* .NET Framework 4.8.
* SQL Server (LocalDB o instancia completa).

## Instrucciones de Instalación y Ejecución

1. **Clonar el repositorio:**
   ```bash
   git clone git@github.com:Bksp/DOTNET.git
   ```
2. **Abrir el proyecto:**
   Abre el archivo de la solución (`.slnx` o `.sln`) o directamente la carpeta del proyecto en Visual Studio.
3. **Restaurar Paquetes NuGet:**
   En Visual Studio, haz clic derecho sobre la solución en el Explorador de Soluciones y selecciona "Restaurar paquetes NuGet" (o compila el proyecto para que se restauren automáticamente).
4. **Configurar la Base de Datos:**
   * Ejecuta el script SQL `prueba.sql` en tu servidor de SQL Server para crear la estructura de la base de datos.
   * Opcionalmente, ejecuta `seed_usuarios.sql` para insertar datos iniciales de prueba.
   * Asegúrate de actualizar la cadena de conexión (`connectionString`) en el archivo `Web.config` para que apunte a tu instancia local de SQL Server.
5. **Ejecutar la aplicación:**
   Presiona `F5` o haz clic en "Iniciar" en Visual Studio. Esto lanzará IIS Express y abrirá la aplicación en tu navegador web predeterminado.

## Autores
- Catherine Gomez
- Fabian Cares

Desarrollado para la Entrega 3 del Tercer Semestre (IP Santo Tomás).
