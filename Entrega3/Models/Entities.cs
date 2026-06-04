using System;
using System.Collections.Generic;

namespace Entrega3.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Navigation properties
        public string NombreRol { get; set; } // Opcional, para facilitar la vista
    }

    public class AreaSolicitante
    {
        public int Id { get; set; }
        public string NombreArea { get; set; }
    }

    public class TipoProblema
    {
        public int Id { get; set; }
        public string NombreTipoProblema { get; set; }
    }

    public class Prioridad
    {
        public int Id { get; set; }
        public string NombrePrioridad { get; set; }
    }

    public class EstadoSolicitud
    {
        public int Id { get; set; }
        public string NombreEstado { get; set; }
    }

    public class SolicitudSoporte
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AreaSolicitanteId { get; set; }
        public int TipoProblemaId { get; set; }
        public int PrioridadId { get; set; }
        public int EstadoSolicitudId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Navigation properties (for viewing in tables)
        public string SolicitanteNombre { get; set; }
        public string SolicitanteCorreo { get; set; }
        public string AreaNombre { get; set; }
        public string TipoProblemaNombre { get; set; }
        public string PrioridadNombre { get; set; }
        public string EstadoNombre { get; set; }
    }
}
