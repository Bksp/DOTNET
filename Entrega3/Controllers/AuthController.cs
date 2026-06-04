using System.Web.Mvc;
using Entrega3.Models;
using Entrega3.Data;

namespace Entrega3.Controllers
{
    public class AuthController : Controller
    {
        private TechHelpDbContext db = new TechHelpDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            // If already logged in, redirect based on role
            if (Session["UsuarioId"] != null)
            {
                return RedirectBasedOnRole();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.GetUsuarioByCredenciales(model.Usuario, model.Password);

                if (usuario != null)
                {
                    Session["UsuarioId"] = usuario.Id;
                    Session["NombreCompleto"] = usuario.NombreCompleto;
                    Session["RolId"] = usuario.RolId;
                    Session["NombreRol"] = usuario.NombreRol;
                    Session["Correo"] = usuario.Correo;

                    return RedirectBasedOnRole();
                }

                ModelState.AddModelError("", "Credenciales inválidas. Intente nuevamente.");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        private ActionResult RedirectBasedOnRole()
        {
            int rolId = (int)Session["RolId"];
            if (rolId == 1) // Administrador
            {
                return RedirectToAction("Index", "Usuarios");
            }
            else if (rolId == 2) // Soporte
            {
                return RedirectToAction("DashboardSoporte", "Solicitudes");
            }
            else // Usuario Normal
            {
                return RedirectToAction("MisSolicitudes", "Solicitudes");
            }
        }
    }
}
