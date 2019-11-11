using PracticaEF.Data;
using PracticaEF.Servicios;
using System.Web.Mvc;
using System.Linq;
using Models;
using System.IO;
using System.Web;
using System;
using System.Collections.Generic;

namespace TP_Final_2019_v._0.Controllers
{
    public class AdminController : Controller
    {
        AdministradorServicio admin = new AdministradorServicio();
        PropuestaServicio prop = new PropuestaServicio();

        private readonly Entities ctx = new Entities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();

            }
        }
        /*-------------------Perfil---------------------------*/
        [HttpGet]
        public ActionResult Perfil(int id)
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                ViewBag.UsuarioEncontrado = Session["session"];
                return View();
            }
        }
        [HttpPost]
        public ActionResult Perfil(ModificarUsuario recibido, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                Usuarios us = (Usuarios)Session["session"];
                Usuarios aCambiar = ctx.Usuarios.Find(us.IdUsuario);

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/users"), fileName);
                    file.SaveAs(path);

                    aCambiar.Foto = fileName;
                    aCambiar.Nombre = recibido.Nombre;
                    aCambiar.Apellido = recibido.Apellido;
                    if (aCambiar.UserName == null)
                    {
                        string nameUser = recibido.Nombre +"."+recibido.Apellido;
                        int contar = (from u in ctx.Usuarios
                                      where u.UserName.Contains(nameUser)
                                      select u.UserName).Count();
                        if (contar == 0)
                        {
                            aCambiar.UserName = nameUser;
                        }
                        else
                        {
                            aCambiar.UserName = nameUser + "." + contar;
                        }
                    }

                    ctx.SaveChanges();
                    Session["session"] = ctx.Usuarios.Find(us.IdUsuario);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MotivoError = "No se hemos podido modficar tu perfil.";
                    return View("Error");
                }
            }
            ViewBag.MotivoError = "El formato de modelo no es válido.";
            return View("Error");
        }
        /*-------------------Crear Propuesta---------------------------*/
        [HttpGet]
        public ActionResult CrearPropuesta()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                Usuarios u = (Usuarios)Session["session"];
                int cantidadPropuestas = admin.ContarPropuestasActivas(u.IdUsuario);
                if (cantidadPropuestas < 3)
                {
                    ViewBag.UsuarioEncontrado = u;
                    return View();
                }
                else
                {
                    Uri url = Request.UrlReferrer;
                    ViewBag.Volver = url;
                    ViewBag.MotivoError = "Has llegado al límite de 3 propuestas activas. No puedes crear una nueva.";
                    return View("Error");
                }
            }
        }
        [HttpPost]
        public ActionResult CrearPropuesta(CrearPropuesta cp, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Usuarios u = (Usuarios)Session["session"];
                int resp = admin.CrearPropuestax(cp, file, u);

                if (resp != 0)
                {
                    if (resp == 1)
                    {
                        return View("TipoDonacionMonetaria");
                    }
                    if (resp == 2)
                    {
                        return View("TipoDonacionInsumos");
                    }
                    if (resp == 3)
                    {
                        return View("TipoDonacionHorasTrabajo");
                    }
                }
                ViewBag.MotivoError = "No hemos podido procesar la solicitud. Intentalo más tarde.";
                return View("Error");
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Intentalo más tarde.";
            return View("Error");
        }
        [HttpGet]
        public ActionResult TipoDonacionMonetaria()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult TipoDonacionMonetaria(TipoDonacionMonetaria td)
        {
            if (ModelState.IsValid)
            {
                Usuarios u = (Usuarios)Session["session"];
                int resp = admin.CargarPropuestaMonetaria(td, u);
                ViewBag.IdPropuesta = resp;
                return View("Referencias");
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Modelo no válido";
            return View("Error");
        }
        [HttpGet]
        public ActionResult TipoDonacionInsumos()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();

            }
        }
        [HttpPost]
        public ActionResult TipoDonacionInsumos(TipoDonacionInsumos td)
        {
            if (ModelState.IsValid)
            {
                Usuarios u = (Usuarios)Session["session"];
                int resp = admin.CargarPropuestaInsumo(td, u);
                ViewBag.IdPropuesta = resp;
                return View("Referencias");
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Modelo no válido";
            return View("Error");

        }
        [HttpGet]
        public ActionResult TipoDonacionHorasTrabajo()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult TipoDonacionHorasTrabajo(TipoDonacionHorasTrabajo td)
        {
            if (ModelState.IsValid)
            {
                Usuarios u = (Usuarios)Session["session"];
                int resp = admin.CargarPropuestaHorasTrabajo(td, u);
                ViewBag.IdPropuesta = resp;
                return View("Referencias");
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Modelo no válido";
            return View("Error");
        }
        [HttpGet]
        public ActionResult Referencias()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Referencias(Referencia r)
        {
            if (ModelState.IsValid)
            {
                admin.CargarReferencias(r);
            }
            ViewBag.Mensaje = "Propuesta cargada exitosamente";
            return View("Finished");
        }

        /*-------------------Ver Propuesta y donaciones---------------------------*/
        public ActionResult Propuestas()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                Usuarios u = (Usuarios)Session["session"];
                List<Propuestas> lista = admin.GetMisPropuestas(u.IdUsuario);
                ViewBag.EstiloPagina = "single-page causes-page";
                return View(lista);
            }
        }
        [HttpGet]
        public ActionResult VerDonacionesPropuesta(int id)
        {
            var p = admin.GetPropuesta(id);
            ViewBag.Propuesta = p;
            if (p.TipoDonacion == 1)
            {
                var tp = (PropuestasDonacionesMonetarias)admin.GetTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesMonetarias>)admin.GetDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionMonetaria);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaMonetaria", don);
            }
            if (p.TipoDonacion == 2)
            {
                var tp = (PropuestasDonacionesInsumos)admin.GetTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesInsumos>)admin.GetDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionInsumo);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaInsumos", don);
            }
            if (p.TipoDonacion == 3)
            {
                var tp = (PropuestasDonacionesHorasTrabajo)admin.GetTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesHorasTrabajo>)admin.GetDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionHorasTrabajo);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaHorasTrabajo", don);
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Ocurrio un problema al visualizar las donaciones recibidas";
            return View("Error");
        }

        /*-------------------Modificar propuesta---------------------------*/
        //devuelve a la vista la propuesta que se va a modificar
        [HttpGet]
        public ActionResult ModificarPropuesta(int id)
        {
            var p = admin.GetPropuesta(id);
            if (p.TipoDonacion == 1)
            {
               ModificarPropuesta mp = admin.GetModificarPropuestaMonetaria(p,id);
                return View(mp);
            }
            if (p.TipoDonacion == 2)
            {
                ModificarPropuesta mp = admin.GetModificarPropuestaInsumo(p, id);
                return View(mp);
            }
            if (p.TipoDonacion == 3)
            {
                ModificarPropuesta mp = admin.GetModificarPropuestaHorasTrabajo(p, id);
                return View(mp);
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Modelo no válido";
            return View("Error");
        }
        [HttpPost]
        public ActionResult ModificarPropuesta(ModificarPropuesta mp, HttpPostedFileBase file)
        {
            admin.ModificarPropuesta(mp, file);
            return RedirectToAction("ModificarPropuesta/" +mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaMonetaria(ModificarPropuesta mp)
        {
            admin.ModificarPropuestaCasoIndividual(mp);
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaInsumos(ModificarPropuesta mp)
        {
            admin.ModificarPropuestaCasoIndividual(mp);
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaHorasTrabajo(ModificarPropuesta mp)
        {
            admin.ModificarPropuestaCasoIndividual(mp);
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaReferencias(ModificarPropuesta mp)
        {
            admin.ModificarReferencias(mp);
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }

        /*-------------------Denuncias---------------------------*/
        [HttpGet]
        public ActionResult Denuncias()
        {
            List<Denuncias> lista = admin.GetDenuncias();
            return View(lista);
        }
        [HttpGet]
        public ActionResult AceptarDenuncia(int id)
        {
            admin.AceptarDenuncia(id);
            return RedirectToAction("Denuncias");
        }
        [HttpGet]
        public ActionResult IgnorarDenuncia(int id)
        {
            admin.IgnorarDenuncia(id);
            return RedirectToAction("Denuncias");
        }

        /*-------------------Varias---------------------------*/

        public ActionResult Finished()
        {
            if (Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
                return View();
            }
        }

        public ActionResult SessionFinish()
        {
            Session["session"] = null;
            return RedirectToAction("../Index/Inicio");
        }

        [HttpGet]
        public ActionResult MiHistorialDonaciones(int id)
        {
            ViewBag.Usuario = id;
            return View();
        }
    }
}