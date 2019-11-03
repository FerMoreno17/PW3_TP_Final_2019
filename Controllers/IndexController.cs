using PracticaEF.Data;
using PracticaEF.Servicios;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace TP_Final_2019_v._0.Controllers
{
    public class IndexController : Controller
    {
        private readonly Entities ctx = new Entities();
        PropuestaServicio prop = new PropuestaServicio();

        // GET: Index
        public ActionResult Inicio()
        {
            ViewBag.Titulo = "Bienvenidos";
            ViewBag.EstiloPagina = "";
            var Listado = (from propuesta in ctx.Propuestas
                                    join usuario in ctx.Usuarios
                                    on propuesta.IdUsuarioCreador equals usuario.IdUsuario
                                    where propuesta.Estado == 1
                                    select propuesta
                          ).ToList();
            return View(Listado);
        }
    }
}