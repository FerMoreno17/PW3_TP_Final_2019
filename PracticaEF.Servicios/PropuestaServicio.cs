using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.SessionState;
using System.Threading.Tasks;
using System.Web.Mvc;
using PracticaEF.Data;
using System.Web;
using System.IO;

namespace PracticaEF.Servicios
{
    public class PropuestaServicio
    {
        Propuestas prop = new Propuestas();

        private readonly Entities ctx = new Entities();

        public void ValorarPropuesta(FormCollection form)
        {
            PropuestasValoraciones pv = new PropuestasValoraciones();
            pv.IdUsuario = Convert.ToInt32(form["IdUsuario"]);
            pv.IdPropuesta = Convert.ToInt32(form["IdPropuesta"]);
            if (Convert.ToInt32(form["Valoracion"]) == 1)
            {
                pv.Valoracion = true;
            }
            else
            {
                pv.Valoracion = false;
            }
            ctx.PropuestasValoraciones.Add(pv);
            ctx.SaveChanges();
        }

        public List<DenunciasMotivos> GetMotivos()
        {
            return ctx.DenunciasMotivos.ToList();
        }

        public void CargarDenuncia(FormCollection form)
        {
            Denuncias dm = new Denuncias();
            dm.IdPropuesta = Convert.ToInt32(form["IdPropuesta"]);
            dm.IdMotivo = Convert.ToInt32(form["IdMotivo"]);
            dm.Comentarios = form["Comentarios"];
            dm.IdUsuario = Convert.ToInt32(form["IdUsuario"]);
            dm.FechaCreacion = DateTime.Today;
            dm.Estado = 1;
            ctx.Denuncias.Add(dm);
            ctx.SaveChanges();
        }

        public Propuestas getPropuesta(int id)
        {
           return (from propuesta in ctx.Propuestas
             where propuesta.IdPropuesta == id
             select propuesta).SingleOrDefault();
        }

        public Object getPropuestaDonacion(int id, int prop)
        {
           
            if(id == 1)
            {
                return (from propuesta in ctx.PropuestasDonacionesMonetarias
                        where propuesta.IdPropuesta == prop
                        select propuesta).SingleOrDefault();
            }
            if (id == 2)
            {
                return (from propuesta in ctx.PropuestasDonacionesInsumos
                        where propuesta.IdPropuesta == prop
                        select propuesta).SingleOrDefault();
            }
            if (id == 3)
            {
                return (from propuesta in ctx.PropuestasDonacionesHorasTrabajo
                        where propuesta.IdPropuesta == prop
                        select propuesta).SingleOrDefault();
            }
            return "error";

        }

        public Object getTotalDonacion(object o, int tp)
        {
            
            if (tp == 1)
            {
                PropuestasDonacionesMonetarias pd = (PropuestasDonacionesMonetarias) o;
                int idProp = pd.IdPropuestaDonacionMonetaria;
                try
                {
                    return (from d in ctx.DonacionesMonetarias
                            where d.IdPropuestaDonacionMonetaria == idProp
                            select d.Dinero).Sum();
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
            if (tp == 2)
            {
                PropuestasDonacionesInsumos pd = (PropuestasDonacionesInsumos)o;
                int idProp = pd.IdPropuestaDonacionInsumo;
                try
                {
                    return (from d in ctx.DonacionesInsumos
                            where d.IdPropuestaDonacionInsumo == idProp
                            select d.Cantidad).Sum();
                }
                catch (Exception)
                {
                    return 0;
                }
                
            }
            if (tp == 3)
            {
                PropuestasDonacionesHorasTrabajo pd = (PropuestasDonacionesHorasTrabajo)o;
                int idProp = pd.IdPropuestaDonacionHorasTrabajo;
                try
                {
                    return (from d in ctx.DonacionesHorasTrabajo
                            where d.IdPropuestaDonacionHorasTrabajo == idProp
                            select d.Cantidad).Sum();
                }
                catch (Exception)
                {
                    return 0;
                }
                
            }
            return "error";
        }

        public DonacionesInsumos getInsumos(int id)
        {
            return (from d in ctx.DonacionesInsumos
                        where d.IdPropuestaDonacionInsumo == id
                        select d).FirstOrDefault();
        }

        public int CargarDonacion(int k, FormCollection form, HttpPostedFileBase file)
        {
            if (k == 1)
            {
                DonacionesMonetarias d = new DonacionesMonetarias();
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/donaciones"), fileName);
                    file.SaveAs(path);

                    d.ArchivoTransferencia = fileName;
                    d.IdPropuestaDonacionMonetaria = Convert.ToInt32(form["IdPropuestaDonacionMonetaria"]);
                    d.IdUsuario = Convert.ToInt32(form["IdUsuario"]);
                    d.Dinero = Convert.ToInt32(form["Dinero"]);
                    d.FechaCreacion = DateTime.Today;
                    ctx.DonacionesMonetarias.Add(d);
                    ctx.SaveChanges();
                }
                return 1;
            }
            if (k == 2)
            {
                DonacionesInsumos d = new DonacionesInsumos();
                d.IdPropuestaDonacionInsumo = Convert.ToInt32(form["IdPropuestaDonacionInsumo"]);
                d.IdUsuario = Convert.ToInt32(form["IdUsuario"]);
                d.Cantidad = Convert.ToInt32(form["Cantidad"]);
                ctx.DonacionesInsumos.Add(d);
                ctx.SaveChanges();
                return 1;
            }
            if (k == 3)
            {
                DonacionesHorasTrabajo d = new DonacionesHorasTrabajo();
                d.IdPropuestaDonacionHorasTrabajo = Convert.ToInt32(form["IdPropuestaDonacionHorasTrabajo"]);
                d.IdUsuario = Convert.ToInt32(form["IdUsuario"]);
                d.Cantidad = Convert.ToInt32(form["Cantidad"]);
                ctx.DonacionesHorasTrabajo.Add(d);
                ctx.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}
