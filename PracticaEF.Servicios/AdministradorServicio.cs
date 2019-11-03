using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

