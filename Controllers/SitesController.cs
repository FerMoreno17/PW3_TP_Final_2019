using PracticaEF.Data;
using PracticaEF.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Net.Mail;

namespace TP_Final_2019_v._0.Controllers
{
    public class SitesController : Controller
    {
        readonly UsuarioServicio user = new UsuarioServicio();
        readonly PropuestaServicio prop = new PropuestaServicio();
        readonly AdministradorServicio admin = new AdministradorServicio();
        readonly Entities ctx = new Entities();

        // GET: AboutUs
        public ActionResult Nosotros()
        {
            ViewBag.EstiloPagina = "single-page about-page";
            return View();
        }

        public ActionResult Propuestas()
        {
            List<Propuestas> lista = prop.getListaPropuestas();
            ViewBag.EstiloPagina = "single-page causes-page";
            return View(lista);
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
                    if(usuario_encontrado.Activo == true)
                    {
                        Session["session"] = usuario_encontrado;
                        return Redirect("/Index/Inicio");
                    }
                    else
                    {
                        ViewBag.MotivoError = "No te encuentras activo aún, revisa tu correo y sigue el enlace que te hemos enviado. Gracias.";
                        return View("../Shared/Error");
                    } 
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

                        Usuarios encontrado = admin.BuscarPorEmail(us.Email);
                        admin.EnviarMail(encontrado);


                        ViewBag.Mensaje = "Usuario registrado exitosamente. Revisa tu correo para activar tu cuenta.";
                        return View("Confirmaciones");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ActivarCuenta(String token)
        {
            admin.ActivarCuenta(token: token);
            return View("Login");
        }

        [HttpGet]
        public ActionResult Confirmaciones()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PropuestaDetalle(int id)
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var p = (from propuesta in ctx.Propuestas
                               join usuario in ctx.Usuarios
                               on propuesta.IdUsuarioCreador equals usuario.IdUsuario
                               where propuesta.IdPropuesta == id
                               select propuesta
                          ).SingleOrDefault();
                ViewBag.EstiloPagina = "single-page causes-page";
                return View(p);
            }
        }

        [HttpPost]
        public ActionResult ValorarPropuesta(FormCollection form)
        {
            int id = Convert.ToInt32(form["IdPropuesta"]);
            prop.ValorarPropuesta(form);
            return RedirectToAction("/PropuestaDetalle/"+id);
        }

        [HttpGet]
        public ActionResult GenerarDonacion(int id)
        {
            var p = prop.getPropuesta(id);
            if (p.TipoDonacion == 1)
            {
                var pd = prop.getPropuestaDonacion(p.TipoDonacion, p.IdPropuesta);
                var d = prop.getTotalDonacion(pd, p.TipoDonacion);
                ViewBag.Propuesta = p;
                ViewBag.PropuestaDonacion = pd;
                ViewBag.SumaDonacion = d;
            }
            if (p.TipoDonacion == 2) 
            { 
            var pd = prop.getPropuestaDonacion(p.TipoDonacion,p.IdPropuesta);
            var d = prop.getTotalDonacion(pd,p.TipoDonacion);
            ViewBag.Propuesta = p;
            ViewBag.PropuestaDonacion = pd;
            ViewBag.SumaDonacion = d;
            }
            if (p.TipoDonacion == 3)
            {
                var pd = prop.getPropuestaDonacion(p.TipoDonacion, p.IdPropuesta);
                var d = prop.getTotalDonacion(pd, p.TipoDonacion);
                ViewBag.Propuesta = p;
                ViewBag.PropuestaDonacion = pd;
                ViewBag.SumaDonacion = d;
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerarDonacion(FormCollection form, HttpPostedFileBase file)
        {
            int k = Convert.ToInt32(form["TipoDonacion"]);
            if (ModelState.IsValid)
            {
                int resp = prop.CargarDonacion(k, form, file);
                if(resp == 1)
                {
                    ViewBag.Mensaje = "Donacion recibida, muchas gracias.";
                    return View("Confirmaciones");
                }
            }
            ViewBag.MotivoError = "Hemos tenido dificultades para registrar su donación.";
            return View("../Shared/Error");
        }

        [HttpGet]
        public ActionResult GenerarDenuncia(int id)
        {
            ViewBag.IdPropuesta = id;
            ViewBag.Motivos = prop.GetMotivos();
            return View();
        }

        [HttpPost]
        public ActionResult GenerarDenuncia(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                prop.CargarDenuncia(form);
                ViewBag.Mensaje = "Denuncia cargada, la misma será motivo de revisión. Gracias por tu colaboración.";
                return View("Confirmaciones");
            }
            return View("/Shared/Error");
        }

       [HttpPost]
        public ActionResult BuscarPropuesta(FormCollection form)
        {
            if(form["Buscar"] == null || form["Buscar"] == "")
            {
                return RedirectToAction("Propuestas");
            }
            else
            {
                string buscar = form["Buscar"];
                List<Propuestas> lista = prop.BuscarPropuestas(buscar);
                ViewBag.EstiloPagina = "single-page causes-page";
                return View("Propuestas", lista);
            }
            
        }
    }
}