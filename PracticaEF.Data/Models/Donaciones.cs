using PracticaEF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Donaciones
    {
        public DateTime FechaDonacion { get; set; }
        public string Nombre { get; set; }
        public int TipoDonacion { get; set; }
        public int Estado { get; set; }
        public decimal TotalRecaudado { get; set; }
        public decimal MiDonacion { get; set; }
        public int IdUsuario { get; set; }
        public int IdPropuestaDIns { get; set; }
        public int IdPropuesta { get; set; }
        public List<PropuestasDonacionesInsumos> listin { get; set; }
        // yo todabia no hice la lista
        //yo hice una query re grande fue un re quilimbo devolverlo
        /* chaob voy a usar tu api ya fue 
         * como quieras ajaj ya  essit a me dcejas te la reuso ,usa lo que quieras no me molesta si queres 
         *c adale  sceguro lauso xq si no no llego 
         * asd igual ya esta creada solo crea el serviico y copiate la vista si si gracias chabon 
         * 
        aunque parezca mucho sin solo joins y te lo mete en la entidad DonacionAux
         * hice una query por cada tipo de donacion
           public List<DonacionAux> MisDonacionesId(int id)
        {
            Entities ctx = new Entities();
            List<DonacionAux> donacions = new List<DonacionAux>();

            #region Donaciones por Id Consulta
            var Donaciones = (from p in ctx.Propuestas
                              join p_mon in ctx.PropuestasDonacionesMonetarias
                               on p.IdPropuesta equals p_mon.IdPropuesta
                              join d_mon in ctx.DonacionesMonetarias
                               on p_mon.IdPropuestaDonacionMonetaria equals d_mon.IdPropuestaDonacionMonetaria
                              where d_mon.IdUsuario == id
                              select new DonacionAux
                              {
                                  Estado = p.Estado,
                                  FechaDonacion = d_mon.FechaCreacion,
                                  IdUsuario = d_mon.IdUsuario,
                                  MiDonacion = d_mon.Dinero,
                                  Nombre = p.Nombre,
                                  TipoDonacion = p.TipoDonacion,
                                  IdPropuesta = p.IdPropuesta
                              }
                             ).ToList();

            var DonacionesI = (from p in ctx.Propuestas
                               join p_in in ctx.PropuestasDonacionesInsumos
                                on p.IdPropuesta equals p_in.IdPropuesta
                               join d_in in ctx.DonacionesInsumos
                                on p_in.IdPropuestaDonacionInsumo equals d_in.IdPropuestaDonacionInsumo
                               where d_in.IdUsuario == id
                               select new DonacionAux
                               {
                                   Estado = p.Estado,
                                   FechaDonacion= (DateTime)d_in.FechaCreacion,
                                   IdUsuario = d_in.IdUsuario,
                                   MiDonacion = d_in.Cantidad,
                                   Nombre = p.Nombre,
                                   IdPropuestaDIns=p_in.IdPropuestaDonacionInsumo,
                                   TipoDonacion = p.TipoDonacion,
                                   IdPropuesta = p.IdPropuesta
                               }
                             ).ToList();
            var DonacionesHrs = (from p in ctx.Propuestas
                                 join p_hrs in ctx.PropuestasDonacionesHorasTrabajo
                                  on p.IdPropuesta equals p_hrs.IdPropuesta
                                 join d_hrs in ctx.DonacionesHorasTrabajo
                                  on p_hrs.IdPropuestaDonacionHorasTrabajo equals d_hrs.IdPropuestaDonacionHorasTrabajo
                                 where d_hrs.IdUsuario == id
                                 select new DonacionAux
                                 {
                                     Estado = p.Estado,
                                     FechaDonacion = (DateTime)d_hrs.FechaCreacion,
                                     IdUsuario = d_hrs.IdUsuario,
                                     MiDonacion = d_hrs.Cantidad,
                                     Nombre = p.Nombre,
                                     TipoDonacion = p.TipoDonacion,
                                     IdPropuesta = p.IdPropuesta
                                 }
                             ).ToList();
            #endregion

            donacions.AddRange(Donaciones);
            donacions.AddRange(DonacionesI);
            donacions.AddRange(DonacionesHrs);

            return CargarDonacionesTotalesALista(donacions);
        }
        CargarDonacionesTotalesALista->este metodo calcula el total donado de cada propuesta, Copialo ajja
        te instalo lo que falta en el proyecto?, para que funciones la api?
        nono, para que el proyecto lo pueda tomar MIRA, listo gracias papa, ni idea que hiciste ,pero gracias
        jajaja solo cambie el ajax, ah para me falta algo segui que lo busco
        segui haciendo el serviico , yo busco como pasarlo a json me falto eso porq ue devuelve XML
        perdon fui al baño jajaa
        */
    }
}