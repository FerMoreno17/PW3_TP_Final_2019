using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class ModificarPropuesta
    {
        public int IdPropuesta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public System.DateTime FechaFin { get; set; }
        public string TelefonoContacto { get; set; }
        public int TipoDonacion { get; set; }
        public string Foto { get; set; }
        public int IdUsuarioCreador { get; set; }
        public int Estado { get; set; }
        public Nullable<decimal> Valoracion { get; set; }


        public int IdPropuestaDonacionMonetaria { get; set; }
        public decimal Dinero { get; set; }
        public string CBU { get; set; }


        public int IdPropuestaDonacionInsumo { get; set; }
        public string NombreItem { get; set; }
        public int Cantidad { get; set; }

        public int IdPropuestaDonacionHorasTrabajo { get; set; }
        public int CantidadHoras { get; set; }
        public string Profesion { get; set; }

        public int IdReferencia { get; set; }
        public string NombreRef { get; set; }
        public string TelefonoRef { get; set; }

        public int IdReferencia2 { get; set; }
        public string NombreRef2 { get; set; }
        public string TelefonoRef2 { get; set; }
    }
}