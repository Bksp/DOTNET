using System.Linq;
using System.Web.Mvc;
using Entrega3.Data;
using Entrega3.Models;

namespace Entrega3.Controllers
{
    public class UsuariosController : Controller
    {
        private TechHelpDbContext db = new TechHelpDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            // Only Administrator (RolId == 1) can access this
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarios = db.GetUsuarios();
            return View(usuarios);
        }
        [HttpGet]
        public ActionResult Crear()
        {
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
                return RedirectToAction("Login", "Auth");

            ViewBag.Roles = new SelectList(db.GetRoles(), "Id", "NombreRol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(UsuarioCreateViewModel model)
        {
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var existingUsers = db.GetUsuarios();
                if (existingUsers.Any(u => u.NombreUsuario.Equals(model.NombreUsuario, System.StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya está registrado.");
                }
                if (existingUsers.Any(u => u.Correo.Equals(model.Correo, System.StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Correo", "Este correo electrónico ya está en uso.");
                }

                if (ModelState.IsValid)
                {
                    var u = new Entrega3.Models.Usuario
                    {
                        NombreCompleto = model.NombreCompleto,
                        NombreUsuario = model.NombreUsuario,
                        Correo = model.Correo,
                        ClaveHash = model.NombreUsuario + "123",
                        RolId = model.RolId
                    };
                    db.InsertUsuario(u);
                    TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Roles = new SelectList(db.GetRoles(), "Id", "NombreRol", model.RolId);
            return View(model);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
                return RedirectToAction("Login", "Auth");

            var u = db.GetUsuarioById(id);
            if (u == null) return HttpNotFound();

            var model = new UsuarioEditViewModel
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                RolId = u.RolId
            };
            ViewBag.Roles = new SelectList(db.GetRoles(), "Id", "NombreRol", model.RolId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioEditViewModel model)
        {
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var u = new Entrega3.Models.Usuario
                {
                    Id = model.Id,
                    NombreCompleto = model.NombreCompleto,
                    Correo = model.Correo,
                    RolId = model.RolId
                };
                db.UpdateUsuarioAdmin(u);
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction("Index");
            }
            ViewBag.Roles = new SelectList(db.GetRoles(), "Id", "NombreRol", model.RolId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleStatus(int id)
        {
            if (Session["RolId"] == null || (int)Session["RolId"] != 1)
                return RedirectToAction("Login", "Auth");

            db.ToggleUserStatus(id);
            TempData["SuccessMessage"] = "Estado del usuario modificado exitosamente.";
            return RedirectToAction("Index");
        }
    }
}
