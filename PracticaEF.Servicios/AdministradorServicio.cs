using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Models;
using PracticaEF.Data;


namespace PracticaEF.Servicios
{
    public class AdministradorServicio
    {

        private readonly Entities1 ctx = new Entities1();
        private readonly PropuestaServicio prop = new PropuestaServicio();

        public Usuarios BuscarPorId(int id)
        {
            return ctx.Usuarios.SingleOrDefault(o => o.IdUsuario == id);
        }

        public Usuarios BuscarPorEmail (string email)
        {
            return ctx.Usuarios.SingleOrDefault(o => o.Email == email);
        }

        public void EnviarMail(Usuarios u)
        {
            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("confirmaciones@ayudandoalprojimo.org");
                correo.To.Add(u.Email);
                correo.Subject = "Activacion de Cuenta";
                correo.Body = HttpContext.Current.Request.Url.Scheme.ToString() + "://" + HttpContext.Current.Request.Url.Authority.ToString() + "/Sites/ActivarCuenta?token=" + u.Token;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                string scuentaCorreo = "ecommerce.mmda@gmail.com";
                string spasswordCorreo = "admin-123";

                smtp.Credentials = new System.Net.NetworkCredential(scuentaCorreo, spasswordCorreo);

                smtp.Send(correo);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        public void ActivarCuenta(string token)
        {
            Usuarios u = (Usuarios)(from usuario in ctx.Usuarios
                     where usuario.Token == token
                     select usuario).SingleOrDefault();
            u.Activo = true;
            ctx.SaveChanges();
        }

        public int CrearPropuestax(CrearPropuesta cp, HttpPostedFileBase file, Usuarios u)
        {
            Propuestas p = new Propuestas();

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/propuestas"), fileName);
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
                
                return cp.TipoDonacion;
            }
            return 0;
        }

        public int CargarPropuestaMonetaria(TipoDonacionMonetaria td,Usuarios u)
        {
            int usua = GetUltimaPropuestaCreada(u.IdUsuario);
            PropuestasDonacionesMonetarias pm = new PropuestasDonacionesMonetarias();
            pm.Dinero = td.Dinero;
            pm.CBU = td.CBU;
            pm.IdPropuesta = usua;
            ctx.PropuestasDonacionesMonetarias.Add(pm);
            ctx.SaveChanges();

            return usua;
        }

        public int CargarPropuestaInsumo(TipoDonacionInsumos td, Usuarios u)
        {
            int usua = GetUltimaPropuestaCreada(u.IdUsuario);
            PropuestasDonacionesInsumos pi = new PropuestasDonacionesInsumos();
            pi.Nombre = td.NombreInsumo;
            pi.Cantidad = td.Cantidad;
            pi.IdPropuesta = usua;
            ctx.PropuestasDonacionesInsumos.Add(pi);
            ctx.SaveChanges();
            return usua;
        }

        public int CargarPropuestaHorasTrabajo(TipoDonacionHorasTrabajo td, Usuarios u)
        {
            int usua = GetUltimaPropuestaCreada(u.IdUsuario);
            PropuestasDonacionesHorasTrabajo ph = new PropuestasDonacionesHorasTrabajo();
            ph.CantidadHoras = td.CantidadHoras;
            ph.Profesion = td.Profesion;
            ph.IdPropuesta = usua;
            ctx.PropuestasDonacionesHorasTrabajo.Add(ph);
            ctx.SaveChanges();
            return usua;
        }

        public int GetUltimaPropuestaCreada(int id)
        {
            var usua = (from prop in ctx.Propuestas
                        where prop.IdUsuarioCreador == id
                        select prop.IdPropuesta).ToList().LastOrDefault();
            return usua;
        }

        public void CargarReferencias(Referencia r)
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

        public List<Propuestas> GetMisPropuestas(int buscar)
        {
            return (from propuesta in ctx.Propuestas
                    where propuesta.IdUsuarioCreador == buscar
                    select propuesta).ToList();
        }

        public Propuestas GetPropuesta(int id)
        {
            //obtengo la propuesta que quiero ver detallada
            return (from p in ctx.Propuestas
                        where p.IdPropuesta == id
                        select p).FirstOrDefault();
        }

        public int GetPropuestasActivas(int id)
        {
            return (from p in ctx.Propuestas
                    where p.IdUsuarioCreador == id
                    select p).Count();
        }

        public int GetTotalDonacionesRecibidas(int id)
        {
            int mon = (from pm in ctx.PropuestasDonacionesMonetarias
                       join dm in ctx.DonacionesMonetarias
                       on pm.IdPropuestaDonacionMonetaria equals dm.IdPropuestaDonacionMonetaria
                       join p in ctx.Propuestas
                       on pm.IdPropuesta equals p.IdPropuesta
                       where p.IdUsuarioCreador == id
                       select dm).Count();
            int ins = (from pi in ctx.PropuestasDonacionesInsumos
                       join di in ctx.DonacionesInsumos
                       on pi.IdPropuestaDonacionInsumo equals di.IdPropuestaDonacionInsumo
                       join p in ctx.Propuestas
                       on pi.IdPropuesta equals p.IdPropuesta
                       where p.IdUsuarioCreador == id
                       select di).Count();
            int hst = (from phst in ctx.PropuestasDonacionesHorasTrabajo
                       join dhst in ctx.DonacionesHorasTrabajo
                       on phst.IdPropuestaDonacionHorasTrabajo equals dhst.IdPropuestaDonacionHorasTrabajo
                       join p in ctx.Propuestas
                       on phst.IdPropuesta equals p.IdPropuesta
                       where p.IdUsuarioCreador == id
                       select dhst).Count();
            int total = mon + ins + hst;
            return total;
        }

        public int ContarPropuestasActivas(int id)
        {
            int cantidad = (from p in ctx.Propuestas
                            where p.IdUsuarioCreador == id
                            && p.Estado == 1
                            select p).Count();
            return cantidad;
        }

        public Object GetTipoPropuesta(int tp, int idProp)
        {//obtengo el id del tipo de propuesta para poder traer la lista de donaciones en el siguiente paso
            if(tp == 1)
            {
                return (from prop in ctx.PropuestasDonacionesMonetarias
                        where prop.IdPropuesta == idProp
                        select prop).FirstOrDefault();
            }
            if (tp == 2)
            {
                return (from prop in ctx.PropuestasDonacionesInsumos
                        where prop.IdPropuesta == idProp
                        select prop).FirstOrDefault();
            }
            if (tp == 3)
            {
                return (from prop in ctx.PropuestasDonacionesHorasTrabajo
                        where prop.IdPropuesta == idProp
                        select prop).FirstOrDefault();
            }
            return 0;
        }

        public Object GetDonacionesPropuesta(int tp, int idTP)
        {
            //tp= tipo de propuesta // idTP= id del tipo de propuesta
            //propuesta monetaria
            if(tp == 1)
            {
                return (from donaciones in ctx.DonacionesMonetarias
                         where donaciones.IdPropuestaDonacionMonetaria == idTP
                         select donaciones).ToList();
            }
            if (tp == 2)
            {
                return (from donaciones in ctx.DonacionesInsumos
                        where donaciones.IdPropuestaDonacionInsumo == idTP
                        select donaciones).ToList();
            }
            if (tp == 3)
            {
                return (from donaciones in ctx.DonacionesHorasTrabajo
                        where donaciones.IdPropuestaDonacionHorasTrabajo == idTP
                        select donaciones).ToList();
            }
            return 0;
            /*return (from propuesta in ctx.Propuestas
             where propuesta.IdUsuarioCreador == buscar
             select propuesta).ToList();*/
        }

        public ModificarPropuesta GetModificarPropuestaMonetaria(Propuestas p,int id)
        {
            var tp = (PropuestasDonacionesMonetarias) prop.GetPropuestaDonacion(id, p.TipoDonacion);
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
            return mp;
        }

        public ModificarPropuesta GetModificarPropuestaInsumo(Propuestas p, int id)
        {
            var tp = (PropuestasDonacionesInsumos) prop.GetPropuestaDonacion(id, p.TipoDonacion);
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
            return mp;
        }

        public ModificarPropuesta GetModificarPropuestaHorasTrabajo(Propuestas p, int id)
        {
            var tp = (PropuestasDonacionesHorasTrabajo)prop.GetPropuestaDonacion(id, p.TipoDonacion);
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
            return mp;
        }

        public void ModificarPropuesta(ModificarPropuesta mp, HttpPostedFileBase file)
        {
            Propuestas p = ctx.Propuestas.Find(mp.IdPropuesta);

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/propuestas"), fileName);
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
        }

        public void ModificarPropuestaCasoIndividual(ModificarPropuesta mp)
        {
            if(mp.TipoDonacion == 1)
            {
                PropuestasDonacionesMonetarias p = ctx.PropuestasDonacionesMonetarias.Find(mp.IdPropuestaDonacionMonetaria);
                p.Dinero = mp.Dinero;
                p.CBU = mp.CBU;
                ctx.SaveChanges();
            }
            if(mp.TipoDonacion == 2)
            {
                PropuestasDonacionesInsumos m = ctx.PropuestasDonacionesInsumos.Find(mp.IdPropuestaDonacionInsumo);
                m.Nombre = mp.NombreItem;
                m.Cantidad = mp.Cantidad;
                ctx.SaveChanges();
            }
            if(mp.TipoDonacion == 3)
            {
                PropuestasDonacionesHorasTrabajo p = ctx.PropuestasDonacionesHorasTrabajo.Find(mp.IdPropuestaDonacionHorasTrabajo);
                p.CantidadHoras = mp.CantidadHoras;
                if (mp.Profesion != "")
                {
                    p.Profesion = mp.Profesion;
                }
                ctx.SaveChanges();
            }
            
        }

        public void ModificarReferencias(ModificarPropuesta mp)
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
        }

        public List<Denuncias> GetDenuncias()
        {
            return (from d in ctx.Denuncias
             where d.Estado == 1
             orderby d.IdPropuesta ascending
             select d).ToList();
        }

        public void AceptarDenuncia(int id)
        {
            Propuestas p = ctx.Propuestas.Find(id);
            p.Estado = 2;
            ctx.SaveChanges();
            AceptarDenunciaPonerEnEstado2Desactivada(id);
        }

        public void IgnorarDenuncia(int id)
        {
            List<PracticaEF.Data.Denuncias> lista = (from d in ctx.Denuncias
                                                     where d.IdPropuesta == id && d.Estado == 1
                                                     select d).ToList();
            foreach (var o in lista)
            {
                o.Estado = 0;
            }
            ctx.SaveChanges(); 
        }

        public void AceptarDenunciaPonerEnEstado2Desactivada(int id)
        {
            List<Denuncias> lista = (from d in ctx.Denuncias
                                                     where d.IdPropuesta == id
                                                     select d).ToList();
            foreach (var o in lista)
            {
                o.Estado = 0;
            }
            ctx.SaveChanges();
        }
    }
}

