using System.Web.Mvc;
using Entrega3.Models;
using Entrega3.Data;

namespace Entrega3.Controllers
{
    public class PerfilController : Controller
    {
        private TechHelpDbContext db = new TechHelpDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UsuarioId"] == null)
                return RedirectToAction("Login", "Auth");

            int id = (int)Session["UsuarioId"];
            var usuario = db.GetUsuarioById(id);

            var model = new PerfilViewModel
            {
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                NombreRol = usuario.NombreRol
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(PerfilViewModel model)
        {
            if (Session["UsuarioId"] == null)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                int id = (int)Session["UsuarioId"];
                db.UpdatePerfil(id, model.NombreCompleto);
                
                // Update session
                Session["NombreCompleto"] = model.NombreCompleto;

                TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
                return RedirectToAction("Index");
            }

            // Restore readonly fields
            model.Correo = Session["Correo"].ToString();
            model.NombreRol = Session["NombreRol"].ToString();
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarPassword(CambiarPasswordViewModel model)
        {
            if (Session["UsuarioId"] == null)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                int id = (int)Session["UsuarioId"];
                bool success = db.ChangePassword(id, model.PasswordActual, model.PasswordNueva);

                if (success)
                {
                    TempData["SuccessMessage"] = "Contraseña cambiada exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("PasswordActual", "La contraseña actual es incorrecta.");
                }
            }

            // If we fail, we need to return to Index with Perfil data
            var modelPerfil = new PerfilViewModel
            {
                NombreCompleto = Session["NombreCompleto"].ToString(),
                Correo = Session["Correo"].ToString(),
                NombreRol = Session["NombreRol"].ToString()
            };
            return View("Index", modelPerfil);
        }
    }
}
