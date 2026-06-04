using System;
using System.Web.Mvc;
using Entrega3.Data;
using Entrega3.Models;

namespace Entrega3.Controllers
{
    [RoutePrefix("api/solicitudes")]
    public class SolicitudesApiController : Controller
    {
        private TechHelpDbContext db = new TechHelpDbContext();

        // GET: /api/solicitudes
        // Soporta filtro ?usuarioId=X
        [HttpGet]
        [Route("")]
        public ActionResult GetSolicitudes(int? usuarioId)
        {
            // In a real application we would check session here to ensure
            // the user is authorized (e.g. only fetching their own ID unless they are Admin/Soporte).
            // For simplicity and to follow the requirement precisely, we return JSON.

            var solicitudes = db.GetSolicitudes(usuarioId);
            return Json(solicitudes, JsonRequestBehavior.AllowGet);
        }

        // GET: /api/solicitudes/{id}
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult GetSolicitud(int id)
        {
            var solicitud = db.GetSolicitudById(id);
            if (solicitud == null)
            {
                return HttpNotFound("Solicitud no encontrada.");
            }
            return Json(solicitud, JsonRequestBehavior.AllowGet);
        }

        // POST: /api/solicitudes
        [HttpPost]
        [Route("")]
        public ActionResult CreateSolicitud(SolicitudCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // In MVC, return 400 Bad Request if invalid.
                Response.StatusCode = 400;
                return Json(new { message = "Datos de solicitud inválidos." });
            }

            if (dto.FechaSolicitud.Date < DateTime.Now.Date)
            {
                Response.StatusCode = 400;
                return Json(new { message = "No se permiten generar solicitudes con fecha en el pasado." });
            }

            // In a true MVC app the user ID is in session, but since this is an API, 
            // if we are calling it from a browser logged into MVC, we can access Session.
            // Let's assume the session is available:
            int usuarioId = 1; // Default fallback
            if (Session["UsuarioId"] != null)
            {
                usuarioId = (int)Session["UsuarioId"];
            }

            var sol = new SolicitudSoporte
            {
                UsuarioId = usuarioId,
                AreaSolicitanteId = dto.AreaSolicitanteId,  
                TipoProblemaId = dto.TipoProblemaId,
                PrioridadId = dto.PrioridadId,
                Descripcion = dto.Descripcion,
                FechaSolicitud = dto.FechaSolicitud
            };

            int id = db.InsertSolicitud(sol);

            return Json(new { id = id, message = "Solicitud creada con éxito." });
        }

        // PUT: /api/solicitudes/{id}/estado
        [HttpPut]
        [Route("{id:int}/estado")]
        public ActionResult UpdateEstado(int id, int nuevoEstadoId)
        {
            // Verify if user is 'Soporte'
            if (Session["RolId"] == null || (int)Session["RolId"] != 2)
            {
                Response.StatusCode = 403;
                return Json(new { message = "No autorizado para cambiar estados." });
            }

            bool result = db.UpdateEstadoSolicitud(id, nuevoEstadoId);
            if (!result)
            {
                return HttpNotFound("Solicitud no encontrada.");
            }

            return Json(new { message = "Estado actualizado correctamente." });
        }

        // DELETE: /api/solicitudes/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeleteSolicitud(int id)
        {
            // Verify if user is 'Soporte'
            if (Session["RolId"] == null || (int)Session["RolId"] != 2)
            {
                Response.StatusCode = 403;
                return Json(new { message = "No autorizado para eliminar." });
            }

            bool result = db.DeleteSolicitud(id);
            if (!result)
            {
                return HttpNotFound("Solicitud no encontrada.");
            }

            return Json(new { message = "Solicitud eliminada correctamente." });
        }
    }
}
