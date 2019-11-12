using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using PracticaEF.Data;


namespace PracticaEF.Servicios
{
    public class DonacionService
    {
       
        public List<DonacionApi> MisDonacionesId(int id)
        {
            Entities1 ctx = new Entities1();
            List<DonacionApi> donacions = new List<DonacionApi>();
           
            #region Donaciones por Id Consulta
            var Donaciones = (from p in ctx.Propuestas
                              join p_mon in ctx.PropuestasDonacionesMonetarias
                               on p.IdPropuesta equals p_mon.IdPropuesta
                              join d_mon in ctx.DonacionesMonetarias
                               on p_mon.IdPropuestaDonacionMonetaria equals d_mon.IdPropuestaDonacionMonetaria
                              where d_mon.IdUsuario == id
                              select new DonacionApi
                              {
                                  Estado = p.Estado,
                                  FechaDonacion = d_mon.FechaCreacion,
                                  IdUsuario = d_mon.IdUsuario,
                                  MiDonacion = d_mon.Dinero,
                                  Nombre = p.Nombre,
                                  Tipo = p.TipoDonacion,
                                  IdPropuesta = p.IdPropuesta
                              }
                              ).ToList();

            var DonacionesI = (from p in ctx.Propuestas
                               join p_in in ctx.PropuestasDonacionesInsumos
                                on p.IdPropuesta equals p_in.IdPropuesta
                               join d_in in ctx.DonacionesInsumos
                                on p_in.IdPropuestaDonacionInsumo equals d_in.IdPropuestaDonacionInsumo
                               where d_in.IdUsuario == id
                               select new DonacionApi
                               {
                                   Estado = p.Estado,
                                   FechaDonacion = d_in.FechaCreacion,
                                   IdUsuario = d_in.IdUsuario,
                                   MiDonacion = d_in.Cantidad,
                                   Nombre = p.Nombre,
                                   IdPropuestaDIns = p_in.IdPropuestaDonacionInsumo,
                                   Tipo = p.TipoDonacion,
                                   IdPropuesta = p.IdPropuesta
                               }
                             ).ToList();
            var DonacionesHrs = (from p in ctx.Propuestas
                                 join p_hrs in ctx.PropuestasDonacionesHorasTrabajo
                                  on p.IdPropuesta equals p_hrs.IdPropuesta
                                 join d_hrs in ctx.DonacionesHorasTrabajo
                                  on p_hrs.IdPropuestaDonacionHorasTrabajo equals d_hrs.IdPropuestaDonacionHorasTrabajo
                                 where d_hrs.IdUsuario == id
                                 select new DonacionApi
                                 {
                                     Estado = p.Estado,
                                     FechaDonacion = d_hrs.FechaCreacion,
                                     IdUsuario = d_hrs.IdUsuario,
                                     MiDonacion = d_hrs.Cantidad,
                                     Nombre = p.Nombre,
                                     Tipo = p.TipoDonacion,
                                     IdPropuesta = p.IdPropuesta
                                 }
                             ).ToList();
            #endregion

            donacions.AddRange(Donaciones);
            donacions.AddRange(DonacionesI);
            donacions.AddRange(DonacionesHrs);

            return CargarDonacionesTotalesALista(donacions);
        }

        public List<DonacionApi> CargarDonacionesTotalesALista(List<DonacionApi> list)
        {
            foreach (DonacionApi item in list)
            {//1->Monetaria   2->Insumos   3->HorasDeTrabajo
                if (item.Tipo == 1)
                {
                    item.TotalRecaudado = CalcularTotalDonadoPropuestaMon(item.IdPropuesta);
                }
                else if (item.Tipo == 2)
                {
                    item.TotalRecaudado = CalcularTotalDonadoPropuestaIns(item.IdPropuestaDIns);
                }
                else if (item.Tipo == 3)
                {
                    item.TotalRecaudado = CalcularTotalDonadoPropuestaHrs(item.IdPropuesta);
                }
            }

            return list.OrderByDescending(model => model.FechaDonacion).ToList();
        }

        public decimal CalcularTotalDonadoPropuestaMon(int id)
        {
            Entities1 ctx = new Entities1();

            decimal contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesMonetarias
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesMonetarias
                          on p_in.IdPropuestaDonacionMonetaria equals d_in.IdPropuestaDonacionMonetaria
                         where p.IdPropuesta == id
                         select d_in.Dinero
                          ).ToList();
            if (dlist.Count > 0)
            {
                foreach (decimal item in dlist)
                {
                    contTotal += item;
                }
                return contTotal;
            }
            else return 0;
        }
        public int CalcularTotalDonadoPropuestaIns(int id)
        {
            Entities1 ctx = new Entities1();
            int contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesInsumos
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesInsumos
                          on p_in.IdPropuestaDonacionInsumo equals d_in.IdPropuestaDonacionInsumo
                         where p_in.IdPropuestaDonacionInsumo == id
                         select d_in.Cantidad
                             ).ToList();


            foreach (int item in dlist)
            {
                contTotal += item;
            }
            return contTotal;

        }
        public int CalcularTotalDonadoPropuestaHrs(int id)
        {
            Entities1 ctx = new Entities1();

            int contTotal = 0;
            var dlist = (from p in ctx.Propuestas
                         join p_in in ctx.PropuestasDonacionesHorasTrabajo
                          on p.IdPropuesta equals p_in.IdPropuesta
                         join d_in in ctx.DonacionesHorasTrabajo
                          on p_in.IdPropuestaDonacionHorasTrabajo equals d_in.IdPropuestaDonacionHorasTrabajo
                         where p.IdPropuesta == id
                         select d_in.Cantidad
                             ).ToList();

            if (dlist.Count > 0)
            {
                foreach (int item in dlist)
                {
                    contTotal += item;
                }
                return contTotal;
            }
            else return 0;
        }
    }
}