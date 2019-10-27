using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CrearPropuesta
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaFin { get; set; }
        public string TelefonoContacto { get; set; }
        public int TipoDonacion { get; set; }
        //public string Foto { get; set; }
        public int IdUsuarioCreador { get; set; }

    }
}