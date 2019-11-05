using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class DenunciasPropuestas
    {
        public int IdDenuncia { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaCreacionDen { get; set; }
        public int EstadoDen { get; set; }

        public int IdPropuesta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaCreacionProp { get; set; }
        public System.DateTime FechaFin { get; set; }
        public string TelefonoContacto { get; set; }
        public int TipoDonacion { get; set; }
        public string Foto { get; set; }
        public int IdUsuarioCreador { get; set; }
        public int EstadoProp { get; set; }
        public Nullable<decimal> Valoracion { get; set; }

        public int IdMotivo { get; set; }
        public string Detalle { get; set; }
    }
}