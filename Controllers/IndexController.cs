using PracticaEF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP_Final_2019_v._0.Controllers
{
    public class IndexController : Controller
    {
        private readonly Entities ctx = new Entities();

        // GET: Index
        public ActionResult Inicio()
        {
            ViewBag.Titulo = "Bienvenidos";
            ViewBag.EstiloPagina = "";
            List<Propuestas> Listado = (from prop in ctx.Propuestas select prop).ToList();
            return View(Listado);
        }
    }
}