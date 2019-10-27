using PracticaEF.Data;
using PracticaEF.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

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
        public ActionResult Login(Login us)
        {
            if (ModelState.IsValid)
            {
                Usuarios u = new Usuarios();
                u.Email = us.Email;
                u.Password = us.Password;
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
        public ActionResult Register(RegistrarUsuario us)
        {
            int res1 = user.ValidarEmail(us.Email);
            int res2 = user.ValidarEdad(us.FechaNacimiento);
            if(res1 == 1)
            {
                ViewBag.EmailRegistrado = "El e-mail ya existe, elige otro.";
            }
            else
            {
                if (res2 == 0)
                {
                    ViewBag.MenorDeEdad = "Eres menor de edad, no puedes registrarte.";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Usuarios u = new Usuarios();
                        u.Email = us.Email;
                        u.Password = us.Password;
                        u.FechaNacimiento = us.FechaNacimiento;
                        user.Agregar(u);
                        return RedirectToAction("RespuestaRegistroUsuario");
                    }
                }
            }
            return View();
        }

        public ActionResult RespuestaRegistroUsuario()
        {
            return View();
        }
    }
}