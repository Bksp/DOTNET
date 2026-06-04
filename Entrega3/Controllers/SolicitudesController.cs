using System.Web.Mvc;

namespace Entrega3.Controllers
{
    public class SolicitudesController : Controller
    {
        [HttpGet]
        public ActionResult MisSolicitudes()
        {
            // Only 'Usuario' (RolId == 3) can access this
            if (Session["RolId"] == null || (int)Session["RolId"] != 3)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpGet]
        public ActionResult DashboardSoporte()
        {
            // Only 'Soporte' (RolId == 2) can access this
            if (Session["RolId"] == null || (int)Session["RolId"] != 2)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}
