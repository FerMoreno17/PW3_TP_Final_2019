using PracticaEF.Data;
using PracticaEF.Servicios;
using System.Web.Mvc;
using System.Linq;
using Models;
using System.IO;
using System.Web;
using System;

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
            if(Session["session"] == null)
            {
                return RedirectToAction("../Index/Inicio");
            }
            else
            {
            return View();

            }
        }
        
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
                    if(aCambiar.UserName == null)
                    {
                        aCambiar.UserName = recibido.Nombre + recibido.Apellido;
                    }

                    ctx.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MotivoError = "No se hemos podido modficar tu perfil.";
                    return View("/Shared/Error");
                }
            }
            ViewBag.MotivoError = "El formato de modelo no es válido.";
            return View("/Shared/Error");
        }

        
        public ActionResult Propuestas()
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

        [HttpGet]
        public ActionResult CrearPropuesta()
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

                    if(cp.TipoDonacion == 1)
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
            return View();
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
            return View();

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
            return View();
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