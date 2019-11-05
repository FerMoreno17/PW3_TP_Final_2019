using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using PracticaEF.Data;



namespace PracticaEF.Servicios
{
    public class AdministradorServicio
    {

        private readonly Entities ctx = new Entities();
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

        public List<Propuestas> getMisPropuestas(int buscar)
        {
            return (from propuesta in ctx.Propuestas
                    where propuesta.IdUsuarioCreador == buscar
                    select propuesta).ToList();
        }

        public Propuestas getPropuesta(int id)
        {
            //obtengo la propuesta que quiero ver detallada
            return (from p in ctx.Propuestas
                        where p.IdPropuesta == id
                        select p).FirstOrDefault();
        }

        public int ContarPropuestasActivas(int id)
        {
            int cantidad = (from p in ctx.Propuestas
                            where p.IdUsuarioCreador == id
                            && p.Estado == 1
                            select p).Count();
            return cantidad;
        }

        public Object getTipoPropuesta(int tp, int idProp)
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

        public Object getDonacionesPropuesta(int tp, int idTP)
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

        public List<PracticaEF.Data.Denuncias> GetDenuncias()
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
            List<PracticaEF.Data.Denuncias> lista = (from d in ctx.Denuncias
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

