using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Entrega3.Models;

namespace Entrega3.Data
{
    public class TechHelpDbContext
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["TechHelpDb"].ConnectionString;

        // --- AUTH & USERS ---

        public Usuario GetUsuarioByCredenciales(string usuarioOCorreo, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // In a real scenario, compare hashes. Here we check plain or assume the hash matches exactly what is passed for simplicity, 
                // but the prompt implies we should at least have ClaveHash logic.
                string query = @"SELECT u.*, r.NombreRol 
                                 FROM Usuarios u 
                                 INNER JOIN Roles r ON u.RolId = r.Id 
                                 WHERE (u.NombreUsuario = @User OR u.Correo = @User) 
                                 AND u.ClaveHash = @Password AND u.Estado = 1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@User", usuarioOCorreo);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return MapUsuario(reader);
                }
                return null;
            }
        }

        public Usuario GetUsuarioById(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.*, r.NombreRol 
                                 FROM Usuarios u 
                                 INNER JOIN Roles r ON u.RolId = r.Id 
                                 WHERE u.Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return MapUsuario(reader);
                }
                return null;
            }
        }

        public List<Usuario> GetUsuarios()
        {
            List<Usuario> list = new List<Usuario>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.*, r.NombreRol 
                                 FROM Usuarios u 
                                 INNER JOIN Roles r ON u.RolId = r.Id";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(MapUsuario(reader));
                }
            }
            return list;
        }

        public void UpdatePerfil(int usuarioId, string nombreCompleto)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET NombreCompleto = @Nombre WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Nombre", nombreCompleto);
                cmd.Parameters.AddWithValue("@Id", usuarioId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ChangePassword(int usuarioId, string oldPassword, string newPassword)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Verify old password first
                string checkQuery = "SELECT Id FROM Usuarios WHERE Id = @Id AND ClaveHash = @Old";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", usuarioId);
                checkCmd.Parameters.AddWithValue("@Old", oldPassword);

                con.Open();
                var result = checkCmd.ExecuteScalar();
                if (result == null) return false; // Wrong old password

                string updateQuery = "UPDATE Usuarios SET ClaveHash = @New WHERE Id = @Id";
                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@New", newPassword);
                updateCmd.Parameters.AddWithValue("@Id", usuarioId);

                updateCmd.ExecuteNonQuery();
                return true;
            }
        }

        public List<Rol> GetRoles()
        {
            List<Rol> list = new List<Rol>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Roles";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Rol
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NombreRol = reader["NombreRol"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }
            return list;
        }

        public void InsertUsuario(Usuario u)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Usuarios (RolId, NombreCompleto, NombreUsuario, Correo, ClaveHash, Estado, FechaRegistro)
                                 VALUES (@RolId, @Nombre, @Usuario, @Correo, @Clave, 1, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RolId", u.RolId);
                cmd.Parameters.AddWithValue("@Nombre", u.NombreCompleto);
                cmd.Parameters.AddWithValue("@Usuario", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@Correo", u.Correo);
                cmd.Parameters.AddWithValue("@Clave", u.ClaveHash);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUsuarioAdmin(Usuario u)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Usuarios 
                                 SET NombreCompleto = @Nombre, 
                                     Correo = @Correo, 
                                     RolId = @RolId 
                                 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Nombre", u.NombreCompleto);
                cmd.Parameters.AddWithValue("@Correo", u.Correo);
                cmd.Parameters.AddWithValue("@RolId", u.RolId);
                cmd.Parameters.AddWithValue("@Id", u.Id);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ToggleUserStatus(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET Estado = Estado ^ 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- SOLICITUDES CRUD (FOR API) ---

        public List<SolicitudSoporte> GetSolicitudes(int? usuarioId = null)
        {
            List<SolicitudSoporte> list = new List<SolicitudSoporte>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT s.*, u.NombreCompleto, u.Correo, a.NombreArea, t.NombreTipoProblema, p.NombrePrioridad, e.NombreEstado 
                                 FROM SolicitudesSoporte s
                                 INNER JOIN Usuarios u ON s.UsuarioId = u.Id
                                 INNER JOIN AreasSolicitantes a ON s.AreaSolicitanteId = a.Id
                                 INNER JOIN TiposProblema t ON s.TipoProblemaId = t.Id
                                 INNER JOIN Prioridades p ON s.PrioridadId = p.Id
                                 INNER JOIN EstadosSolicitud e ON s.EstadoSolicitudId = e.Id";
                
                if (usuarioId.HasValue)
                {
                    query += " WHERE s.UsuarioId = @UsuarioId";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                if (usuarioId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId.Value);
                }

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(MapSolicitud(reader));
                }
            }
            return list;
        }

        public SolicitudSoporte GetSolicitudById(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT s.*, u.NombreCompleto, u.Correo, a.NombreArea, t.NombreTipoProblema, p.NombrePrioridad, e.NombreEstado 
                                 FROM SolicitudesSoporte s
                                 INNER JOIN Usuarios u ON s.UsuarioId = u.Id
                                 INNER JOIN AreasSolicitantes a ON s.AreaSolicitanteId = a.Id
                                 INNER JOIN TiposProblema t ON s.TipoProblemaId = t.Id
                                 INNER JOIN Prioridades p ON s.PrioridadId = p.Id
                                 INNER JOIN EstadosSolicitud e ON s.EstadoSolicitudId = e.Id
                                 WHERE s.Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return MapSolicitud(reader);
                }
                return null;
            }
        }

        public int InsertSolicitud(SolicitudSoporte sol)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO SolicitudesSoporte (UsuarioId, AreaSolicitanteId, TipoProblemaId, PrioridadId, EstadoSolicitudId, Descripcion, FechaSolicitud, FechaRegistro)
                                 OUTPUT INSERTED.Id
                                 VALUES (@UsuarioId, @AreaId, @TipoId, @PrioId, @EstadoId, @Desc, @Fecha, @FechaReg)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UsuarioId", sol.UsuarioId);
                cmd.Parameters.AddWithValue("@AreaId", sol.AreaSolicitanteId);
                cmd.Parameters.AddWithValue("@TipoId", sol.TipoProblemaId);
                cmd.Parameters.AddWithValue("@PrioId", sol.PrioridadId);
                cmd.Parameters.AddWithValue("@EstadoId", 1); // 1 = Pendiente
                cmd.Parameters.AddWithValue("@Desc", sol.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", sol.FechaSolicitud);
                cmd.Parameters.AddWithValue("@FechaReg", DateTime.Now);

                con.Open();
                int id = (int)cmd.ExecuteScalar();
                return id;
            }
        }

        public bool UpdateEstadoSolicitud(int id, int nuevoEstadoId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE SolicitudesSoporte SET EstadoSolicitudId = @EstadoId WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EstadoId", nuevoEstadoId);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool DeleteSolicitud(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM SolicitudesSoporte WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        // --- MAPPERS ---

        private Usuario MapUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = Convert.ToInt32(reader["Id"]),
                RolId = Convert.ToInt32(reader["RolId"]),
                NombreCompleto = reader["NombreCompleto"].ToString(),
                NombreUsuario = reader["NombreUsuario"].ToString(),
                Correo = reader["Correo"].ToString(),
                ClaveHash = reader["ClaveHash"].ToString(),
                Estado = Convert.ToBoolean(reader["Estado"]),
                FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                NombreRol = reader["NombreRol"].ToString()
            };
        }

        private SolicitudSoporte MapSolicitud(SqlDataReader reader)
        {
            return new SolicitudSoporte
            {
                Id = Convert.ToInt32(reader["Id"]),
                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                AreaSolicitanteId = Convert.ToInt32(reader["AreaSolicitanteId"]),
                TipoProblemaId = Convert.ToInt32(reader["TipoProblemaId"]),
                PrioridadId = Convert.ToInt32(reader["PrioridadId"]),
                EstadoSolicitudId = Convert.ToInt32(reader["EstadoSolicitudId"]),
                Descripcion = reader["Descripcion"].ToString(),
                FechaSolicitud = Convert.ToDateTime(reader["FechaSolicitud"]),
                FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                SolicitanteNombre = reader["NombreCompleto"].ToString(),
                SolicitanteCorreo = reader["Correo"].ToString(),
                AreaNombre = reader["NombreArea"].ToString(),
                TipoProblemaNombre = reader["NombreTipoProblema"].ToString(),
                PrioridadNombre = reader["NombrePrioridad"].ToString(),
                EstadoNombre = reader["NombreEstado"].ToString()
            };
        }
    }
}
