using System;
using System.ComponentModel.DataAnnotations;

namespace Entrega3.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o correo es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class PerfilViewModel
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; }

        public string Correo { get; set; } // Readonly

        public string NombreRol { get; set; } // Readonly
    }

    public class CambiarPasswordViewModel
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria")]
        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string PasswordNueva { get; set; }

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña")]
        [Compare("PasswordNueva", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string PasswordConfirmacion { get; set; }
    }

    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }


        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }
    }

    public class UsuarioEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }
    }

    // DTOs for the RESTful API
    public class SolicitudCreateDTO
    {
        [Required(ErrorMessage = "El área solicitante es obligatoria")]
        public int AreaSolicitanteId { get; set; }

        [Required(ErrorMessage = "El tipo de problema es obligatorio")]
        public int TipoProblemaId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        public string Descripcion { get; set; }

        public int PrioridadId { get; set; }

        [Required(ErrorMessage = "La fecha de solicitud es obligatoria")]
        public DateTime FechaSolicitud { get; set; }
    }
}
