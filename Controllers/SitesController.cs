using PracticaEF.Data;
using PracticaEF.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP_Final_2019_v._0.Controllers
{
    public class SitesController : Controller
    {
        UsuarioServicio user = new UsuarioServicio();
        Entities ctx = new Entities();

        // GET: AboutUs
        public ActionResult Nosotros()
        {
            ViewBag.EstiloPagina = "single-page about-page";
            return View();
        }

        public ActionResult Causas()
        {
            ViewBag.EstiloPagina = "single-page causes-page";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Estilo = "...";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                var resp = user.Login(u);
                if(resp == 1)   
                {
                    var usuario_encontrado = user.getUsuario(u);
                    Session["session"] = usuario_encontrado;
                    return Redirect("/Admin/Index");
                }
                else
                {
                    ViewBag.Error = "usuario o contraseña no validos";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Estilo = "...";
            return View();
        }

        [HttpPost]
        public ActionResult Register(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                user.Agregar(u);
            }
            return View();
        }
    }
}