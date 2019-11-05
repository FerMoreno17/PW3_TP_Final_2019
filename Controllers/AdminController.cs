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
            //contar la cantidad de propuestas activas que tiene el usuario si son >=5 ya no puede crear una nueva
            if (ModelState.IsValid)
            {
                Propuestas p = new Propuestas();
                Usuarios u = (Usuarios)Session["session"];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/propuestas"), fileName);
                    file.SaveAs(path);

                    p.Nombre = cp.Nombre;
                    p.Descripcion = cp.Descripcion;
                    p.FechaFin = cp.FechaFin;
                    p.TelefonoContacto = cp.TelefonoContacto;
                    p.TipoDonacion = cp.TipoDonacion;
                    p.Foto = fileName;
                    p.IdUsuarioCreador = u.IdUsuario;
                    p.Valoracion = 0;
                    p.Estado = 1;
                    p.FechaCreacion = DateTime.Today;

                    ctx.Propuestas.Add(p);
                    ctx.SaveChanges();

                    if (cp.TipoDonacion == 1)
                    {
                        return View("TipoDonacionMonetaria");
                    }
                    if (cp.TipoDonacion == 2)
                    {
                        return View("TipoDonacionInsumos");
                    }
                    if (cp.TipoDonacion == 3)
                    {
                        return View("TipoDonacionHorasTrabajo");
                    }
                }
                return View();
            }
            return View();
        }
        /*[HttpPost]
        public ActionResult CrearPropuesta(CrearPropuesta cp, HttpPostedFileBase file)
        {
            //contar la cantidad de propuestas activas que tiene el usuario si son >=5 ya no puede crear una nueva
            if (ModelState.IsValid)
            {
                Propuestas p = new Propuestas();

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/propuestas"), fileName);
                    //file.SaveAs(path);

                    p.Nombre = cp.Nombre;
                    p.Descripcion = cp.Descripcion;
                    p.FechaFin = cp.FechaFin;
                    p.TelefonoContacto = cp.TelefonoContacto;
                    p.TipoDonacion = cp.TipoDonacion;
                    p.Foto = fileName;
                    p.IdUsuarioCreador = cp.IdUsuarioCreador;
                    p.Valoracion = 0;
                    p.Estado = 1;
                    p.FechaCreacion = DateTime.Today;

                    //ctx.Propuestas.Add(p);
                    //ctx.SaveChanges();

                    ViewBag.TipoDonacion = cp.TipoDonacion;
                    return View("TipoDonacion");
                }
                ViewBag.MotivoError = "No se hemos podido crear la propuesta";
                return View("../Shared/Error");
            }
            ViewBag.MotivoError = "El formato de modelo no es válido";
            return View("../Shared/Error");
        }
        [HttpGet]
        public ActionResult TipoDonacion()
        {
            return View();
        }

         [HttpPost]
        public string TipoDonacion(TipoDonacion td)
        {
               if(td.Td == 1)
                {
                    return "monetaria";
                }
                if (td.Td == 2)
                {
                    return "insumos";
                }
                if (td.Td == 3)
                {
                    return "horas trabajo";
                }
            return "error: " + td.Td + "string: " + td.ValorString + "valor: " + td.ValorInt;
        }*/
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
                var usua = (from prop in ctx.Propuestas
                            where prop.IdUsuarioCreador == u.IdUsuario
                            select prop.IdPropuesta).ToList().LastOrDefault();

                PropuestasDonacionesMonetarias pm = new PropuestasDonacionesMonetarias();
                pm.Dinero = td.Dinero;
                pm.CBU = td.CBU;
                pm.IdPropuesta = usua;
                ctx.PropuestasDonacionesMonetarias.Add(pm);
                ctx.SaveChanges();
                ViewBag.IdPropuesta = usua;
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
                var usua = (from prop in ctx.Propuestas
                            where prop.IdUsuarioCreador == u.IdUsuario
                            select prop.IdPropuesta).ToList().LastOrDefault();
                PropuestasDonacionesInsumos pi = new PropuestasDonacionesInsumos();
                pi.Nombre = td.NombreInsumo;
                pi.Cantidad = td.Cantidad;
                pi.IdPropuesta = usua;
                ctx.PropuestasDonacionesInsumos.Add(pi);
                ctx.SaveChanges();
                ViewBag.IdPropuesta = usua;
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
                var usua = (from prop in ctx.Propuestas
                            where prop.IdUsuarioCreador == u.IdUsuario
                            select prop.IdPropuesta).ToList().LastOrDefault();
                PropuestasDonacionesHorasTrabajo ph = new PropuestasDonacionesHorasTrabajo();
                ph.CantidadHoras = td.CantidadHoras;
                ph.Profesion = td.Profesion;
                ph.IdPropuesta = usua;
                ctx.PropuestasDonacionesHorasTrabajo.Add(ph);
                ctx.SaveChanges();
                ViewBag.IdPropuesta = usua;
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
                PropuestasReferencias ref1 = new PropuestasReferencias();
                PropuestasReferencias ref2 = new PropuestasReferencias();
                ref1.IdPropuesta = r.IdPropuesta;
                ref1.Nombre = r.Nombre1;
                ref1.Telefono = r.Telefono1;
                ctx.PropuestasReferencias.Add(ref1);
                ref2.IdPropuesta = r.IdPropuesta;
                ref2.Nombre = r.Nombre2;
                ref2.Telefono = r.Telefono2;
                ctx.PropuestasReferencias.Add(ref2);
                ctx.SaveChanges();
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
                List<Propuestas> lista = admin.getMisPropuestas(u.IdUsuario);
                ViewBag.EstiloPagina = "single-page causes-page";
                return View(lista);
            }
        }
        [HttpGet]
        public ActionResult VerDonacionesPropuesta(int id)
        {
            var p = admin.getPropuesta(id);
            ViewBag.Propuesta = p;
            if (p.TipoDonacion == 1)
            {
                var tp = (PropuestasDonacionesMonetarias)admin.getTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesMonetarias>)admin.getDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionMonetaria);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaMonetaria", don);
            }
            if (p.TipoDonacion == 2)
            {
                var tp = (PropuestasDonacionesInsumos)admin.getTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesInsumos>)admin.getDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionInsumo);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaInsumos", don);
            }
            if (p.TipoDonacion == 3)
            {
                var tp = (PropuestasDonacionesHorasTrabajo)admin.getTipoPropuesta(p.TipoDonacion, p.IdPropuesta);
                var don = (List<DonacionesHorasTrabajo>)admin.getDonacionesPropuesta(p.TipoDonacion, tp.IdPropuestaDonacionHorasTrabajo);
                ViewBag.Propuesta = p;
                ViewBag.TipoPropuesta = tp;
                return View("VerDonacionesPropuestaHorasTrabajo", don);
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Ocurrio un problema al visualizar las donaciones recibidas";
            return View("Error");
        }
        /*-------------------Modificar propuesta---------------------------*/
        [HttpGet]
        public ActionResult ModificarPropuesta(int id)
        {
            var p = prop.getPropuesta(id);
            if (p.TipoDonacion == 1)
            {
                var tp = (PropuestasDonacionesMonetarias)prop.getPropuestaDonacion(id, p.TipoDonacion);
                var r = prop.GetReferencias(p.IdPropuesta);
                ModificarPropuesta mp = new ModificarPropuesta();
                mp.IdPropuesta = tp.IdPropuesta;
                mp.Nombre = p.Nombre;
                mp.Descripcion = p.Descripcion;
                mp.FechaFin = p.FechaFin;
                mp.TelefonoContacto = p.TelefonoContacto;
                mp.TipoDonacion = p.TipoDonacion;
                mp.Foto = p.Foto;
                mp.IdPropuestaDonacionMonetaria = tp.IdPropuestaDonacionMonetaria;
                mp.Dinero = tp.Dinero;
                mp.CBU = tp.CBU;
                int i = 1;
                foreach (var x in r)
                {
                    if (i == 1)
                    {
                        mp.IdReferencia1 = x.IdReferencia;
                        mp.NombreRef1 = x.Nombre;
                        mp.TelefonoRef1 = x.Telefono;
                    }
                    if (i == 2)
                    {
                        mp.IdReferencia2 = x.IdReferencia;
                        mp.NombreRef2 = x.Nombre;
                        mp.TelefonoRef2 = x.Telefono;
                    }
                    i++;
                }
                return View(mp);
            }
            if (p.TipoDonacion == 2)
            {
                var tp = (PropuestasDonacionesInsumos)prop.getPropuestaDonacion(id, p.TipoDonacion);
                var r = prop.GetReferencias(p.IdPropuesta);
                ModificarPropuesta mp = new ModificarPropuesta();
                mp.IdPropuesta = tp.IdPropuesta;
                mp.Nombre = p.Nombre;
                mp.Descripcion = p.Descripcion;
                mp.FechaFin = p.FechaFin;
                mp.TelefonoContacto = p.TelefonoContacto;
                mp.TipoDonacion = p.TipoDonacion;
                mp.Foto = p.Foto;
                mp.IdPropuestaDonacionInsumo = tp.IdPropuestaDonacionInsumo;
                mp.NombreItem = tp.Nombre;
                mp.Cantidad = tp.Cantidad;
                int i = 1;
                foreach (var x in r)
                {
                    if (i == 1)
                    {
                        mp.IdReferencia1 = x.IdReferencia;
                        mp.NombreRef1 = x.Nombre;
                        mp.TelefonoRef1 = x.Telefono;
                    }
                    if (i == 2)
                    {
                        mp.IdReferencia2 = x.IdReferencia;
                        mp.NombreRef2 = x.Nombre;
                        mp.TelefonoRef2 = x.Telefono;
                    }
                    i++;
                }
                return View(mp);
            }
            if (p.TipoDonacion == 3)
            {
                var tp = (PropuestasDonacionesHorasTrabajo)prop.getPropuestaDonacion(id, p.TipoDonacion);
                var r = prop.GetReferencias(p.IdPropuesta);
                ModificarPropuesta mp = new ModificarPropuesta();
                mp.IdPropuesta = tp.IdPropuesta;
                mp.Nombre = p.Nombre;
                mp.Descripcion = p.Descripcion;
                mp.FechaFin = p.FechaFin;
                mp.TelefonoContacto = p.TelefonoContacto;
                mp.TipoDonacion = p.TipoDonacion;
                mp.Foto = p.Foto;
                mp.IdPropuestaDonacionHorasTrabajo = tp.IdPropuestaDonacionHorasTrabajo;
                mp.CantidadHoras = tp.CantidadHoras;
                mp.Profesion = tp.Profesion;
                int i = 1;
                foreach (var x in r)
                {
                    if (i == 1)
                    {
                        mp.IdReferencia1 = x.IdReferencia;
                        mp.NombreRef1 = x.Nombre;
                        mp.TelefonoRef1 = x.Telefono;
                    }
                    if (i == 2)
                    {
                        mp.IdReferencia2 = x.IdReferencia;
                        mp.NombreRef2 = x.Nombre;
                        mp.TelefonoRef2 = x.Telefono;
                    }
                    i++;
                }
                return View(mp);
            }
            ViewBag.MotivoError = "No hemos podido procesar la solicitud. Modelo no válido";
            return View("Error");
        }
        [HttpPost]
        public ActionResult ModificarPropuesta(ModificarPropuesta mp, HttpPostedFileBase file)
        {
            Propuestas p = ctx.Propuestas.Find(mp.IdPropuesta);

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/propuestas"), fileName);
                file.SaveAs(path);
                p.Foto = fileName;
            }
            p.Nombre = mp.Nombre;
            p.Descripcion = mp.Descripcion;
            if (mp.FechaFin.ToString() != "1/1/0001 00:00:00")
            {
                p.FechaFin = mp.FechaFin;
            }
            p.TelefonoContacto = mp.TelefonoContacto;
            ctx.SaveChanges();

            return RedirectToAction("ModificarPropuesta");
        }
        [HttpPost]
        public ActionResult ModificarPropuestaMonetaria(ModificarPropuesta mp)
        {
            PropuestasDonacionesMonetarias p = ctx.PropuestasDonacionesMonetarias.Find(mp.IdPropuestaDonacionMonetaria);
            p.Dinero = mp.Dinero;
            p.CBU = mp.CBU;
            ctx.SaveChanges();
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaInsumos(ModificarPropuesta mp)
        {
            PropuestasDonacionesInsumos m = ctx.PropuestasDonacionesInsumos.Find(mp.IdPropuestaDonacionInsumo);
            m.Nombre = mp.NombreItem;
            m.Cantidad = mp.Cantidad;
            ctx.SaveChanges();
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaHorasTrabajo(ModificarPropuesta mp)
        {
            PropuestasDonacionesHorasTrabajo p = ctx.PropuestasDonacionesHorasTrabajo.Find(mp.IdPropuestaDonacionHorasTrabajo);
            p.CantidadHoras = mp.CantidadHoras;
            if (mp.Profesion != "")
            {
                p.Profesion = mp.Profesion;
            }
            ctx.SaveChanges();
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }
        [HttpPost]
        public ActionResult ModificarPropuestaReferencias(ModificarPropuesta mp)
        {
            int j = 1;
            List<PropuestasReferencias> p = prop.GetReferencias(mp.IdPropuesta);
            foreach (var x in p)
            {
                if (j == 1)
                {
                    PropuestasReferencias pr = ctx.PropuestasReferencias.Find(x.IdReferencia);
                    pr.Nombre = mp.NombreRef1;
                    pr.Telefono = mp.TelefonoRef1;
                    ctx.SaveChanges();
                }
                if (j == 2)
                {
                    PropuestasReferencias pr = ctx.PropuestasReferencias.Find(x.IdReferencia);
                    pr.Nombre = mp.NombreRef2;
                    pr.Telefono = mp.TelefonoRef2;
                    ctx.SaveChanges();
                }
                j++;

            }
            return RedirectToAction("ModificarPropuesta/" + mp.IdPropuesta);
        }

        /*-------------------Denuncias---------------------------*/
        [HttpGet]
        public ActionResult Denuncias()
        {
            List<PracticaEF.Data.Denuncias> lista = admin.GetDenuncias();
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
    }
}